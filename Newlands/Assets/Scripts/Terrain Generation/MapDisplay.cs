using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour {

	[Header ("Map Properties")]
	public int mapSize;
	public float mapScale;
	public int octaves;
	public float persistence;
	[Range (0, 1)]
	public float lacunarity;
	[Header ("Editor Properties")]
	public bool autoUpdate;

	private GameObject displayMesh;
	public void GenerateMap () {
		if (displayMesh == null)
			displayMesh = GameObject.CreatePrimitive (PrimitiveType.Plane);
		displayMesh.transform.localScale = new Vector3 (mapSize, 1, mapSize);
		Texture2D tex = new Texture2D (mapSize, mapSize);
		tex.SetPixels (MapGenerator.GeneratePixelMap (mapSize, mapScale, octaves, lacunarity, persistence));
		tex.Apply ();
		displayMesh.GetComponent<Renderer> ().sharedMaterial.mainTexture = tex;
	}
}