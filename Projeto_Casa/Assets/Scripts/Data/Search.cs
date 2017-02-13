using System;
using System.Collections;
using UnityEngine;

namespace AssemblyCSharp
{
	public static class Search
	{
		//Busca em largura modificada, vai jogar no "result" todos os componentes que sejam quadros eletricos.
		public static void BreadthFirstSearch(ArrayList explored, ArrayList frontier, ArrayList result){
			if (frontier.Count > 0) {
				Node currentNode = (Node)(frontier [0]);
				frontier.Remove (currentNode);
				explored.Add (currentNode);
				if (currentNode.GetComponent<InfoQadroEletrico> () != null) {
					result.Add (currentNode);
				}
				foreach (Edge e in currentNode.GetEdges()) {
					Node myChild = null;
					if (!e.inv.Equals (currentNode.gameObject))
						myChild = e.inv.GetComponent<Node> ();
					else if (!e.outv.Equals (currentNode.gameObject))
						myChild = e.outv.GetComponent<Node> ();
					if (!explored.Contains (myChild)) {
						if (!frontier.Contains (myChild)) {
							frontier.Add (myChild);
						}
					}
				}
				BreadthFirstSearch (explored, frontier, result);
			}
		}
	}
}

