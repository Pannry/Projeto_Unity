	using System;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	namespace AssemblyCSharp
	{
	public class Node : MonoBehaviour
	{
		private LinkedList<Edge> edges;
		private string label;
		private string name;

		void Update(){
			RemoveEdge (null);
		}

		public void CreateNode (string label, string name)
		{
			this.label = label;
			edges = new LinkedList<Edge> ();
			this.name = name;
		}
		public GameObject GetObject(){
			return gameObject;
		}
		public string GetLabel(){
			return label;
		}
		public bool Compare(GameObject other){
			return other.Equals (gameObject);
		}
		public static bool isNode(string tag){
			return (tag == Tags.EmbutidaLaje() || tag == Tags.NoBaixo());
		}

		public static void DestroyEdgeInNodes(LinkedList<Node> nodes,GameObject edge){
			foreach (Node n in nodes) {
				// Cria um dicionario.
				n.RemoveEdge(edge);
			}
		}

		public void RemoveEdge(GameObject edge){
			Edge[] e = new Edge[edges.Count];
			edges.CopyTo (e, 0);
			edges = new LinkedList<Edge> ();
			for (int i = 0; i < e.Length; i++) {
				if (!e [i].gameObject.Equals (edge))
					edges.AddLast (e [i]);
			}

		}

		public LinkedList<Edge> GetEdges(){
			return edges;
		}

		public void AddEdge(Edge e, float h){
			edges.AddLast (e);
			edges.Last.Value.height = h;
		}
	}
	}

