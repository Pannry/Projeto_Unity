using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AssemblyCSharp
{
	public class Popup : MonoBehaviour
	{
		protected GameObject panel;
		protected GameObject canvas;
		protected Controller Controller;

		public Popup ()
		{
		}

		protected void SetPopupObject(GameObject mypopup){
			panel = mypopup;
		}

		protected void SetCanvasObject(GameObject mycanvas){
			canvas = mycanvas;
		}

		protected void SetControllerObject(Controller e){
			Controller = e;
		}
	}
}

