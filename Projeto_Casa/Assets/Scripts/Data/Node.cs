	using System;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	namespace AssemblyCSharp
	{
	public class Node : MonoBehaviour
	{
		private LinkedList<Edge> edges;

        private static Dictionary<string, int> quantidade = new Dictionary<string, int>();

        public static Dictionary<string, int> Quantidade {
            get {return quantidade;}
        }

        void FixedUpdate(){
			RemoveEdge (null);
		}

		public void CreateNode ()
		{
			edges = new LinkedList<Edge> ();
			string text = gameObject.GetComponent<Text> ().text;
			//this.myName = name;
            Debug.Log(gameObject.name);
			if (quantidade.ContainsKey(text)) {
				quantidade[text] += 1;
            }
            else {
				quantidade.Add(text, 1);
            }
		}

        public void Remove() {
			quantidade[gameObject.GetComponent<Text> ().text] -= 1;
			if (quantidade [gameObject.GetComponent<Text> ().text] == 0)
				quantidade.Remove (gameObject.GetComponent<Text> ().text);
        }
		public GameObject GetObject(){
			return gameObject;
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
				if (e[i] != null && !e [i].gameObject.Equals (edge))
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

