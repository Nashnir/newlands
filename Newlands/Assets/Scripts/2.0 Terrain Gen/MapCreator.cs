using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MapCreator : MonoBehaviour {

    private List<TerrainChunk> chunks = new List<TerrainChunk>();

    public Transform container;
    public GenerationMode mode = GenerationMode.single;
    public int mapWidth = 100;

    public TerrainChunk GenerateChunk(Vector2 pos) {
        var terrainData = new TerrainData();
        var heights = MapMath.GenerateHeights(mapWidth);
        terrainData.heightmapResolution = mapWidth;
        terrainData.SetHeights(0, 0, heights);
        terrainData.name = "generated_terrain";
        var terrainObj = Terrain.CreateTerrainGameObject(terrainData);
        Instantiate(terrainObj, pos, Quaternion.identity, container);
        return AddChunk(new TerrainChunk { terrainObject = terrainObj, terrain = terrainObj.GetComponent<Terrain>(), data = terrainData, position = pos });
    }

    private TerrainChunk AddChunk(TerrainChunk chunk) {
        chunks.Add(chunk);
        return chunk;
    }

    public struct TerrainChunk {
        public GameObject terrainObject;
        public Terrain terrain;
        public TerrainData data;
        public Vector2 position;
    }

    public enum GenerationMode {
        single,
        multiple,
        infinite
    }

}