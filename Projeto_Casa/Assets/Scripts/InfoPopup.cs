
#if UNITY_EDITOR
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
		public GameObject myEdge;
		[MenuItem("Window/My Window")]
		public static void Init(){
			EditorWindow window = GetWindow(typeof(InfoPopup));
			window.titleContent.text = "Informações";
			window.Show();
		}
		void OnGUI() {
			foreach (Edge e in edges) {
				if (e.edge.Equals (myEdge)) {
					EditorGUILayout.LabelField ("Deu certo");
				}
			}
			
		}

		void OnDestroy(){
			foreach (Edge e in edges)
				Destroy (e.edge.GetComponent<BoxCollider> ());
		}
		public InfoPopup (LinkedList<Edge> _edges, GameObject _myEdge)
		{
			myEdge = _myEdge;
			edges = _edges;
			Init ();
		}
	}
}
#endif
