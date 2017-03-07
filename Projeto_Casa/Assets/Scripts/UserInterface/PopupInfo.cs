using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace AssemblyCSharp
{
	public class PopupInfo : Popup
	{

		private GameObject edge;
		public GameObject contentToAdd;
		public GameObject popupChooseCircuit;
		private LinkedList<GameObject> myContent;
		private ArrayList selectedEdges;
		private GameObject view;
		private Text length;
		private Dropdown type;
		private Dropdown conductor;
		private bool isClosed = true;

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
			if (gameObject.name == "PopupInfo(Clone)") {
				if (selectedEdges == null)
					GetContent (edge);
				else {
					foreach (Edge e in selectedEdges) {
						GetContent (e.gameObject);
					}
				}
			}
		}

		public void GetContent(GameObject e){
			GameObject content = GameObject.Find ("Content");
			ArrayList conductors = new ArrayList ();
			if (myContent.Count != 0) {
				foreach (GameObject c in myContent) {
					conductors.Add (c.GetComponent<Conductor>());
				}
			}
			foreach (Conductor c in e.GetComponent<Edge>().content) {
				bool contains = false;
				foreach (Conductor aux in conductors) {
					if (c.GetSwitchBoard () == aux.GetSwitchBoard () &&
						c.GetMyType () == aux.GetMyType () &&
						c.GetConductor () == aux.GetConductor ())
						contains = true;
				}
				if (!contains) {
					GameObject info = Instantiate (contentToAdd);
					info.transform.SetParent (content.transform, false);
					info.transform.position += new Vector3 (0, -50 * myContent.Count, 0);
					Text[] array = info.GetComponentsInChildren<Text> ();
					foreach (Text t in array) {				
						if (t.name == "Conductor") {
							t.text += c.GetConductor ();
						}
						if (t.name == "Type") {
							t.text += c.GetMyType ();
						}
						if (t.name == "Circuit") {
							t.text += " " + c.GetSwitchBoard ();
						}
					}
					Conductor aux = info.AddComponent<Conductor> ();
					aux.SetSwitchBoard (c.GetSwitchBoard ());
					aux.SetType(c.GetMyType());
					aux.SetConductor (c.GetConductor ());
					info.GetComponent<PopupInfo> ().SetPopupObject (info);
					myContent.AddLast (info);
				}

			}
		}

		public static void CreateMultiEdgesInfoBox(GameObject popup, Controller e, LinkedList<Edge> edges, ArrayList myEdges){
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
			panel.GetComponent<PopupInfo> ().SetEdge(myEdges);
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
			float result = 0F;
			foreach (Edge ed in myEdges) {
				LineRenderer lr = ed.GetComponent<LineRenderer> ();
				Edge a = ed.GetComponent<Edge> ();
				if (!a.isVertical) {
					float xratio = panel.GetComponent<PopupInfo> ().Controller.GetRatios () [0];
					float zratio = panel.GetComponent<PopupInfo> ().Controller.GetRatios () [1];
					Vector3 reworkedA = new Vector3 (lr.GetPosition (0).x * (1 / xratio),
						lr.GetPosition (0).y, lr.GetPosition (0).z * (1 / zratio));
					Vector3 reworkedB = new Vector3 (lr.GetPosition (1).x * (1 / xratio),
						lr.GetPosition (1).y, lr.GetPosition (1).z * (1 / zratio));
					result += Vector3.Distance (reworkedA, reworkedB);
					//Debug.Log (result.ToString ());
					Edge verticalComponentA = ed.GetComponent<Edge> ().GetVEdges () [0];
					Edge verticalComponentB = ed.GetComponent<Edge> ().GetVEdges () [1];
					if (verticalComponentA != null || verticalComponentB != null) {
						if (verticalComponentB != null) {
							LineRenderer rb = verticalComponentB.gameObject.GetComponent<LineRenderer> ();
							float distB = Vector3.Distance (rb.GetPosition (0), rb.GetPosition (1));
							result += distB;
						}
						if (verticalComponentA != null) {
							LineRenderer ra = verticalComponentA.gameObject.GetComponent<LineRenderer> ();
							float distA = Vector3.Distance (ra.GetPosition (0), ra.GetPosition (1));
							result += distA;
						}
					}
				}
			}
			if (result.ToString ().Length > 6) {
				panel.GetComponent<PopupInfo> ().length.text += " " + result.ToString ().Remove (5);
			}
			else
				panel.GetComponent<PopupInfo> ().length.text += " " + result.ToString ();
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
			LineRenderer lr = myEdge.GetComponent<LineRenderer> ();
			Edge a = myEdge.GetComponent<Edge> ();
			if (!a.isVertical) {
				float xratio = panel.GetComponent<PopupInfo> ().Controller.GetRatios () [0];
				float zratio = panel.GetComponent<PopupInfo> ().Controller.GetRatios () [1];
				Vector3 reworkedA = new Vector3 (lr.GetPosition (0).x * (1 / xratio),
					lr.GetPosition (0).y, lr.GetPosition (0).z * (1 / zratio));
				Vector3 reworkedB = new Vector3 (lr.GetPosition (1).x * (1 / xratio),
					lr.GetPosition (1).y, lr.GetPosition (1).z * (1 / zratio));
				float result = Vector3.Distance (reworkedA, reworkedB);
				//Debug.Log (result.ToString ());
				if (result.ToString ().Length > 6) {
					Edge verticalComponentA = myEdge.GetComponent<Edge> ().GetVEdges () [0];
					Edge verticalComponentB = myEdge.GetComponent<Edge> ().GetVEdges () [1];
					if (verticalComponentA != null || verticalComponentB != null) {
						if (verticalComponentB != null) {
							LineRenderer rb = verticalComponentB.gameObject.GetComponent<LineRenderer>();
							float distB = Vector3.Distance (rb.GetPosition (0), rb.GetPosition (1));
							result += distB;
						}
						if (verticalComponentA != null){
							LineRenderer ra = verticalComponentA.gameObject.GetComponent<LineRenderer>();
							float distA = Vector3.Distance (ra.GetPosition (0), ra.GetPosition (1));
							result += distA;
						}
						panel.GetComponent<PopupInfo> ().length.text += " " + result.ToString ().Remove (5);
					}
					else
						panel.GetComponent<PopupInfo> ().length.text += " " + result.ToString ().Remove (5);
				} else {
					panel.GetComponent<PopupInfo> ().length.text += " " + result.ToString ();
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

		private void SetEdge(ArrayList _edge){
			selectedEdges = _edge;
		}

		public void OnClickExit(){
			Controller.popupOpen = false;
			Destroy (canvas);
		}


		public void OnClickChooseCircuit(){
			if (isClosed) {
				ArrayList frontier = new ArrayList ();
				if (selectedEdges != null)
					edge = ((Edge)selectedEdges [0]).gameObject;
				frontier.Add (edge.GetComponent<Edge> ().firstVertex.GetComponent<Node> ());
				ArrayList explored = new ArrayList ();
				ArrayList result = new ArrayList ();
				Search.BreadthFirstSearch (explored, frontier, result);
				//Debug.Log ("Total de Quadros: " + result.Count);
				GameObject popup = Instantiate (popupChooseCircuit);
				GameObject canvas = GameObject.Find ("Canvas");
				popup.transform.SetParent (canvas.transform, false);
				GameObject.Find ("AllCircuits").GetComponent<Text> ().text += "\n";
				string s = "Cancelar";
				List<string> slist = new List<string> ();
				slist.Add (s);
				GameObject.Find ("ChooseCircuit").GetComponent<Dropdown> ().AddOptions (slist);
				foreach (Node n in result) {
					int totalDeCircuitos = n.gameObject.GetComponent<InfoQadroEletrico> ().GetNumberOfCircuits ();
					for (int i = 0; i < totalDeCircuitos; i++) {
						s =n.gameObject.GetComponent<InfoQadroEletrico> ().GetID() + i;
						GameObject.Find ("AllCircuits").GetComponent<Text> ().text += " " + s;
						slist = new List<string> ();
						slist.Add (s);
						GameObject.Find ("ChooseCircuit").GetComponent<Dropdown> ().AddOptions (slist);
					}
				}
				isClosed = false;
				if (result.Count == 0) {
					isClosed = true;
					Destroy (GameObject.Find("PopupChooseCircuit(Clone)"));
				}
			}

		}

		public void OnClickAdd(){
			PopupInfo myPopup = GameObject.Find ("PopupInfo(Clone)").GetComponent<PopupInfo> ();
			Dropdown options = GameObject.Find ("ChooseCircuit").GetComponent<Dropdown> ();
			string circuit = options.captionText.text;
			if (circuit != "Cancelar") {
				bool add = false;

				GameObject content = GameObject.Find ("Content");
				GameObject info = Instantiate (contentToAdd);
				info.transform.SetParent (content.transform, false);
				info.transform.position += new Vector3 (0, -50 * myPopup.myContent.Count, 0);
				Text[] array = info.GetComponentsInChildren<Text> ();
				Conductor toInsert = new Conductor ();
				foreach (Text t in array) {				
					if (t.name == "Conductor") {
						string s = "";
						if (myPopup.conductor.value == 0)
							s = " Fio";
						else
							s = " Cabo";
						t.text += s;
						toInsert.SetConductor (s);
					}
					if (t.name == "Type") {
						string s = "";
						switch (myPopup.type.value) {
						case 0:
							s = " Terra";
							break;
						case 1:
							s = " Retorno";
							break;
						case 2:
							s = " Fase";
							break;
						case 3:
							s = "Neutro";
							break;
						}
						t.text += s;
						toInsert.SetType (s);

						string resultString = Regex.Match (circuit, @"\d+").Value;
						int circuit_int = -1;
						int.TryParse (resultString, out circuit_int);
						toInsert.SetCircuit(circuit_int);
						toInsert.SetSwitchBoard (circuit);

						if (myPopup.selectedEdges == null) {
							add = myPopup.edge.GetComponent<Edge> ().InsertContent (toInsert,false);
						} else {
							foreach (Edge e in myPopup.selectedEdges) {
								Conductor clone = new Conductor (toInsert);
								add = (e.InsertContent (clone,true) || add);
								//Debug.Log ("add: " + add);
							}
						}
					}
					if (t.name == "Circuit")
						t.text += " " + circuit;
				}
				if (add) {
					info.GetComponent<PopupInfo> ().SetPopupObject (info);
					Conductor aux = info.AddComponent<Conductor> () as Conductor;
					aux.SetSwitchBoard (circuit);
					aux.SetType (toInsert.GetMyType ());
					aux.SetConductor (toInsert.GetConductor ());
					myPopup.myContent.AddLast (info);
					//Ainda testando labels...apaga aqui quando for mostrar @@@@@@@@@@@@@
					//toInsert.DrawLabel (myPopup.edge);
				}
				if (!add) {
					Destroy (info);
				}
			}
			Destroy (GameObject.Find("PopupChooseCircuit(Clone)"));
			myPopup.isClosed = true;
		}

		public override void OnClickToDestroy(){
			GameObject parent = GameObject.Find ("PopupInfo(Clone)");
			GameObject[] array = new GameObject[parent.GetComponentInParent<PopupInfo>().myContent.Count];
			//Debug.Log ("Counter: " +parent.GetComponentInParent<PopupInfo>().myContent.Count);
			parent.GetComponentInParent<PopupInfo>().myContent.CopyTo (array, 0);
			base.OnClickToDestroy ();
			for (int i = 0; i < array.Length; i++) {
				if (array [i] == gameObject) {
					//chama método
					if (parent.GetComponent<PopupInfo>().selectedEdges == null)
						parent.GetComponentInParent<PopupInfo> ().edge.GetComponent<Edge> ().RemoveContent (array[i].GetComponent<Conductor>());
					else {
						foreach (Edge e in parent.GetComponent<PopupInfo>().selectedEdges) {
							e.RemoveContent (array [i].GetComponent<Conductor> ());
						}
					}
					break;
				}
			}
		}

	}
}

