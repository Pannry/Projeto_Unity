using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

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
		public bool isDown = false;
		public float radius;
		public LinkedList<Conductor> content;
		public Edge[] verticalEdges;
		private int multiplier; // multiplicador de tamanho.

		public void IncrementMultiplier(){
			multiplier += 1;
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
			ArrayList group = new ArrayList ();
			int counter = 0;
			if(!isVertical)
				foreach(Conductor c in content){
					if (c.GetLabel () != null) {
						c.GetLabel ().transform.rotation = Quaternion.identity;
					}						
					GameObject g = c.GetGameObject ();
					g.transform.rotation = Quaternion.identity;
					po1 = GetComponent<LineRenderer> ().GetPosition (0);
					po2 = GetComponent<LineRenderer> ().GetPosition (1);
					g.transform.Rotate (90, 0, 0);
					if (group.Count != 0 && (c.circuit != ((Conductor)group [0]).circuit ||
						c.GetSwitchBoard() != ((Conductor)group [0]).GetSwitchBoard())) {
						counter++;
					}
					if (group.Count == 0 && counter == 0) {
						Debug.Log ("c1");
						group.Add (c);
						po1 = (po1 + po2) / 2;
						g.transform.position = po1;
						RotateIcon (po1, po2, g);
						c.DrawLabel (gameObject);
					}
					if (group.Count == 1 && counter == 1) {
						Debug.Log ("c2");
						group.Add (c);
						po1 = (po1 + po2) / 2;
						po2 = gameObject.GetComponent<LineRenderer> ().GetPosition (0);
						po1 = (po1 + po2) / 2;
						g.transform.position = po1;
						RotateIcon (po1, po2, g);
						c.DrawLabel (gameObject);
					}
					if (counter == 2 && counter == 2) {
						Debug.Log ("c3");
						group.Add (c);
						po1 = (po1 + po2) / 2;
						po1 = (po1 + po2) / 2;
						g.transform.position = po1;
						RotateIcon (po1, po2, g);
						c.DrawLabel (gameObject);
					}
					for (int i = 0; i < group.Count; i++) {
						Debug.Log ("@Test.");
						if (group [i] != null && c.GetSwitchBoard () == ((Conductor)group [i]).GetSwitchBoard ()
							&& ((Conductor)group [i]).GetMyType () != c.GetMyType ()) {
							Debug.Log ("Test.");
							g.transform.position = ((Conductor)group [i]).GetGameObject ().transform.position;
							g.transform.position += ((Conductor)group [i]).GetGameObject ().transform.right * .1F;
							g.transform.rotation = ((Conductor)group [i]).GetGameObject ().transform.rotation;
							group [i] = c;
						}
					}
				}
		}

		private void RotateIcon(Vector3 po1, Vector3 po2, GameObject g){

			po1 = gameObject.GetComponent<LineRenderer> ().GetPosition (0);
			po2 = gameObject.GetComponent<LineRenderer> ().GetPosition (1);
			double deltay = po2.z - po1.z;
			double deltax = po2.x - po1.x;
			double m = deltay / deltax;
			float angle = (float)(Math.Atan (m))* 57.2958F;
			g.transform.Rotate(new Vector3 (0, 0, angle));//Convertendod de radiano para grao.
		}


		public bool InsertContent(Conductor c, string circuit){
			Debug.Log ("Metodo");
			ArrayList frontier = new ArrayList ();
			frontier.Add (inv.GetComponent<Node> ());
			ArrayList explored = new ArrayList ();
			string s = circuit;
			s = Regex.Replace (s, "[0-9.]", "");
			if (!isVertical && !Search.BreadthFirstSearch (s, explored, frontier)) {
				Debug.Log("Quadro não pertencente");
				return false;
			}
			if (content.Count >= 9*multiplier) {
				Debug.Log ("Erro, maximo de fios tolerados alcancado.");
				return false;
				//Deve-se perguntar se o usuario quer duplicar o eletroduto.
				//Uma janela deve retornar um booleano aqui.
				//Crie outra classe.
			}
			LinkedList<int> totalCircuitsGroups = new LinkedList<int>();
			bool rotated = false;
			bool add = false;
			// Apagar proxima linha quando na classe popupinfo for adicionada o circuito do condutor.
			Vector3 po1 = new Vector3(), po2 = new Vector3();
			GameObject g = null;
			if (!isVertical) {
				string resultString = Regex.Match (circuit, @"\d+").Value;
				int circuit_int = -1;
				int.TryParse (resultString, out circuit_int);
				c.SetCircuit(circuit_int);
				c.SetSwitchBoard (circuit);
				po1 = gameObject.GetComponent<LineRenderer> ().GetPosition (0);
				po2 = gameObject.GetComponent<LineRenderer> ().GetPosition (1);
				g = Instantiate (GameObject.Find (c.GetMyType ().ToLower ().Trim ())) as GameObject;
			}
			totalCircuitsGroups.AddLast (c.circuit);
			/*if(isVertical) Debug.Log ("Circuito escolhido:" + c.circuit + " Tipo de fio" + c.GetMyType() +
				" used by: "+ c.usedByHowMany +" c::"+ content.Count);*/
			if(g!= null) c.SetGameObject (g);
			// Se nao havia fiacao, comeca adicionando no meio.
			if (content.Count == 0) {
				if (!isVertical) {
					po1 = (po1 + po2) / 2;
					g.transform.position = po1;
					RotateIcon (po1, po2, g);
					c.DrawLabel (gameObject);
				}
				add = true;
			} else {
				foreach (Conductor x in content) {
					if (x.GetMyType () == c.GetMyType () && x.circuit == c.circuit ) {
						if (!isVertical) {
							// Não aceita mesmo se forem quadros diferentes.
							// Estou supondo que quadros diferentes não são conexos.
							Debug.Log ("Erro! Jah existe um fio/cabo de mesmo tipo pertencente a este circuito.");
							if (g != null)
								Destroy (g);
							return false;
						} else {
							x.usedByHowMany++;
							return true;
						}
					} else if (x.circuit == c.circuit) {
						// Se esse condutor pertence ha um mesmo circuito que esta aqui, entao:
						add = true;
						if (!isVertical) {
							g.transform.position = x.GetGameObject ().transform.position;
							g.transform.position += x.GetGameObject ().transform.right * .1F;
							if (!rotated) {
								RotateIcon (po1, po2, g);
								rotated = true;
							}
						}
					}
					//Ve quantos circuitos diferentes tem no eletroduto.
					else{
						if (!totalCircuitsGroups.Contains (x.circuit)) {
							totalCircuitsGroups.AddLast (x.circuit);
						}
					}
				}
				// Se houver menos de 3[a partir de 1] circuitos diferentes, e nao foi desenhado o simbolo ainda.
				// Se houver mais, dara erro por ja haver 3 grupamentos de circuito e o usuario querer adicionar mais.
				if (totalCircuitsGroups.Count < 4 && !add && !isVertical) {
					po1 = (po1 + po2) / 2;
					if (totalCircuitsGroups.Count == 3) {
						// Agrupamento no final.
						po1 = (po1 + po2) / 2;
					}
					if (totalCircuitsGroups.Count == 2) {
						// Agrupamento no comeco
						po2 = gameObject.GetComponent<LineRenderer> ().GetPosition (0);
						po1 = (po1 + po2) / 2;
					}
					g.transform.position = po1;
					RotateIcon (po1, po2, g);
					c.DrawLabel (gameObject);
					add = true;
				} else if (totalCircuitsGroups.Count >= 4 && !isVertical) {
					Debug.Log ("Erro! Já existem 3 agrupamentos de circuitos nesse eletroduto, delete um agrupamento para inserir" +
						" um fio de outro circuito.");
				}

			}
			// Se ainda houver espaço para inserir na vertical, pode inserir.(Não tem icone, entao nao ha restriçao de grupamento.)
			if (!add && isVertical && content.Count < 9) 
				add = true;
			if (add) {
				content.AddLast (c);
				bool add1 = false;
				if (verticalEdges [0] != null && !isVertical) {
					Debug.Log ("Entrou no 0");
					add1 = verticalEdges [0].InsertContent (new Conductor(c),circuit);
					add = add && add1;
				}
				if (verticalEdges [1] != null&& !isVertical) {
					Debug.Log ("Entrou no 1");
					add1 = verticalEdges [1].InsertContent (new Conductor(c),circuit);
					add = add && add1;
				}
				if (!add) {
					Debug.Log("Erro!");
					if(g!=null) Destroy (g);
				}
				if (isVertical) {
					c.usedByHowMany++;
				}
			}
			if (!add) 
			if(g!=null) Destroy (g);
			return add;
		}

		void Update(){
			Vector3 a = gameObject.GetComponent<LineRenderer> ().GetPosition (0);
			Vector3 b = gameObject.GetComponent<LineRenderer> ().GetPosition (1);
			gameObject.transform.position = (a + b) / 2;
			if (isVertical) {
				verticalEdges [0] = null;
				verticalEdges [1] = null;
			}
		}

		private void PrintConduits(){
			Debug.Log ("Conduits ::");
			foreach (Conductor con in content) {
				Debug.Log (con.Print ());
			}
		}

		private void RemoveIfContains(Conductor mytarget){
			Conductor[] array = new Conductor[content.Count];
			content.CopyTo (array, 0);
			content = new LinkedList<Conductor> ();
			for (int i = 0; i < array.Length; i++) {
				if (!(mytarget.GetSwitchBoard () == array [i].GetSwitchBoard () &&
				    mytarget.GetMyType () == array [i].GetMyType () &&
					mytarget.GetConductor () == array [i].GetConductor ()))
					content.AddLast(array[i]);				
			}
		}

		public void RemoveContent(Conductor c){
			PrintConduits ();
			Conductor[] array = new Conductor[content.Count];
			content.CopyTo (array, 0);
			int i = -1;
			for (int j = 0; j < array.Length; j++) {
				Debug.Log (array [j].GetSwitchBoard () + " VS " + c.GetSwitchBoard ());
				Debug.Log (array [j].GetConductor () + " VS " + c.GetConductor ());
				Debug.Log (array [j].GetMyType () +" VS "+ c.GetMyType ());
				if (array [j].GetSwitchBoard () == c.GetSwitchBoard () &&
				    array [j].GetConductor () == c.GetConductor () &&
				    array [j].GetMyType () == c.GetMyType ())
					i = j;
			}
			if (i != -1) {
				if (!isVertical) {
					Destroy (array [i].GetLabel ());
					Debug.Log ("Elemento :::" + array [i].GetMyType ());
					RemoveIfContains (array [i]);
					if (verticalEdges [0] != null) {
						Conductor[] arrayAux = new Conductor[verticalEdges [0].content.Count];
						verticalEdges [0].content.CopyTo (arrayAux, 0);
						for (int j = 0; j < arrayAux.Length; j++) {
							if (array [i].GetSwitchBoard () == arrayAux [j].GetSwitchBoard () && array [i].GetMyType () == arrayAux [j].GetMyType ()) {
								arrayAux [j].usedByHowMany--;
								Debug.Log ("Used by: " + arrayAux [j].usedByHowMany);
								if (arrayAux [j].usedByHowMany <= 0) {
									Debug.Log ("ElementoVert :::" + arrayAux [j].GetMyType ());
									verticalEdges [0].RemoveIfContains (arrayAux [j]);
									Debug.Log ("Removed");
								}
							}
						}
					}
					if (verticalEdges [1] != null) {
						Conductor[] arrayAux = new Conductor[verticalEdges [1].content.Count];
						verticalEdges [1].content.CopyTo (arrayAux, 0);
						for (int j = 0; j < arrayAux.Length; j++) {
							if (array [i].GetSwitchBoard () == arrayAux [j].GetSwitchBoard () && array [i].GetMyType () == arrayAux [j].GetMyType ()) {
								arrayAux [j].usedByHowMany--;
								Debug.Log ("Used by: " + arrayAux [j].usedByHowMany);
								if (arrayAux [j].usedByHowMany <= 0) {
									Debug.Log ("ElementoVert :::" + arrayAux [j].GetMyType ());
									verticalEdges [1].RemoveIfContains (arrayAux [j]);
									Debug.Log ("Removed");
								}
							}
						}
					}
					Destroy (array [i].GetGameObject ());
				} else {
					array [i].usedByHowMany--;

				}
				PrintConduits ();
				UpdateIcons ();
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

