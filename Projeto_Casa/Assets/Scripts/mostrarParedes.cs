using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class mostrarParedes : MonoBehaviour {

	private bool opt;

	public GameObject walls;

	public void clickTest() {
		if (opt == true) {
			walls.GetComponent<Renderer>().enabled = !walls.GetComponent<Renderer>().enabled;
			opt = !opt;
			Debug.Log (opt);
		} else {
			walls.GetComponent<Renderer>().enabled = !walls.GetComponent<Renderer>().enabled;
			opt = !opt;
			Debug.Log (opt);
		}
	}

}


/*

	void Update() {

		if (Input.GetKeyDown(KeyCode.Z)) {
			// show
			// renderer.enabled = true;
			walls.GetComponent<Renderer>().enabled = true;
			Debug.Log ("z press!");
		}

		if (Input.GetKeyDown(KeyCode.X)) {
			// hide
			// renderer.enabled = false;
			walls.GetComponent<Renderer>().enabled = false;

			Debug.Log ("x press!");
		}

	}


*/