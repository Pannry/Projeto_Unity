using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AssemblyCSharp
{
	public class PopupInfo : Popup
	{

		private GameObject edge;
		public GameObject contentToAdd;
		private LinkedList<GameObject> myContent;
		private GameObject view;
		private Text length;
		private Dropdown type;
		private Dropdown conductor;

		void Update(){
			if (myContent != null && myContent.Count > 0) {
				bool trigger = false;
				GameObject[] array = new GameObject[myContent.Count];
				myContent.CopyTo (array, 0);

				for (int i = 0; i < array.Length; i++) {
					if (trigger) {
						array[i].transform.position += new Vector3 (0, +50, 0);
					}
					if (array [i] == null) {
						myContent.Remove (array [i]);
						trigger = true;
					}
				}
			}
		}

		void Start(){
			if(gameObject.name == "PopupInfo(Clone)")
				GetContent ();
		}

		public void GetContent(){
			GameObject content = GameObject.Find ("Content");
			foreach (Conductor c in edge.GetComponent<Edge>().content) {
				GameObject info = Instantiate (contentToAdd);
				info.transform.SetParent (content.transform, false);
				info.transform.position += new Vector3 (0, -50*myContent.Count, 0);
				Text[] array = info.GetComponentsInChildren<Text> ();
				foreach (Text t in array) {				
					if (t.name == "Conductor") {
						t.text += c.GetConductor ();
					}
					if (t.name == "Type") {
						t.text += c.GetType();
					}
				}
				info.GetComponent<PopupInfo> ().SetPopupObject (info);
				myContent.AddLast (info);

			}
		}

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
			panel.GetComponent<PopupInfo> ().myContent = new LinkedList<GameObject> ();
			panel.GetComponent<PopupInfo> ().SetPopupObject (panel);
			panel.GetComponent<PopupInfo> ().SetCanvasObject (canvas);
			panel.GetComponent<PopupInfo> ().SetControllerObject (e);
			panel.GetComponent<PopupInfo> ().SetEdge(myEdge);
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
				if (a.gameObject.Equals (myEdge)) {
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
			conductor = mykind;
		}

		private void SetEdge(GameObject _edge){
			edge = _edge;
		}

		public void OnClickExit(){
			Controller.popupOpen = false;
			Destroy (canvas);
		}

		public void OnClickAdd(){
			GameObject content = GameObject.Find ("Content");
			GameObject info = Instantiate (contentToAdd);
			info.transform.SetParent (content.transform, false);
			info.transform.position += new Vector3 (0, -50*myContent.Count, 0);
			Text[] array = info.GetComponentsInChildren<Text> ();
			Conductor toInsert = new Conductor();
			foreach (Text t in array) {				
				if (t.name == "Conductor") {
					string s = "";
					if(conductor.value == 0)
						s = " Fio";
					else
						s = " Cabo";
					t.text += s;
					toInsert.SetConductor (s);
				}
				if (t.name == "Type") {
					string s = "";
					switch (type.value) {
					case 0:
						s = " Terra";
						break;
					case 1:
						s = " Retorno";
						break;
					case 2:
						s = " Fase";
						break;
					}
					t.text += s;
					toInsert.SetType (s);
					edge.GetComponent<Edge> ().InsertContent (toInsert);
				}
			}
			info.GetComponent<PopupInfo> ().SetPopupObject (info);
			myContent.AddLast (info);
		}
			
		public override void OnClickToDestroy(){
			GameObject parent = GameObject.Find ("PopupInfo(Clone)");
			GameObject[] array = new GameObject[parent.GetComponentInParent<PopupInfo>().myContent.Count];
			parent.GetComponentInParent<PopupInfo>().myContent.CopyTo (array, 0);
			base.OnClickToDestroy ();
			for (int i = 0; i < array.Length; i++) {
				if (array [i] == gameObject) {
					//chama método
					parent.GetComponentInParent<PopupInfo>().edge.GetComponent<Edge>().RemoveContent(i);
					break;
				}
			}
		}
			
	}
}

