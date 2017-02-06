using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AssemblyCSharp
{
	/*
	 * Um problema ainda pendente eh quando for criada uma aresta vertical abaixo de um no, se ja existir uma acima.
	* Classe que representa uma aresta.
	* Caso a aresta seja vertical, o primeiro vertice eh o referente a sua posicao.
	* Isto pq uma aresta vertical sempre deve estar alinhada com um dos vertices.
	* O outro vertice, so indica que ela faz parte de uma conexao.
	* 
	* m = tgα = Δy / Δx 
	*/
	public class Edge : MonoBehaviour
	{

		public GameObject inv,outv;
		public float height;
		public bool isVertical = false;
		public float radius;
		public LinkedList<Conductor> content;
		private Edge[] verticalEdges;
		public GameObject variavelTeste;
		private int multiplier; // multiplicador de tamanho.



		void Start(){
			verticalEdges = new Edge[2];
			content = new LinkedList<Conductor> ();
			multiplier = 1;
		}

		public void CreateEdge (GameObject v1, GameObject v2)
		{
			inv = v1;
			outv = v2;	
		}

		//metodo para toda vida q moverem o eletroduto, os icones da fiacao serao atualizados.
		public void UpdateIcons(){
		}

		public bool InsertContent(Conductor c){
			if (content.Count >= 9*multiplier) {
				Debug.Log ("Erro, maximo de fios tolerados alcancado.");
				//Deve-se perguntar se o usuario quer duplicar o eletroduto.
				//Uma janela deve retornar um booleano aqui.
				//Crie outra classe.
			}
			LinkedList<int> totalCircuits = new LinkedList<int>();
			bool rotated = false;
			bool add = false;
			// Apagar proxima linha quando na classe popupinfo for adicionada o circuito do condutor.
			c.SetCircuit(new System.Random().Next(1,4));
			totalCircuits.AddLast (c.circuit);
			Vector3 po1 = gameObject.GetComponent<LineRenderer> ().GetPosition (0);
			Vector3 po2 = gameObject.GetComponent<LineRenderer> ().GetPosition (1);
			GameObject g = Instantiate (GameObject.Find(c.GetMyType().ToLower().Trim())) as GameObject;
			c.SetGameObject (g);
			// Se nao havia fiacao, comeca adicionando no meio.
			if (content.Count == 0) {
				//TODO Vou ter que definir agrupamentos por circuitos(Maximo de 3 por eletroduto).
				po1 = (po1 + po2) / 2;
				g.transform.position = po1;
				po1 = gameObject.GetComponent<LineRenderer> ().GetPosition (0);
				po2 = gameObject.GetComponent<LineRenderer> ().GetPosition (1);
				double deltay = po2.z - po1.z;
				Debug.Log (deltay);
				double deltax = po2.x - po1.x;
				Debug.Log (deltax);
				double m = deltay / deltax;
				float angle = (float)Math.Atan (m);
				g.transform.Rotate (new Vector3 (0, 0, angle * 57.2958F));//Convertendod de radiano para grao.
				add = true;
			} else {
				foreach (Conductor x in content) {
					if (x.GetMyType () == c.GetMyType () && x.circuit == c.circuit) {
						Debug.Log (x.GetMyType ());
						Debug.Log (c.GetMyType ());
						Debug.Log (x.circuit);
						Debug.Log ( c.circuit);
						Debug.Log ("Erro! Jah existe um fio/cabo de mesmo tipo pertencente a este circuito.");
						Destroy (g);
						add = false;
						return add;
					} else if (x.circuit == c.circuit) {
						// Se esse condutor pertence ha um mesmo circuito que esta aqui, entao:
						g.transform.position = x.GetGameObject ().transform.position;
						g.transform.position += x.GetGameObject ().transform.right * .1F;
						add = true;
						if(!rotated){
							po1 = gameObject.GetComponent<LineRenderer> ().GetPosition (0);
							po2 = gameObject.GetComponent<LineRenderer> ().GetPosition (1);
							double deltay = po2.z - po1.z;
							Debug.Log (deltay);
							double deltax = po2.x - po1.x;
							Debug.Log (deltax);
							double m = deltay / deltax;
							float angle = (float)Math.Atan (m);
							g.transform.Rotate (new Vector3 (0, 0, angle * 57.2958F));//Convertendod de radiano para grao.
							rotated = true;
						}
					}
					//Ve quantos circuitos diferentes tem no eletroduto.
					if(x.circuit != c.circuit){
						if (!totalCircuits.Contains (x.circuit)) {
							totalCircuits.AddLast (x.circuit);
						}
					}
				}
				//Debug.Log ("New insertion ::");
				//foreach (Conductor con in content) {
				//	Debug.Log (con.Print ());
				//}
				// Se houver menos de 3[a partir de 1] circuitos diferentes, e nao foi desenhado o simbolo ainda.
				if (totalCircuits.Count < 4 && !add) {
					po1 = (po1 + po2) / 2;
					if (totalCircuits.Count == 3) {
						// Agrupamento no final.
						po1 = (po1 + po2) / 2;
					}
					if (totalCircuits.Count == 2) {
						// Agrupamento no comeco
						po2 = gameObject.GetComponent<LineRenderer> ().GetPosition (0);
						po1 = (po1 + po2) / 2;
					}
					g.transform.position = po1;
					po1 = gameObject.GetComponent<LineRenderer> ().GetPosition (0);
					po2 = gameObject.GetComponent<LineRenderer> ().GetPosition (1);
					double deltay = po2.z - po1.z;
					Debug.Log (deltay);
					double deltax = po2.x - po1.x;
					Debug.Log (deltax);
					double m = deltay / deltax;
					float angle = (float)Math.Atan (m);
					g.transform.Rotate (new Vector3(0,0,angle*57.2958F));//Convertendod de radiano para grao.
					add = true;
				}
			}
			// add soh eh true quando o simbolo foi desenhado.
			if(add)
				content.AddLast (c);
			return add;
		}


		public void RemoveContent(int i){
			Conductor[] array = new Conductor[content.Count];
			content.CopyTo (array, 0);
			content.Remove (array [i]);
		}

		public void SetVEdges(Edge e1, Edge e2){
			verticalEdges [0] = e1;
			verticalEdges [1] = e2;
		}

		public Edge[] GetVEdges(){
			return verticalEdges;
		}
			
	}
}


