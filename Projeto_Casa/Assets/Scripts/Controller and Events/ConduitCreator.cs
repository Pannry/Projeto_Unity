using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class ConduitCreator:MonoBehaviour
	{
		private Ray ray;
		private RaycastHit hit;
		private GameObject prefab;
		private GameObject lastNode, lastObject;
		private Edge tempEdge;
		private float height;

		void Update(){
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				try{
					CreateConduitCeiling ();
				}
				catch(IndexOutOfRangeException e){
					Debug.Log("Problema na criacao no elemento eletroduto, redefina o elemento.");
				}

			}
		}

		public void DefinePrefab(){
			int option = GetComponent<Controller> ().GetOption ();
			prefab = GetComponent<Controller> ().prefab [option - 1];
		}

		public void CreateConduitCeiling(){
			// Pego as coordenadas do objeto em que o raio colide, então eu vou ter os vertices das
			// linhas centralizados no node.
			GameObject obj = hit.transform.gameObject;
			string tag = hit.transform.gameObject.tag;
			float x, z, currentHeight;
			x = obj.transform.position.x;
			currentHeight = obj.transform.position.y;
			z = obj.transform.position.z;
			float ceilingHeight = 2.8F;
			int option = GetComponent<Controller> ().GetOption ();
			//GetButton para drag, Se a tag do meu objeto for nao nula:
			if (Input.GetButton ("Fire1") && option == 3 && tag != Tags.SemTag ()) { 
				DefinePrefab();
				if (Input.GetButtonDown ("Fire1") && Node.isNode (tag)) {
					// Pegue a referência e a altura do primeiro objeto criado.
					height = currentHeight;
					lastNode = obj;
					lastObject = Instantiate (prefab, new Vector3 (x, ceilingHeight, z), Quaternion.identity) as GameObject;
					LineRenderer lr = lastObject.GetComponent<LineRenderer> ();
					lr.SetWidth (0.1F, 0.1F);
					// E digo que a linha sera formada por 2 vertices.
					lr.SetVertexCount (2);
					// O primeiro vertice eh a posicao onde esse click foi identificado.
					lr.SetPosition (0, new Vector3 (x, ceilingHeight, z));
					//tempEdge = new Edge (lastObject, obj, null);
					tempEdge = lastObject.AddComponent<Edge>();
					tempEdge.CreateEdge (obj, null);
				}
				// Arrasta o ultimo objeto criado.
				if (lastObject != null) {
					// Pego o renderizador de linha do ultimo objeto criado e mexo somente o ultimo vertice.
					// Isso da todo o efeito de drag.
					LineRenderer lr = lastObject.GetComponent<LineRenderer> ();
					lr.SetPosition (1, new Vector3 (hit.point.x, ceilingHeight, hit.point.z));
				}
			}
			if (Input.GetButtonUp ("Fire1") && lastObject != null && option == 3) {
				// Se a tag não for de nenhum nó, destrua...
				// Se a linha for solta em algum objeto que nao seja no, sera destruida.
				if (!Node.isNode (tag)) {
					GetComponent<Controller> ().DestroyThisErrorEdge (lastObject);
					Destroy (lastObject);
					lastNode = null;
					tempEdge = null;
				} else {
					tempEdge.CreateEdge(tempEdge.inv, obj);
					GetComponent<Controller> ().InsertOnEdges (tempEdge);
					lastNode.GetComponent<Node>().AddEdge(tempEdge,height);
					obj.GetComponent<Node>().AddEdge(tempEdge,currentHeight);
					LineRenderer lr = lastObject.GetComponent<LineRenderer> ();
					lr.SetPosition (1, new Vector3 (x, height, z));
					// Aqui, se necessario, sera criada a tubulacao vertical em relacao ao par de vertices
					if (ceilingHeight > currentHeight) {
						GameObject verticalLine = Instantiate (prefab, new Vector3 (hit.point.x, ceilingHeight, hit.point.z), Quaternion.identity) as GameObject;
						LineRenderer r = verticalLine.GetComponent<LineRenderer> ();
						r.SetWidth (0.1F, 0.1F);
						r.SetVertexCount (2);
						r.SetPosition (0, new Vector3 (x, ceilingHeight, z));
						r.SetPosition (1, new Vector3 (x, currentHeight, z));
						tempEdge = verticalLine.AddComponent<Edge>();
						tempEdge.CreateEdge (lastNode, obj);
						tempEdge.isVertical = true;
						tempEdge.SetVerticalID ("out");
						GetComponent<Controller> ().InsertOnEdges (tempEdge);
						lastNode.GetComponent<Node> ().AddEdge (tempEdge, currentHeight);
						obj.GetComponent<Node> ().AddEdge (tempEdge, currentHeight);
						lastObject.GetComponent<Edge> ().SetVEdges (tempEdge, null);

					} if (height < ceilingHeight) {
						GameObject verticalLine = Instantiate (prefab, new Vector3 (hit.point.x, height, hit.point.z), Quaternion.identity) as GameObject;
						LineRenderer r = verticalLine.GetComponent<LineRenderer> ();
						float lastX, lastZ;
						lastX = lastNode.transform.position.x;
						lastZ = lastNode.transform.position.z;
						lr.SetPosition (0, new Vector3 (lastX, ceilingHeight, lastZ));
						lr.SetPosition (1, new Vector3 (x, ceilingHeight, z));
						r.SetWidth (0.1F, 0.1F);
						r.SetVertexCount (2);
						r.SetPosition (0, new Vector3 (lastX, height, lastZ));
						r.SetPosition (1, new Vector3 (lastX, ceilingHeight, lastZ));
						// Adiciona-se os 3 vertices para mover perpendicularmente ao node.
						//tempEdge = new Edge (linhaVertical, lastNode, obj); 
						tempEdge = verticalLine.AddComponent<Edge>();
						tempEdge.CreateEdge (lastNode, obj);
						tempEdge.isVertical = true;
						tempEdge.SetVerticalID ("in");
						GetComponent<Controller> ().InsertOnEdges (tempEdge);
						//sempre a altura mais baixa eh salva
						lastNode.GetComponent<Node> ().AddEdge (tempEdge, height);
						obj.GetComponent<Node> ().AddEdge (tempEdge, height);
						lastObject.GetComponent<Edge> ().SetVEdges (lastObject.GetComponent<Edge> ().GetVEdges()[0],tempEdge);
					}
					lastNode = null;
					tempEdge = null;
					lastObject = null;
				}
			}
		}
	}
}

