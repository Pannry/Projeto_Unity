using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class MoveHanddle :MonoBehaviour
	{
		private Ray ray;
		private RaycastHit hit;
		private GameObject lastObject;

		void Update(){
			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				Move ();				
			}
		}

		private void MoveEdges(){
			foreach (Edge e in lastObject.GetComponent<Node>().GetEdges()) {
				if (e.gameObject != null) {
					LineRenderer lr = e.gameObject.GetComponent<LineRenderer> ();
					//Se Reta horizontal...
					if (!e.isVertical) {
						//e.UpdateIcons ();
						if (lastObject.Equals (e.inv)) {
							lr.SetPosition (0, new Vector3 (lastObject.transform.position.x, lr.GetPosition (0).y,
								lastObject.transform.position.z));
						} else if (lastObject.Equals (e.outv)) {
							lr.SetPosition (1, new Vector3 (lastObject.transform.position.x, lr.GetPosition (1).y,
								lastObject.transform.position.z));
						}
					} else {
						//So move aresta vertical se estiver fazendo o mov apartir do node inferior.
						if (lastObject.transform.position.y == e.height) {
							// Todas as verticais são filhas de um mesmo par de vertices. Só da pra identificar
							// sob qual par cada uma deve ficar através do ID.
							if (e.inv.Equals(lastObject)) {
								lr.SetPosition (0, new Vector3 (lastObject.transform.position.x, lr.GetPosition (0).y,
									lastObject.transform.position.z));
								lr.SetPosition (1, new Vector3 (lastObject.transform.position.x, lr.GetPosition (1).y,
									lastObject.transform.position.z));
							}
						}
					}
				}
			}
		}

		public void Move(){
			int option = GetComponent<Controller> ().GetOption ();
			// Pega a tag do objeto que o raio colidir.
			string tag = hit.transform.gameObject.tag;
			// Se esse objeto for um node eu vou move-lo.
			if (Input.GetButton ("Fire1") && option == 98 && Node.isNode(tag)) {
				// A partir do momento em que se segura o botao do mause, e detectado um click, e eu pego aquele node
				if (Input.GetButtonDown ("Fire1") && Node.isNode(tag)) {
					lastObject = hit.transform.gameObject;
				}
				// Fico mudando a posicao do node.
				if (lastObject != null && Node.isNode (lastObject.tag)) {
					MoveEdges ();
					lastObject.transform.position = new Vector3 (hit.point.x,lastObject.transform.position.y,hit.point.z);
				}
				// Quando soltar o botao o node vai parar de se mover, e eu tiro a referencia do lastobject.

			}
			if (Input.GetButtonUp ("Fire1") && lastObject!=null) {
				foreach (Edge e in lastObject.GetComponent<Node>().GetEdges()) {
					e.UpdateIcons ();
				}
				lastObject = null;
			}
		}

	}
}

