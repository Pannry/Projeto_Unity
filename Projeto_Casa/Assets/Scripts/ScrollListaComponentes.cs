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
           // foreach (AssemblyCSharp.Node edge in scriptController.Nodes) {

             //   text += edge.GetComponent<Text>().text + "\n";
            //}

            foreach(KeyValuePair<string, int> dic in AssemblyCSharp.Node.Quantidade){
                text += dic.Key + " " + dic.Value + "\n";
            }

            Debug.Log(scroll);
            scroll.GetComponentInChildren<Text>().text = text;
        }          
	}
}
