
using System;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace AssemblyCSharp
{
	public class InfoPopup:EditorWindow
	{
		public LinkedList<Edge> edges;
		[MenuItem("Window/My Window")]
		public static void Init(){
			EditorWindow window = GetWindow(typeof(InfoPopup));
			window.Show();
		}
		void OnGUI() {
		}
		void OnDestroy(){
			foreach (Edge e in edges)
				Destroy (e.edge.GetComponent<BoxCollider> ());
		}
		public InfoPopup (LinkedList<Edge> _edges)
		{
			edges = _edges;
			Init ();
		}
	}
}

