using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class WallCreator:MonoBehaviour
	{
		private Ray ray;
		private RaycastHit hit;
		private GameObject prefab;
		private GameObject[] squares;
		private GameObject lastObject;

		void Start(){
			squares = new GameObject[2];
			squares [0] = null;
			squares [1] = null;
		}

		void Update(){
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				float height = GetComponent<Controller>().GetHeight();
				try{
					CreateWall (height);
				}
				catch(IndexOutOfRangeException e){
					Debug.Log("Problema na criacao no elemento parede, redefina o elemento.");
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
		/// Creates the wall.
		/// Na primeira acao é criado um ponto inicial da parede.
		/// Depois um segundo ponto, onde são definidos largura e comprimento.
		/// </summary>
		/// <param name="height">Height.</param>
		public void CreateWall(float height){
			int option = GetComponent<Controller> ().GetOption ();

			// Se o raio estiver colidindo com a planta, cria um marcador.
			//Debug.Log( hit.transform.gameObject.tag == Tags.Planta());
			//Debug.Log (option);
			if(Input.GetButtonDown("Fire1") && option == 2 && hit.transform.gameObject.tag == Tags.Planta() ){ 
				Debug.Log ("teste");
				DefinePrefab ();
				lastObject = Instantiate(prefab,new Vector3(hit.point.x,0,hit.point.z), Quaternion.identity) as GameObject;
				// Se o primeiro marcador nao estiver criado, o segundo tambem nao foi criado, logo nao ha marcas
				// entao cria-se um marcador na primeira celula. Vai pro else no proximo marcador criado.
				if (squares [0] == null) {
					squares [0] = lastObject;
				}
				else {
					squares [1] = lastObject;
					// Guardo a posicao inicial do primeiro marcador.
					Vector3 pos = squares [0].transform.position;
					// Calculo o ponto medio, e coloco o primeiro marcador la.
					squares [0].transform.position = (pos + squares [1].transform.position) / 2;
					// Coloco o marcador a metade da altura que sera escalonado, para que a parede
					// fique exatamente do tamanho que eu quero, e tocando no chao.
					squares [0].transform.position += new Vector3 (0, height/2, 0);
					// Calculo quanto eu devo escalonar em cada eixo.
					float scaleX = (pos.x - squares [1].transform.position.x);
					float scaleY = (pos.y - squares [1].transform.position.y);
					float scaleZ = (pos.z - squares [1].transform.position.z);
					// Se o numero estiver negativo eu transformo.
					if(scaleX < 0)
						scaleX = scaleX*-1;
					if(scaleY < 0)
						scaleY = scaleY*-1;
					if(scaleZ < 0)
						scaleZ = scaleZ*-1;

					// Finalmente eu escalono o objeto.
					squares [0].transform.localScale += new Vector3 (scaleX, height, scaleZ);
					// Reseta os marcadores e deleta a parede(marcador2) nao utilizada.
					squares [0].tag = "parede";
					squares [0] = null;
					Destroy (squares [1]);
					squares [1] = null;
				}
			}

		}
	}
}

