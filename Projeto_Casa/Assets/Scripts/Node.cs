	using System;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	namespace AssemblyCSharp
	{
	public class Node : MonoBehaviour
	{
		private GameObject obj;
		private LinkedList<GameObject> edges;
		private string label;
		private string name;
		public Node (GameObject obj, string label, string name)
		{
			this.obj = obj;
			this.label = label;
			edges = new LinkedList<GameObject> ();
			this.name = name;
		}
		public GameObject GetObject(){
			return obj;
		}
		public string GetLabel(){
			return label;
		}
		public bool Compare(GameObject other){
			return other.Equals (obj);
		}
		public static bool isNode(string tag){
			return (tag == Tags.EmbutidaLaje() || tag == Tags.QuadroEletrico());
		}
		public static void SearchNodeAndAddEdge(LinkedList<Node> nodes, GameObject node ,GameObject edge){
			foreach (Node n in nodes) {
				if (n.Compare (node)) {
					n.AddEdge (edge);
					break;
				}
			}
		}
		public static void SearchNodeAndDestroyEdge(LinkedList<Node> nodes,GameObject edge){
			foreach (Node n in nodes) {
				// Cria um dicionario.
				if (n.GetEdges().Contains(edge)) {
					n.RemoveEdge (edge);
				}
			}
		}

		public static void DestroyNode(LinkedList<Node> nodes, GameObject node){
			Node[] n = new Node[nodes.Count];
			nodes.CopyTo(n,0);
			for (int i = 0; i < nodes.Count; i++) {
				if (n[i].Compare (node)) {
					Debug.Log ("Node apagado =>" + n [i].name);
					foreach (GameObject a in n[i].GetEdges()) {
						Destroy(a);
					}
					nodes = new LinkedList<Node> ();
					for(int j = 0; j < n.Length; j++) {
						if (j != i)
							nodes.AddLast (n [j]);
					}
				}
			}
		}

		public void RemoveEdge(GameObject edge){
			edges.Remove (edge);
		}

		public LinkedList<GameObject> GetEdges(){
			return edges;
		}

		public void AddEdge(GameObject edge){
			edges.AddLast (edge);
		}
	}
	}

