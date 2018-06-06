using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapMath {

	public static float[, ] GenerateHeights(int w) {
		float[, ] heights = new float[w, w];
		int x;
		int y;
		for (x = 0; x < w; x++) {
			for (y = 0; y < w; y++) {
				heights[x, y] = Mathf.PerlinNoise(x / w, y / w);
			}
		}

		return heights;
	}

}