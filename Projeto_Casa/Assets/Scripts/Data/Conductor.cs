using System;
using UnityEngine;
namespace AssemblyCSharp
{
	public class Conductor
	{
		private GameObject gameObject;
		//Fio ou Cabo.
		private string conductor;
		//Fase, Neutro, Retorno ou Terra
		private string type;
		private float offset;
		private int mycircuit;

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

