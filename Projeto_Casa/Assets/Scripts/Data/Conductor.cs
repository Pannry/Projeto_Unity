using System;
using UnityEngine;
using UnityEngine.UI;
namespace AssemblyCSharp
{
	public class Conductor
	{
		private string switchboard;
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

		public GameObject GetLabel(){
			return text;
		}

		public GameObject GetGameObject(){
			return gameObject;
		}

		public void SetSwitchBoard(string s){
			switchboard = s;
			text = MonoBehaviour.Instantiate (GameObject.Find ("Label")) as GameObject;
			text.GetComponent<Text> ().text = s;
		}

		public string GetSwitchBoard(){
			return switchboard;
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
			//string s  = GetSwitchBoard

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

		public void DrawLabel(GameObject edge){
			GameObject g = GameObject.Find ("Canvas_MainMenu");
			Canvas c = g.GetComponent<Canvas> ();
			text.transform.SetParent (c.transform, false);
			text.transform.position = Camera.main.WorldToScreenPoint (gameObject.transform.position);
			RotateIcon (edge,text);
		}

		private void RotateIcon(GameObject edge,GameObject text){

			Vector3 po1 = edge.GetComponent<LineRenderer> ().GetPosition (0);
			Vector3 po2 = edge.GetComponent<LineRenderer> ().GetPosition (1);
			double deltay = po2.z - po1.z;
			double deltax = po2.x - po1.x;
			double m = deltay / deltax;
			float angle = (float)(Math.Atan (m))* 57.2958F;
			text.transform.Rotate(new Vector3 (0, 0, angle));//Convertendod de radiano para grao.
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

