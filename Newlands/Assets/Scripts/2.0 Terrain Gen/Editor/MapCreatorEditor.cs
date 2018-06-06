using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapCreator))]
public class MapCreatorEditor : Editor {
	public override void OnInspectorGUI() {
		base.DrawDefaultInspector();
		if (GUILayout.Button("Generate Map")) {
			((MapCreator) target).GenerateChunk(Vector3.zero);
		}
	}
}