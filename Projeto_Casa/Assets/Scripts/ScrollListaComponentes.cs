using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollListaComponentes : MonoBehaviour {
    public GameObject scrollListaComponentes;
    private bool key = false;
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.I) && key == false) {
            key = true;
            GameObject scroll = Instantiate(scrollListaComponentes);
            AssemblyCSharp.Controller scriptController = GameObject.FindGameObjectWithTag("planta").GetComponent<AssemblyCSharp.Controller>();

            string text = "";
            //foreach (AssemblyCSharp.Node edge in scriptController.Nodes) {

            //    text += edge.GetComponent<Text>().text + "\n";
            //}

            foreach(KeyValuePair<string, int> dic in AssemblyCSharp.Node.Quantidade){
                text += dic.Key + " x " + dic.Value + "\n";
            }

			text += "Total de metros do eletroduto: " + GetCondutorSize ();
			text += "\nTotal de metros de fio fase: " + GetCondutorSize ("fase");
			text += "\nTotal de metros de fio retorno: " + GetCondutorSize ("retorno");
			text += "\nTotal de metros de fio terra: " + GetCondutorSize ("terra");
			text += "\nTotal de metros de fio neutro: " + GetCondutorSize ("neutro");

            Debug.Log(scroll);
            scroll.GetComponentInChildren<Text>().text = text;
        }       
		else if (Input.GetKeyDown (KeyCode.I) && key == true) {
			Destroy(GameObject.Find ("ListaDosComponentes(Clone)"));
			key = false;
		}
	}

	/// <summary>
	/// Gets the size of the tubulation.
	/// </summary>
	private float GetCondutorSize(){
		AssemblyCSharp.Controller scriptController = GameObject.FindGameObjectWithTag("planta").GetComponent<AssemblyCSharp.Controller>();
		float result = 0;
		float resultrw = 0;
		foreach (AssemblyCSharp.Edge e in scriptController.Edges) {
				if (e.gameObject != null) {
				LineRenderer lr = e.gameObject.GetComponent<LineRenderer> ();
				Vector3 reworkedA = new Vector3 (lr.GetPosition (0).x * (1 / scriptController.GetRatios()[0]),
					lr.GetPosition (0).y, lr.GetPosition (0).z * (1 / scriptController.GetRatios()[1]));
				Vector3 reworkedB = new Vector3 (lr.GetPosition (1).x * (1 / scriptController.GetRatios()[0]),
					lr.GetPosition (1).y, lr.GetPosition (1).z * (1 / scriptController.GetRatios()[1]));
					result += Vector3.Distance (lr.GetPosition (0), lr.GetPosition (1));
					resultrw += Vector3.Distance (reworkedA, reworkedB);
				}
			}
		return resultrw;
	}

	private float GetCondutorSize(string wire){
		AssemblyCSharp.Controller scriptController = GameObject.FindGameObjectWithTag("planta").GetComponent<AssemblyCSharp.Controller>();
		float result = 0;
		float resultrw = 0;
		foreach (AssemblyCSharp.Edge e in scriptController.Edges) {
			if (e.gameObject != null) {
				foreach (AssemblyCSharp.Conductor c in e.content) {
					//Debug.Log (c.GetMyType ().ToLower () + " " + wire);
					if (c.GetMyType ().ToLower().Trim() == wire) {
						LineRenderer lr = e.gameObject.GetComponent<LineRenderer> ();
						Vector3 reworkedA = new Vector3 (lr.GetPosition (0).x * (1 / scriptController.GetRatios()[0]),
							lr.GetPosition (0).y, lr.GetPosition (0).z * (1 / scriptController.GetRatios()[1]));
						Vector3 reworkedB = new Vector3 (lr.GetPosition (1).x * (1 / scriptController.GetRatios()[0]),
							lr.GetPosition (1).y, lr.GetPosition (1).z * (1 / scriptController.GetRatios()[1]));
						result += Vector3.Distance (lr.GetPosition (0), lr.GetPosition (1));
						resultrw += Vector3.Distance (reworkedA, reworkedB);
					}
				}
			}
		}
		return resultrw;
	}

}
