
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
		public ClickEvent myEvent;
		public LinkedList<AssemblyCSharp.Edge> edges;
		public GameObject myEdge;
		[MenuItem("Window/My Window")]
		public static void Init(){
			EditorWindow window = GetWindow(typeof(InfoPopup));
			window.titleContent.text = "Informações";
			window.Show();
		}
		void OnGUI() {
			foreach ( AssemblyCSharp.Edge e in edges) {
				if (e.edge.Equals (myEdge)) {
					LineRenderer lr = myEdge.GetComponent<LineRenderer> ();
					if (!e.isVertical) {
						float xratio = myEvent.GetRatios () [0];
						float zratio = myEvent.GetRatios () [1];
						Vector3 reworkedA = new Vector3 (lr.GetPosition (0).x * (1 / xratio),
							                    lr.GetPosition (0).y, lr.GetPosition (0).z * (1 / zratio));
						Vector3 reworkedB = new Vector3 (lr.GetPosition (1).x * (1 / xratio),
							                    lr.GetPosition (1).y, lr.GetPosition (1).z * (1 / zratio));
						float result = Vector3.Distance (reworkedA, reworkedB);
						EditorGUILayout.LabelField ("Comprimento: " + result.ToString ());
					} else {
						float result = Vector3.Distance (lr.GetPosition(0), lr.GetPosition(1));
						EditorGUILayout.LabelField ("Comprimento: " + result.ToString ());
					}
				}
			}
			
		}

		void OnDestroy(){
			foreach ( AssemblyCSharp.Edge e in edges)
				Destroy (e.edge.GetComponent<BoxCollider> ());
		}
		public InfoPopup (LinkedList< AssemblyCSharp.Edge> _edges, GameObject _myEdge, AssemblyCSharp.ClickEvent _myEvent)
		{
			myEvent = _myEvent;
			myEdge = _myEdge;
			edges = _edges;
			Init ();
		}
	}
}
#endif
