using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupObjectsMenu : MonoBehaviour {

	public Canvas parent;
	public Canvas submenu;
	public bool subMenuOpen = false;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (!parent.enabled) {
			submenu.enabled = false;
			subMenuOpen = false;
		}
	}
	public void SubMenuOpen(){
		if (subMenuOpen == false){
			subMenuOpen = true;
			submenu.enabled = true;
		}
		else if (subMenuOpen == true){
			subMenuOpen = false;
			submenu.enabled = false;
		}
	}

}
