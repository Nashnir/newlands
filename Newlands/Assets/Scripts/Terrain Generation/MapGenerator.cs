using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapGenerator {

	public static float[, ] GenerateMap(int size, float scale, int seed, int octaves, float lacunarity, float persistence, Vector2 octave_offset, Vector2 map_offset, MapDisplay.NormalizeMode normalizeMode) {
		float[, ] map = new float[size, size];

		float maxValue = float.MinValue;
		float minValue = float.MaxValue;

		System.Random prng = new System.Random(seed);
		Vector2[] octaveOffset = new Vector2[octaves];
		float maxPossibleHeight = 0f;
		float amplitude0 = 1;

		for (int i = 0; i < octaves; i++) {
			octaveOffset[i] = new Vector2(prng.Next(-10000, 10000) + octave_offset.x, prng.Next(-100000, 100000) + octave_offset.y);

			maxPossibleHeight += amplitude0;
			amplitude0 *= persistence;
		}
		//calculate values
		for (int x = 0; x < size; x++) {
			for (int y = 0; y < size; y++) {

				float amplitude = 1;
				float frequency = 1;
				float value = 0;

				for (int i = 0; i < octaves; i++) {
					float sampleX = (x - size / 2 + map_offset.x) / scale * frequency + octaveOffset[i].x;
					float sampleY = (y - size / 2 + map_offset.y) / scale * frequency + octaveOffset[i].y;

					float perlinValue = Mathf.PerlinNoise(sampleX * frequency, sampleY * frequency) * amplitude * 2 - 1;
					value += perlinValue * amplitude;

					amplitude *= persistence;
					frequency *= lacunarity;
				}
				//Keeping track of min and max values
				if (value < minValue)
					minValue = value;
				else if (value > maxValue)
					maxValue = value;

				map[x, y] = value;
			}
		}
		//normalize values
		for (int x = 0; x < size; x++) {
			for (int y = 0; y < size; y++) {
				if (normalizeMode == MapDisplay.NormalizeMode.Local) {
					map[x, y] = Mathf.InverseLerp(minValue, maxValue, map[x, y]);
				} else {
					map[x, y] = (map[x, y] + 1) / (2f * maxPossibleHeight / 3.75f);
				}
			}
		}

		return map;
	}
}