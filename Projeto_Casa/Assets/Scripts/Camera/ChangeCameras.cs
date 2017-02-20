using UnityEngine;
using System.Collections;

public class ChangeCameras : MonoBehaviour {
	/// <summary>
	/// Ainda tá errado.
	/// </summary>
	public Camera[] cams;
	private bool showLabels = true;
	private GameObject[] array = null;

	void Update(){
		if(array != null)
			foreach (GameObject g in array) {
				if (showLabels)
					g.SetActive (true);
				else
					g.SetActive (false);
			}	
	}

	public void CamOneMove(){
		cams [0].enabled = true;
		cams [1].enabled = false;
		cams [2].enabled = false;
		GameObject.Find("ScriptCamera").GetComponent<ChangeCameras>().showLabels = true;
	}
	public void CamTwoMove(){
		GameObject.Find("ScriptCamera").GetComponent<ChangeCameras>().array = GameObject.FindGameObjectsWithTag("labels");
		cams [0].enabled = false;
		cams [1].enabled = true;
		cams [2].enabled = false;
		GameObject.Find("ScriptCamera").GetComponent<ChangeCameras>().showLabels = false;
	}
	public void CamThreeMove(){
		GameObject.Find("ScriptCamera").GetComponent<ChangeCameras>().array = GameObject.FindGameObjectsWithTag("labels");
		cams [0].enabled = false;
		cams [1].enabled = false;
		cams [2].enabled = true;
		GameObject.Find("ScriptCamera").GetComponent<ChangeCameras>().showLabels = false;
	}
}
