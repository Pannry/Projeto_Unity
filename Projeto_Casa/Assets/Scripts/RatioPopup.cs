using System;
using UnityEditor;
using UnityEngine;
using System.Collections;


namespace AssemblyCSharp
{
	public class RatioPopup: EditorWindow
	{
		public bool save = false;
		public GameObject mySquare;
		public ClickEvent myEvents;
		public string[] options;
		public float myXRatio, myYRatio;
		[MenuItem("Window/My Window")]
		public static void Init(){
			EditorWindow window = GetWindow(typeof(RatioPopup));
			window.Show();
		}
		void OnGUI() {
			GUILayout.Label ("Configurando a Razão", EditorStyles.boldLabel);
			EditorGUILayout.LabelField ("Razão MundoX/RealX =" + myEvents.GetRatios () [0].ToString ());
			EditorGUILayout.LabelField ("Razão MundoY/RealY =" + myEvents.GetRatios () [1].ToString ());
			myXRatio = EditorGUILayout.Slider ("Largura", myXRatio, -100, 100);
			myYRatio = EditorGUILayout.Slider ("Comprimento", myYRatio, -100, 100);
		}
		void OnDestroy(){
			
			LineRenderer lr = mySquare.GetComponent<LineRenderer> ();
			float worldXRatio = lr.GetPosition (1).x - lr.GetPosition (0).x;
			float worldYRatio = lr.GetPosition (2).z - lr.GetPosition (1).z;
			if (worldXRatio < 0)
				worldXRatio *= -1;
			if (worldYRatio < 0)
				worldYRatio *= -1;
			myXRatio = worldXRatio / myXRatio;
			myYRatio = worldYRatio / myYRatio;
			Debug.Log ("Calculated ratio " + myXRatio);
			myEvents.SetRatios (myXRatio, myYRatio);
			Destroy (mySquare);
		}
		public RatioPopup (string[] _options,ClickEvent events, GameObject square)
		{
			mySquare = square;
			myEvents = events;
			options = _options;
			Init ();
		}
	
	}
}

