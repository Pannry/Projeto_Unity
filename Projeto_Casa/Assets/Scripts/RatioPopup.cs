using System;
using UnityEditor;
using UnityEngine;
using System.Collections;


namespace AssemblyCSharp
{
	public class RatioPopup: EditorWindow
	{
		public string[] options;
		public int index = 0;
		[MenuItem("Examples/Editor GUILayout Popup usage")]
		public static void Init(){
			EditorWindow window = GetWindow(typeof(RatioPopup));
			window.Show();
		}
		void OnGUI() {
			index = EditorGUILayout.Popup(index, options);
		}
		public RatioPopup (string[] _options)
		{
			options = _options;
			Init ();
		}
	}
}

