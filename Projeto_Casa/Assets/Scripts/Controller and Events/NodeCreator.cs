using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class NodeCreator : MonoBehaviour
	{
		
		private Ray ray;
		private RaycastHit hit;
		private GameObject prefab;

		void Update(){
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				float height = GetComponent<Controller>().GetHeight();
				if(GetComponent<Controller> ().GetOption () != 2)
				try{
					CreateNode (height);
				}
				catch(IndexOutOfRangeException e){
					Debug.Log("Problema na criacao no elemento node, reselecione o elemento.");
				}
			}
		}

		public void DefinePrefab(){
			int option = GetComponent<Controller> ().GetOption ();
			prefab = GetComponent<Controller> ().prefab [option - 1];
		}

		// Não está havendo erro, mas se acontecer 1, pode ser pq não estou verificando a opção.
		public void CreateNode(float height){
			string tag = hit.transform.gameObject.tag;
			int layer = hit.transform.gameObject.layer;
			// Se o raio estiver colidindo com a planta, cria o objeto a uma certa altura da planta.
			if(Input.GetButtonDown("Fire1")  && tag == Tags.Planta()){
				DefinePrefab ();
				GameObject obj= Instantiate(prefab,new Vector3(hit.point.x,height,hit.point.z), Quaternion.identity) as GameObject;
				obj.transform.Rotate(new Vector3(90F,0F,0F));
				Node n = obj.AddComponent<Node> ();
				n.CreateNode (obj.tag, obj.name);
				GetComponent<Controller> ().InsertOnNodes (n);
			}
		}
	}
}

