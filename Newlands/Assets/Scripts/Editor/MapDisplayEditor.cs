using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (MapDisplay))]
public class MapDisplayEditor : Editor {

	MapDisplay mapDisplay;

	public override void OnInspectorGUI () {
		mapDisplay = (MapDisplay) target;

		if (DrawDefaultInspector ()) {
			if (mapDisplay.autoUpdate) {
				mapDisplay.GenerateMap ();
			}
		}
		if (GUILayout.Button ("Generate Map")) {
			mapDisplay.GenerateMap ();
		}
	}

	public void Onvalidate () {

	}
}