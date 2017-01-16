using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AssemblyCSharp
{
	public class Edge : MonoBehaviour
	{
		public GameObject inv,outv;
		public float height;
		public bool isVertical = false;
		public float radius;
		public LinkedList<Conductor> content;

		void Start(){
			content = new LinkedList<Conductor> ();
		}

		public void CreateEdge (GameObject v1, GameObject v2)
		{
			inv = v1;
			outv = v2;	
		}

		public void InsertContent(Conductor c){
			content.AddLast (c);
			Debug.Log ("New insertion ::");
			foreach(Conductor con in content){
				Debug.Log(con.Print());
			}
		}

		public void RemoveContent(int i){
			Conductor[] array = new Conductor[content.Count];
			content.CopyTo (array, 0);
			content.Remove (array [i]);
		}
	}
}


