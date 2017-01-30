using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowWalls : MonoBehaviour {

	private bool opt;

	private GameObject[] walls;

	public void clickTest() {
		walls = GameObject.FindGameObjectsWithTag("parede");
		if (opt == true) {
			for(int i = 0; i < walls.Length; i++)
				walls[i].GetComponent<Renderer>().enabled = !walls[i].GetComponent<Renderer>().enabled;
			opt = !opt;
			Debug.Log (opt);
		} else {
			for(int i = 0; i < walls.Length; i++)
				walls[i].GetComponent<Renderer>().enabled = !walls[i].GetComponent<Renderer>().enabled;
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