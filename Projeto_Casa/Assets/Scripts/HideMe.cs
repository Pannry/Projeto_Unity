using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class HideMe : MonoBehaviour
	{
		void Update(){
			GameObject main = GameObject.FindGameObjectWithTag (Tags.Planta ());
			if (main.GetComponent<Controller> ().popupOpen) {
				gameObject.GetComponent<Canvas> ().enabled = false;
			}
			else
				gameObject.GetComponent<Canvas> ().enabled = true;
		}
	}
}

