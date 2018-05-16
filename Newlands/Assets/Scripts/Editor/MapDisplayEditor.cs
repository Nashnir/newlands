using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapDisplay))]
[CanEditMultipleObjects]
public class MapDisplayEditor : Editor {

	MapDisplay mapDisplay;

	public override void OnInspectorGUI() {
		mapDisplay = (MapDisplay) target;

		if (DrawDefaultInspector()) {
			if (mapDisplay.autoUpdate) {
				if (mapDisplay.mode == MapDisplay.RenderMode.texture) {
					mapDisplay.GenerateTextureMap();
				} else if (mapDisplay.mode == MapDisplay.RenderMode.mesh) {
					mapDisplay.GenerateMeshMap();
				}
			}
		}
		if (GUILayout.Button("Generate Texture Map")) {
			mapDisplay.GenerateTextureMap();
		} else if (GUILayout.Button("Generate Mesh Map")) {
			mapDisplay.GenerateMeshMap();
		}
	}
}