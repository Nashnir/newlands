using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapGenerator {

	public static float[, ] GenerateMap (int size, int octaves, float lacunarity, float persistence) {
		float[, ] map = new float[size, size];
		for (int x = 0; x < size; x++) {
			for (int y = 0; y < size; y++) {
				for (int n = 0; n < octaves; n++) {
					map[x, y] += Mathf.PerlinNoise (x / Mathf.Pow (lacunarity, n), y / Mathf.Pow (lacunarity, n)) * Mathf.Pow (persistence, n);
				}
			}
		}
		return map;
	}
	public static Color[] GeneratePixelMap (int size, float scale, int octaves, float lacunarity, float persistence) {
		Color[] colorMap = new Color[size * size];
		float[] map = new float[size * size];

		var lowestValue = float.MaxValue;
		var highestValue = float.MinValue;

		for (int x = 0; x < size; x++) {
			for (int y = 0; y < size; y++) {

				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;

				for (int n = 0; n < octaves; n++) {
					float sampleX = x / scale * frequency;
					float sampleY = y / scale * frequency;

					float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
					noiseHeight += perlinValue * amplitude;

					amplitude *= persistence;
					frequency *= lacunarity;
				}
				if (noiseHeight < lowestValue)
					lowestValue = noiseHeight;
				else if (noiseHeight > highestValue)
					highestValue = noiseHeight;

				map[x * size + y] = noiseHeight;
			}
		}
		for (int x = 0; x < size; x++) {
			for (int y = 0; y < size; y++) {
				var value = Mathf.InverseLerp (lowestValue, highestValue, map[x * size + y]);
				colorMap[x * size + y] = new Color (value, value, value);
			}
		}
		return colorMap;
	}
}