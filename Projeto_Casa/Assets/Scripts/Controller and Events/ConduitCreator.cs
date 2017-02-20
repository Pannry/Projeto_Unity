using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	/// <summary>
	/// Script usado únicamente para criar eletrodutos.
	/// </summary>
	public class ConduitCreator:MonoBehaviour
	{
		private Ray ray;
		private RaycastHit hit;
		private GameObject prefab;
		private GameObject lastNode, lastObject;
		private Edge tempEdge;
		private float height;

		/// <summary>
		/// Constantemente fica fazendo atualizações, para saber se o controller está pedindo para criar
		/// um eletroduto. Ele chega isso atravez da opção usada.
		/// </summary>
		void Update(){
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				try{
					int option = GetComponent<Controller> ().GetOption ();
					if(option == 3){
						CreateConduit (false,2.8F);
					}
					else if(option == 15){
						CreateConduit (true,0.01F);
					}
					else if(option == 16){
						CreateConduit (false,-1);
					}
				}
				catch(IndexOutOfRangeException e){
					Debug.Log("Problema na criacao no elemento eletroduto, redefina o elemento.");
				}

			}
		}

		/// <summary>
		/// Defines the prefab.
		/// </summary>
		public void DefinePrefab(){
			int option = GetComponent<Controller> ().GetOption ();
			prefab = GetComponent<Controller> ().prefab [option - 1];
		}

		/// <summary>
		/// Creates the conduit.
		/// </summary>
		/// <param name="isDown">If set to <c>true</c> is down. O eletroduto será criado em direção ao chão.
		/// Do contrario em direção a laje.</param>
		/// <param name="conduitHeight">A altura que o eletroduto deve ficar.</param>
		public void CreateConduit(bool isDown, float conduitHeight){
			// Pego as coordenadas do objeto em que o raio colide, então eu vou ter os vertices das
			// linhas centralizados no node.
			GameObject node = hit.transform.gameObject;
			string tag = hit.transform.gameObject.tag;
			float x, z, currentHeight;
			x = node.transform.position.x;
			currentHeight = node.transform.position.y;
			z = node.transform.position.z;
			if (conduitHeight == -1)
				conduitHeight = node.transform.position.y;
			int option = GetComponent<Controller> ().GetOption ();
			//GetButton para drag, Se a tag do meu objeto for nao nula:
			if (Input.GetButton ("Fire1") && (option == 3 || option == 15 ||option == 16) && tag != Tags.SemTag ()) { 
				DefinePrefab();
				if (Input.GetButtonDown ("Fire1") && Node.isNode (tag)) {
					// Pegue a referência e a altura do primeiro objeto criado.
					height = currentHeight;
					lastNode = node;
					lastObject = Instantiate (prefab, new Vector3 (x, conduitHeight, z), Quaternion.identity) as GameObject;
					LineRenderer lr = lastObject.GetComponent<LineRenderer> ();
					lr.SetWidth (0.05F, 0.05F);
					// E digo que a linha sera formada por 2 vertices.
					lr.SetVertexCount (2);
					// O primeiro vertice eh a posicao onde esse click foi identificado.
					lr.SetPosition (0, new Vector3 (x, conduitHeight, z));
					//tempEdge = new Edge (lastObject, obj, null);
					tempEdge = lastObject.AddComponent<Edge>();
					tempEdge.CreateEdge (node, null);
				}
				// Arrasta o ultimo objeto criado.
				if (lastObject != null) {
					// Pego o renderizador de linha do ultimo objeto criado e mexo somente o ultimo vertice.
					// Isso da todo o efeito de drag.
					LineRenderer lr = lastObject.GetComponent<LineRenderer> ();
					lr.SetPosition (1, new Vector3 (hit.point.x, conduitHeight, hit.point.z));
				}
			}
			if (Input.GetButtonUp ("Fire1") && lastObject != null && (option == 3 || option == 15||option == 16)) {
				// Se a tag não for de nenhum nó, destrua...
				// Se a linha for solta em algum objeto que nao seja no, sera destruida.
				if (!Node.isNode (tag) || lastNode.Equals(node)) {
					GetComponent<Controller> ().DestroyThisErrorEdge (lastObject);
					Destroy (lastObject);
					lastNode = null;
					tempEdge = null;
				} else {
					tempEdge.CreateEdge(tempEdge.firstVertex, node);
					GetComponent<Controller> ().InsertOnEdges (tempEdge);
					lastNode.GetComponent<Node>().AddEdge(tempEdge,height);
					node.GetComponent<Node>().AddEdge(tempEdge,currentHeight);
					LineRenderer lr = lastObject.GetComponent<LineRenderer> ();
					lr.SetPosition (1, new Vector3 (x, height, z));
					// Aqui, se necessario, sera criada a tubulacao vertical em relacao ao par de vertices
					//if (/*option == 3 || option == 15*/true) {
						tempEdge = GetComponent<Controller> ().HasVerticalEdge (node, isDown);
						// Cria aresta com in e outter verteces.
						if (tempEdge == null) {
							GameObject verticalLine = Instantiate (prefab, new Vector3 (hit.point.x, conduitHeight, hit.point.z), Quaternion.identity) as GameObject;
							LineRenderer r = verticalLine.GetComponent<LineRenderer> ();
							r.SetWidth (0.05F, 0.05F);
							r.SetVertexCount (2);
							r.SetPosition (0, new Vector3 (x, conduitHeight, z));
							r.SetPosition (1, new Vector3 (x, currentHeight, z));
							tempEdge = verticalLine.AddComponent<Edge> ();
							//Na aresta vertical, o primeiro vertice é referente à posição da aresta.
							tempEdge.CreateEdge (node, lastNode);
							tempEdge.isVertical = true;
							if (option == 15)
								tempEdge.isDown = true;
							node.GetComponent<Node> ().AddEdge (tempEdge, currentHeight);
							GetComponent<Controller> ().InsertOnEdges (tempEdge);
						}
						lastObject.GetComponent<Edge> ().SetVEdges (tempEdge, null);
						lastNode.GetComponent<Node> ().AddEdge (tempEdge, currentHeight);
						//Cria uma aresta vertical para o ultimo nó.
						tempEdge = GetComponent<Controller> ().HasVerticalEdge (lastNode, isDown);
						float lastX, lastZ;
						lastX = lastNode.transform.position.x;
						lastZ = lastNode.transform.position.z;
						lr.SetPosition (0, new Vector3 (lastX, conduitHeight, lastZ));
						lr.SetPosition (1, new Vector3 (x, conduitHeight, z));
						if (tempEdge == null) {
							GameObject verticalLine = Instantiate (prefab, new Vector3 (hit.point.x, height, hit.point.z), Quaternion.identity) as GameObject;
							LineRenderer r = verticalLine.GetComponent<LineRenderer> ();
							r.SetWidth (0.05F, 0.05F);
							r.SetVertexCount (2);
							r.SetPosition (0, new Vector3 (lastX, height, lastZ));
							r.SetPosition (1, new Vector3 (lastX, conduitHeight, lastZ));
							tempEdge = verticalLine.AddComponent<Edge> ();
							//Na aresta vertical, o primeiro vertice é referente à posição da aresta.
							tempEdge.CreateEdge (lastNode, node);
							tempEdge.isVertical = true;
							if (option == 15)
								tempEdge.isDown = true;
							GetComponent<Controller> ().InsertOnEdges (tempEdge);
							lastNode.GetComponent<Node> ().AddEdge (tempEdge, height);
						}
						lastObject.GetComponent<Edge> ().SetVEdges (lastObject.GetComponent<Edge> ().GetVEdges () [0], tempEdge);
						node.GetComponent<Node> ().AddEdge (tempEdge, height);
					//}
					lastNode = null;
					tempEdge = null;
					lastObject = null;
				}
			}
		}



		//Retorna verdade se já existe uma aresta vertical associada aquele nó. 

	}
}

