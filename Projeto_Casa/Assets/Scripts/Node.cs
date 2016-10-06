	using System;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	namespace AssemblyCSharp
	{
	public class Node : MonoBehaviour
	{
		private GameObject objeto;
		private LinkedList<int> verticesDasArestas;
		private LinkedList<GameObject> arestas;
		private string rotulo;
		public Node (GameObject objeto, string rotulo)
		{
			this.objeto = objeto;
			this.rotulo = rotulo;
			arestas = new LinkedList<GameObject> ();
		}
		public GameObject GetObject(){
			return objeto;
		}
		public string GetLabel(){
			return rotulo;
		}
		public bool Compare(GameObject outro){
			return outro.Equals (objeto);
		}
		public static bool isNode(string tag){
			return (tag == Tags.EmbutidaLaje() || tag == Tags.QuadroEletrico());
		}
		public static void SearchNodeAndAddEdge(LinkedList<Node> nodes, GameObject node ,GameObject aresta){
			foreach (Node n in nodes) {
				if (n.Compare (node)) {
					n.AddEdge (aresta);
					break;
				}
			}
		}
		public static void SearchNodeAndDestroyEdge(LinkedList<Node> nodes,GameObject aresta){
			foreach (Node n in nodes) {
				// Cria um dicionario.
				if (n.GetEdges().Contains(aresta)) {
					n.RemoveEdge (aresta);
				}
			}
		}

		public static LinkedList<GameObject> DestroyNode(LinkedList<Node> nodes, GameObject node){
			LinkedList<GameObject> toDestroy = new LinkedList<GameObject>();
			Node[] n = new Node[nodes.Count];
			nodes.CopyTo(n,0);
			for (int i = 0; i < nodes.Count; i++) {
				if (n[i].Compare (node)) {
					Debug.Log ("Encontrado");
					nodes.Remove (n[i]);
					foreach (GameObject a in n[i].GetEdges()) {
						Destroy(a);
					}
				}
			}
			return toDestroy;
		}

		public void RemoveEdge(GameObject aresta){
			arestas.Remove (aresta);
		}

		public LinkedList<GameObject> GetEdges(){
			return arestas;
		}

		public void AddEdge(GameObject aresta){
			arestas.AddLast (aresta);
		}
	}
	}

