using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AssemblyCSharp {
	public class InfoQadroEletrico : MonoBehaviour {
		public GameObject janelaInformacao;

		private string nomeDoQuadro;
		private int quantidadeDeCircuitos;

		// Use this for initialization
		void Start () {
			Debug.Log (gameObject);
			GameObject.Find ("Building Plot").GetComponent<Controller> ().popupOpen = true;
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void eventoOk(){
			Debug.Log (gameObject);
			InputField[] array = janelaInformacao.GetComponentsInChildren<InputField> ();
			foreach (InputField field in array) {
				if (field.name == "InputFieldIdentificador") nomeDoQuadro = field.text;
				if (field.name == "InputFieldNumeroCircuitos")
					int.TryParse (field.text, out quantidadeDeCircuitos);
				
			}
			Debug.Log (nomeDoQuadro);
			Destroy (janelaInformacao);
			GameObject.Find ("Building Plot").GetComponent<Controller> ().popupOpen = false;
		}
			

	}
}