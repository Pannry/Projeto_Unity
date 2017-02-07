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

		public void DoubleMultiplier(){
			multiplier *= 2;
		}

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

		//Cada aresta deve atualizar as informacoes de sua aresta vertical, pois quando o usuario remover uma fiacao de uma aresta,
		// a da aresta vertical tambem vai embora, dai se outro eletroduto usava aquele circuito, vai ficar zuado.
		//metodo para toda vida q moverem o eletroduto, os icones da fiacao serao atualizados.
		public void UpdateIcons(){
			//TODO hora do update.
			Vector3 po1,po2;
			po1 = GetComponent<LineRenderer> ().GetPosition (0);
			po2 = GetComponent<LineRenderer> ().GetPosition (1);
			Conductor[] group = new Conductor[3];
			group [0] = null;
			group [1] = null;
			group [2] = null;
			int counter = 0;
			if(!isVertical)
				foreach(Conductor c in content){
					
					GameObject g = c.GetGameObject ();
					g.transform.rotation = Quaternion.identity;
					po1 = GetComponent<LineRenderer> ().GetPosition (0);
					po2 = GetComponent<LineRenderer> ().GetPosition (1);
					g.transform.Rotate (90, 0, 0);
					if (group[0]!= null && c.circuit != group [0].circuit) {
						counter++;
					}
					if (counter == 0) {
						Debug.Log ("c1");
						group [0] = c;
						po1 = (po1 + po2) / 2;
						g.transform.position = po1;
						DrawIcon (po1, po2, g);
					}
					if (counter == 1) {
						Debug.Log ("c2");
						group [1] = c;
						po1 = (po1 + po2) / 2;
						po2 = gameObject.GetComponent<LineRenderer> ().GetPosition (0);
						po1 = (po1 + po2) / 2;
						g.transform.position = po1;
						DrawIcon (po1, po2, g);
					}
					if (counter == 2) {
						Debug.Log ("c3");
						group [2] = c;
						po1 = (po1 + po2) / 2;
						po1 = (po1 + po2) / 2;
						g.transform.position = po1;
						DrawIcon (po1, po2, g);
					}
					for(int i = 0; i < group.Length; i++)
						if (group [i] != null && c.circuit == group [i].circuit && !group[i].Equals(c)) {
							g.transform.position = group [i].GetGameObject ().transform.position;
							g.transform.position += group [i].GetGameObject ().transform.right * .1F;
							DrawIcon (po1, po2, g);
							group [i] = c;
						}
				}
		}

		private void DrawIcon(Vector3 po1, Vector3 po2, GameObject g){
			
			po1 = gameObject.GetComponent<LineRenderer> ().GetPosition (0);
			po2 = gameObject.GetComponent<LineRenderer> ().GetPosition (1);
			double deltay = po2.z - po1.z;
			double deltax = po2.x - po1.x;
			double m = deltay / deltax;
			float angle = (float)(Math.Atan (m))* 57.2958F;
			g.transform.Rotate(new Vector3 (0, 0, angle));//Convertendod de radiano para grao.
		}

		private void RedrawIcon(Vector3 po1, Vector3 po2, GameObject g){
			
			po1 = gameObject.GetComponent<LineRenderer> ().GetPosition (0);
			po2 = gameObject.GetComponent<LineRenderer> ().GetPosition (1);
			double deltay = po2.z - po1.z;
			double deltax = po2.x - po1.x;
			double m = deltay / deltax;
			float angle = (float)(Math.Atan (m))* 57.2958F;
			g.transform.Rotate (new Vector3 (0, 0, -angle));//Convertendod de radiano para grao.
		}

		public bool InsertContent(Conductor c){
			if (content.Count >= 9*multiplier) {
				Debug.Log ("Erro, maximo de fios tolerados alcancado.");
				return false;
				//Deve-se perguntar se o usuario quer duplicar o eletroduto.
				//Uma janela deve retornar um booleano aqui.
				//Crie outra classe.
			}
			LinkedList<int> totalCircuits = new LinkedList<int>();
			bool rotated = false;
			bool add = false;
			// Apagar proxima linha quando na classe popupinfo for adicionada o circuito do condutor.
			Vector3 po1 = new Vector3(), po2 = new Vector3();
			GameObject g = null;
			if (!isVertical) {
				c.SetCircuit(new System.Random().Next(1,6));
				po1 = gameObject.GetComponent<LineRenderer> ().GetPosition (0);
				po2 = gameObject.GetComponent<LineRenderer> ().GetPosition (1);
				g = Instantiate (GameObject.Find (c.GetMyType ().ToLower ().Trim ())) as GameObject;
			}
			totalCircuits.AddLast (c.circuit);
			/*if(isVertical) Debug.Log ("Circuito escolhido:" + c.circuit + " Tipo de fio" + c.GetMyType() +
				" used by: "+ c.usedByHowMany +" c::"+ content.Count);*/
			if(g!= null) c.SetGameObject (g);
			// Se nao havia fiacao, comeca adicionando no meio.
			if (content.Count == 0) {
				if (!isVertical) {
					po1 = (po1 + po2) / 2;
					g.transform.position = po1;
					DrawIcon (po1, po2, g);
				}
				add = true;
			} else {
				foreach (Conductor x in content) {
					if (x.GetMyType () == c.GetMyType () && x.circuit == c.circuit) {
						if (!isVertical) {
							Debug.Log ("Erro! Jah existe um fio/cabo de mesmo tipo pertencente a este circuito.");
							if (g != null)
								Destroy (g);
							return false;
						} else {
							//Debug.Log ("ok!");
							x.usedByHowMany++;
							Debug.Log ("Circuito escolhido:" + c.circuit + " Tipo de fio" + c.GetMyType() +
								" used by: "+ x.usedByHowMany);
							return true;
						}
					} else if (x.circuit == c.circuit) {
						// Se esse condutor pertence ha um mesmo circuito que esta aqui, entao:
						add = true;
						if (!isVertical) {
							g.transform.position = x.GetGameObject ().transform.position;
							g.transform.position += x.GetGameObject ().transform.right * .1F;
							if (!rotated) {
								DrawIcon (po1, po2, g);
								rotated = true;
							}
						}
					}
					//Ve quantos circuitos diferentes tem no eletroduto.
					else{
						if (!totalCircuits.Contains (x.circuit)) {
							totalCircuits.AddLast (x.circuit);
						}
					}
				}
				// Se houver menos de 3[a partir de 1] circuitos diferentes, e nao foi desenhado o simbolo ainda.
				if (totalCircuits.Count < 4 && !add && !isVertical) {
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
					DrawIcon (po1, po2, g);
					add = true;
				} else if (totalCircuits.Count >= 4 && !isVertical) {
					Debug.Log ("Erro! Já existem 3 agrupamentos de circuitos nesse eletroduto, delete um agrupamento para inserir" +
					" um fio de outro circuito.");
				}

			}
			// add soh eh true quando o simbolo foi desenhado.
			if (!add && isVertical && content.Count < 9) 
				add = true;
			if (add) {
				if (isVertical) {
					c.usedByHowMany++;
					Debug.Log ("Circuito escolhido:" + c.circuit + " Tipo de fio" + c.GetMyType() +
						" used by: "+ c.usedByHowMany);
				}
				content.AddLast (c);
				bool add1 = false;
				if (verticalEdges [0] != null) {
					add1 = verticalEdges [0].InsertContent (c);
					add = add && add1;
				}
				if (verticalEdges [1] != null) {
					add1 = verticalEdges [1].InsertContent (c);
					add = add && add1;
				}
				if (!add) {
					Debug.Log("Erro!");
					if(g!=null) Destroy (g);
				}
			}
			//Debug.Log ("Retornando " + add);
			if (!add) 
				if(g!=null) Destroy (g);
			return add;
		}

		void Update(){
			//UpdateIcons ();
		}

		private void PrintConduits(){
			Debug.Log ("New insertion ::");
			foreach (Conductor con in content) {
				Debug.Log (con.Print ());
			}
		}

		public void RemoveContent(int i){
			Conductor[] array = new Conductor[content.Count];
			content.CopyTo (array, 0);
			if (!isVertical) {
				content.Remove (array [i]);
				if (verticalEdges [0] != null) {
					Conductor[] arrayAux = new Conductor[verticalEdges [0].content.Count];
					verticalEdges [0].content.CopyTo (arrayAux, 0);
					for(int j = 0; j < arrayAux.Length; j++){
						if (array [i].circuit == arrayAux [j].circuit && array [i].GetMyType() == arrayAux [j].GetMyType()) {
							arrayAux [j].usedByHowMany--;
							Debug.Log ("Used by: " + arrayAux [j].usedByHowMany);
							if (arrayAux [j].usedByHowMany == 0) {
								verticalEdges [0].content.Remove (arrayAux [j]);
								Debug.Log ("Removed");
							}
						}
					}
				}
				if (verticalEdges [1] != null) {
					Conductor[] arrayAux = new Conductor[verticalEdges [1].content.Count];
					verticalEdges [1].content.CopyTo (arrayAux, 0);
					for(int j = 0; j < arrayAux.Length; j++){
						if (array [i].circuit == arrayAux [j].circuit && array [i].GetMyType() == arrayAux [j].GetMyType()) {
							arrayAux [j].usedByHowMany--;
							Debug.Log ("Used by: " + arrayAux [j].usedByHowMany);
							if (arrayAux [j].usedByHowMany == 0) {
								verticalEdges [1].content.Remove (arrayAux [j]);
								Debug.Log ("Removed");
							}
						}
					}
				}
				Destroy(array [i].GetGameObject ());
			}
			else {
				array [i].usedByHowMany--;

			}
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


