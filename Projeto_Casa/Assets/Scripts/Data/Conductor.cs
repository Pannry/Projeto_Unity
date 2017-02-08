using System;
using UnityEngine;
using UnityEngine.UI;
namespace AssemblyCSharp
{
	public class Conductor
	{
		private GameObject gameObject;
		private GameObject text;
		//Fio ou Cabo.
		private string conductor;
		//Fase, Neutro, Retorno ou Terra
		private string type;
		private float offset;
		private int mycircuit;
		//especial para arestas verticais.
		public int usedByHowMany;

		public GameObject GetGameObject(){
			return gameObject;
		}

		public void SetGameObject(GameObject g){
			gameObject = g;
		}


		public int circuit{
			get{ return mycircuit;}
		}

		public int GetCircuit(){
			return mycircuit;
		}

		public void SetCircuit(int i){
			mycircuit = i;
			string s  ="C " + i.ToString();
			text = MonoBehaviour.Instantiate (GameObject.Find ("Label")) as GameObject;
			text.GetComponent<Text> ().text = s;
		}

		public Conductor (string conductor, string type, float offset)
		{
			conductor = conductor;
			type = type;
			offset = offset;
		}

		public Conductor(){
		}

		public string Print(){
			string s = conductor + "   " + type;
			return s;
		}

		public void DrawLabel(){
			GameObject g = GameObject.Find ("Canvas_MainMenu");
			Canvas c = g.GetComponent<Canvas> ();
			text.transform.SetParent (c.transform, false);
			text.transform.position = Camera.main.WorldToScreenPoint (gameObject.transform.position);
			text.transform.position += text.transform.right*10;
		}

		public void SetType(string s){
			type = s;
		}

		public void SetConductor(string c){
			conductor = c;
		}

		public string GetConductor(){
			return conductor;
		}

		public string GetMyType(){
			return type;
		}
	}
}

