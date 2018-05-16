using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour {

	[Header ("Map Properties")]
	public int seed;
	public Vector2 offset;
	public int mapSize;
	public float noiseScale;
	public int octaves;
	[Range (0.00001f, 1f)]
	public float persistence;
	public float lacunarity;
	[Header ("Editor Properties")]
	public bool autoUpdate;

	private GameObject displayMesh;

	public void GenerateTextureMap () {
		string msg = "Map Generation Complete. Took: ";
		float startTime = Time.time;
		//Mesh creation
		if (displayMesh == null)
			displayMesh = GameObject.CreatePrimitive (PrimitiveType.Plane);
		displayMesh.transform.localScale = new Vector3 (mapSize, 1, mapSize);
		displayMesh.transform.parent = transform;
		//Texture creation
		Texture2D tex = new Texture2D (mapSize, mapSize);
		var map = MapGenerator.GenerateMap (mapSize, noiseScale, seed, octaves, lacunarity, persistence, offset);
		Color[] colors = new Color[mapSize * mapSize];
		for (int y = 0; y < mapSize; y++) {
			for (int x = 0; x < mapSize; x++) {
				colors[y * mapSize + x] = Color.Lerp (Color.black, Color.white, map[x, y]);
			}
		}
		tex.SetPixels (colors);
		tex.Apply ();
		displayMesh.GetComponent<Renderer> ().sharedMaterial = Resources.Load<Material> ("Materials/texture_material");
		displayMesh.GetComponent<Renderer> ().sharedMaterial.mainTexture = tex;

		float duration = Time.time - startTime;
		msg += (duration).ToString () + " seconds";
		//Debug.Log (msg);
	}
	public void GenerateMeshMap () {
		//Mesh creation
		if (displayMesh == null)
			displayMesh = GameObject.CreatePrimitive (PrimitiveType.Plane);
		displayMesh.transform.localScale = new Vector3 (mapSize, 1, mapSize);
		displayMesh.transform.parent = transform;
		//Material operations
		displayMesh.GetComponent<Renderer> ().sharedMaterial = Resources.Load<Material> ("Materials/mesh_material");
		var map = MapGenerator.GenerateMap (mapSize, noiseScale, seed, octaves, lacunarity, persistence, offset);
		
	}

	public void OnValidate () {
		if (mapSize < 1)
			mapSize = 1;
		else if (noiseScale <= 1)
			noiseScale = 1.0001f;
		else if (octaves < 1)
			octaves = 1;
		else if (lacunarity <= 0)
			lacunarity = 0.0001f;
	}
}