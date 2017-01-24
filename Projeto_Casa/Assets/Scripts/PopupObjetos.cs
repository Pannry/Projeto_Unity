using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupObjetos : MonoBehaviour {

	public Canvas Canvas_subMenu;
	public bool subMenuOpen = false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void SubMenuOpen(){
		if (subMenuOpen == false){
			subMenuOpen = true;
			Canvas_subMenu.enabled = true;
		}
		else if (subMenuOpen == true){
			subMenuOpen = false;
			Canvas_subMenu.enabled = false;
		}
	}

}
