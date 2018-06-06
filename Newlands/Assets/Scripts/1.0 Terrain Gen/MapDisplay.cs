using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour {

	[Header("Map Properties")]
	public int seed;
	public Vector2 octaveOffset;
	public Vector2 mapOffset;
	public int mapSize;
	public float noiseScale;
	public int octaves;
	[Range(0.00001f, 1f)]
	public float persistence;
	[Range(1, 4)]
	public float lacunarity;
	[Header("3D Properties")]
	public float mapAmplitude;
	public AnimationCurve heightProfile;
	[Header("Color Properties")]
	public TerrainType[] regions;
	[Header("Editor Properties")]
	public RenderMode mode;
	public bool autoUpdate;
	public NormalizeMode normalizeMode;

	private GameObject displayMesh;

	private void Start() {
		GenerateMeshMap();
	}

	private Texture2D TextureFromColorMap(Color[] colorMap) {
		Texture2D texture = new Texture2D(mapSize, mapSize);
		texture.SetPixels(colorMap);
		texture.name = "generated_texture";
		texture.Apply();
		return texture;
	}
	private Texture2D TextureFromHeightMap(float[, ] heightMap) {
		Color[] colors = new Color[mapSize * mapSize];
		for (int y = 0; y < mapSize; y++) {
			for (int x = 0; x < mapSize; x++) {
				colors[y * mapSize + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
			}
		}
		return TextureFromColorMap(colors);
	}

	public void GenerateTextureMap() {
		string msg = "Map Generation Complete. Took: ";
		float startTime = Time.time;
		//Mesh creation
		if (displayMesh == null)
			displayMesh = GameObject.CreatePrimitive(PrimitiveType.Plane);
		displayMesh.transform.localScale = new Vector3(mapSize, 1, mapSize);
		displayMesh.transform.parent = transform;
		//Texture creation
		var map = MapGenerator.GenerateMap(mapSize, noiseScale, seed, octaves, lacunarity, persistence, octaveOffset, mapOffset, normalizeMode);
		Texture2D tex = TextureFromHeightMap(map);
		displayMesh.GetComponent<Renderer>().sharedMaterial = Resources.Load<Material>("Materials/texture_material");
		displayMesh.GetComponent<Renderer>().sharedMaterial.mainTexture = tex;

		float duration = Time.time - startTime;
		msg += (duration).ToString() + " seconds";
		//Debug.Log (msg);
	}
	public void GenerateMeshMap() {
		if (displayMesh == null)
			displayMesh = GameObject.CreatePrimitive(PrimitiveType.Plane);
		displayMesh.transform.parent = transform;
		displayMesh.transform.localScale = new Vector3(1f, 1f, 1f);

		var map = MapGenerator.GenerateMap(mapSize, noiseScale, seed, octaves, lacunarity, persistence, octaveOffset, mapOffset, normalizeMode);
		Mesh mesh = new Mesh();
		Vector3[] vertices = new Vector3[mapSize * mapSize];
		int[] triangles = new int[(mapSize - 1) * (mapSize - 1) * 6];
		Vector2[] uv = new Vector2[mapSize * mapSize];
		Color[] colorMap = new Color[mapSize * mapSize];
		//Setting vertices and triangles
		int vertexIndex = 0;
		int triangleIndex = 0;
		for (int y = 0; y < mapSize; y++) {
			for (int x = 0; x < mapSize; x++) {
				//Colors
				for (int i = 0; i < regions.Length; i++) {
					if (map[x, y] <= regions[i].height) {
						colorMap[y * mapSize + x] = regions[i].color;
						break;
					}
				}
				//Mesh
				vertices[vertexIndex] = new Vector3(x - (mapSize - 1) / 2f, map[x, y] * mapAmplitude * heightProfile.Evaluate(map[x, y]), y - (mapSize - 1) / 2f);
				uv[vertexIndex] = new Vector2(x / (float) mapSize, y / (float) mapSize);

				if (x < mapSize - 1 && y < mapSize - 1) {
					//1st triangle
					triangles[triangleIndex + 2] = vertexIndex;
					triangles[triangleIndex + 1] = vertexIndex + 1;
					triangles[triangleIndex + 0] = vertexIndex + mapSize + 1;
					//2nd triangle
					triangles[triangleIndex + 5] = vertexIndex;
					triangles[triangleIndex + 4] = vertexIndex + mapSize + 1;
					triangles[triangleIndex + 3] = vertexIndex + mapSize;

					triangleIndex += 6;
				}

				vertexIndex++;
			}
		}
		//Setting mesh
		mesh.name = "procedural_terrain_mesh";
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uv;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();

		displayMesh.GetComponent<MeshFilter>().sharedMesh = mesh;
		displayMesh.GetComponent<MeshCollider>().sharedMesh = mesh;
		//Material operations
		displayMesh.GetComponent<Renderer>().sharedMaterial = Resources.Load<Material>("Materials/mesh_material");
		displayMesh.GetComponent<Renderer>().sharedMaterial.SetTexture("_BaseColorMap", TextureFromColorMap(colorMap));
	}

	public void OnValidate() {
		if (mapSize < 1)
			mapSize = 1;
		else if (noiseScale <= 1)
			noiseScale = 1.0001f;
		else if (octaves < 1)
			octaves = 1;
		else if (lacunarity <= 0)
			lacunarity = 0.0001f;
	}

	public enum RenderMode {
		texture,
		mesh
	}
	public enum NormalizeMode {
		Local,
		Global
	}
}

[System.Serializable]
public struct TerrainType {
	public string name;
	public float height;
	public Color color;
}