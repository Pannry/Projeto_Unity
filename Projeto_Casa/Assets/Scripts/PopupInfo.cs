﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AssemblyCSharp
{
	public class PopupInfo : Popup
	{

		public GameObject contentToAdd;
		private int totalObjects;
		private GameObject view;
		private Text length;
		private Dropdown type;
		private Dropdown kind;

		public static void CreateInfoBox(GameObject popup, Controller e, LinkedList<Edge> edges, GameObject myEdge){
			//Creating a Canvas:
			GameObject canvas = new GameObject("Canvas");
			Canvas c = canvas.AddComponent<Canvas>();
			c.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.AddComponent<CanvasScaler>();
			canvas.AddComponent<GraphicRaycaster>();
			//Creating a Panel:
			GameObject panel = Instantiate(popup);
			panel.transform.SetParent (canvas.transform, false);
			panel.GetComponent<PopupInfo> ().totalObjects = 0;
			panel.GetComponent<PopupInfo> ().SetPopupObject (panel);
			panel.GetComponent<PopupInfo> ().SetCanvasObject (canvas);
			panel.GetComponent<PopupInfo> ().SetControllerObject (e);
			RectTransform[] array = panel.GetComponentsInChildren<RectTransform> ();
			foreach (RectTransform ele in array) {
				if (ele.name == "Length") {
					panel.GetComponent<PopupInfo> ().SetLengthObject (ele.gameObject.GetComponent<Text>());
				}
				if (ele.name == "Type") {
					panel.GetComponent<PopupInfo> ().SetTypeObject (ele.GetComponent<Dropdown>());
				}
				if (ele.name == "Conductor") {
					panel.GetComponent<PopupInfo> ().SetKindObject (ele.GetComponent<Dropdown>());
				}
				if(ele.name == "Scroll View"){
					panel.GetComponent<PopupInfo> ().SetViewObject (ele.gameObject);
				}
			}
			foreach ( Edge a in edges) {
				if (a.edge.Equals (myEdge)) {
					LineRenderer lr = myEdge.GetComponent<LineRenderer> ();
					if (!a.isVertical) {
						float xratio = panel.GetComponent<PopupInfo> ().Controller.GetRatios () [0];
						float zratio = panel.GetComponent<PopupInfo> ().Controller.GetRatios () [1];
						Vector3 reworkedA = new Vector3 (lr.GetPosition (0).x * (1 / xratio),
							lr.GetPosition (0).y, lr.GetPosition (0).z * (1 / zratio));
						Vector3 reworkedB = new Vector3 (lr.GetPosition (1).x * (1 / xratio),
							lr.GetPosition (1).y, lr.GetPosition (1).z * (1 / zratio));
						float result = Vector3.Distance (reworkedA, reworkedB);
						Debug.Log (result.ToString ());
						if(result.ToString().Length > 6)
							panel.GetComponent<PopupInfo> ().length.text +=" " +result.ToString ().Remove(5);
						else
							panel.GetComponent<PopupInfo> ().length.text +=" " +result.ToString ();
					} else {
						float result = Vector3.Distance (lr.GetPosition(0), lr.GetPosition(1));
						panel.GetComponent<PopupInfo> ().length.text +=" " +result.ToString ();
						Debug.Log (result.ToString ());
					}
				}
			}
		}

		private void SetViewObject(GameObject myview){
			view = myview;
		}

		private void SetLengthObject(Text mylength){
			length = mylength;
		}

		private void SetTypeObject(Dropdown mytype){
			type = mytype;
		}

		private void SetKindObject(Dropdown mykind){
			kind = mykind;
		}

		public void OnClickExit(){
			Controller.popupOpen = false;
			Destroy (canvas);
		}

		public void OnClickAdd(){
			GameObject viewport = GameObject.Find ("Viewport");
			GameObject info = Instantiate (contentToAdd);
			info.transform.SetParent (viewport.transform, false);
			info.transform.position += new Vector3 (0, -50*totalObjects, 0);
			totalObjects++;
		}
	}
}

