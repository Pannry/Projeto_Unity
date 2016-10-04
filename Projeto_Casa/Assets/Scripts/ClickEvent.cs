using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClickEvent : MonoBehaviour {
	
	Ray ray;
	RaycastHit hit;
	public float height;
	public GameObject[] prefab;
	private GameObject lastObject;
	private int option = 0; // Coloque o elemento prefab de acordo com sua respectiva opcao - 1
	private GameObject[] squares;

	void Start () {
		squares = new GameObject[2];
		squares [0] = null;
		squares [1] = null;
	}
	
	void Update () {
		Debug.Log (option);
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray,out hit))
		{
			if (Input.GetButtonDown ("Fire1") && hit.transform.gameObject.tag == "Untagged")
				lastObject = null;
			CreateNode (1);
			CreateNode (4);
			CreateWall ();
			CreateLine ();
			if(Input.GetButtonDown("Fire1") && option == 99 && hit.transform.gameObject.tag != "Untagged" 
				&& hit.transform.gameObject.tag != "Planta" ){
				Destroy (hit.transform.gameObject);
			}
		}	
	}

	public void CreateNode(int index){
		if(Input.GetButtonDown("Fire1") && option == index && hit.transform.gameObject.tag == "Planta" ){
			GameObject obj=Instantiate(prefab[option - 1],new Vector3(hit.point.x,height,hit.point.z), Quaternion.identity) as GameObject;
			obj.transform.Rotate(new Vector3(90F,0F,0F));
		}
	}

	public void CreateWall(){
		if(Input.GetButtonDown("Fire1") && option == 2 && hit.transform.gameObject.tag == "Planta" ){ 
			// Para ser click and click, troque esse evento por true. Desse jeito é click and drag[ACIMA];
			lastObject = Instantiate(prefab[option -1],new Vector3(hit.point.x,0,hit.point.z), Quaternion.identity) as GameObject;
			Transform tr = lastObject.transform;
			if (squares [0] == null)
				squares [0] = lastObject;
			else {
				squares [1] = lastObject;
				Vector3 pos = squares [0].transform.position;
				squares [0].transform.position = (pos + squares [1].transform.position) / 2;
				squares [0].transform.position += new Vector3 (0, 1.25F, 0);
				float scaleX = (pos.x - squares [1].transform.position.x);
				float scaleY = (pos.y - squares [1].transform.position.y);
				float scaleZ = (pos.z - squares [1].transform.position.z);
				if(scaleX < 0)
					scaleX = scaleX*-1;
				if(scaleY < 0)
					scaleY = scaleY*-1;
				if(scaleZ < 0)
					scaleZ = scaleZ*-1;


				squares [0].transform.localScale += new Vector3 (scaleX, 2.5F, scaleZ);
				//squares [0].GetComponent<BoxCollider> ().size += new Vector3 (scaleX, 2.5F, scaleZ);

				squares [0] = null;
				Destroy (squares [1]);
				squares [1] = null;
			}
		}

	}

	public void CreateLine(){
		if(Input.GetButton("Fire1") && option == 3 && hit.transform.gameObject.tag != "Untagged"){ 
			// Para ser click and click, troque esse evento por true. Desse jeito é click and drag[ACIMA];
			if(Input.GetButtonDown("Fire1")&& hit.transform.gameObject.tag == "CX OCTOGONAL"){
				lastObject = Instantiate(prefab[option -1],new Vector3(hit.point.x,height,hit.point.z), Quaternion.identity) as GameObject;
				LineRenderer lr = lastObject.GetComponent<LineRenderer>();
				lr.SetWidth(0.05F,0.05F);
				lr.SetVertexCount(2);
				lr.SetPosition(0,new Vector3(hit.point.x,height,hit.point.z));					
			}
			if(lastObject != null){
				LineRenderer lr = lastObject.GetComponent<LineRenderer>();
				lr.SetPosition(1,new Vector3(hit.point.x,height,hit.point.z));
			}
		}
		if(Input.GetButtonUp("Fire1") && hit.transform.gameObject.tag != "CX OCTOGONAL" 
			&& lastObject != null && option == 3){
			Destroy(lastObject);
		}
	}
	
	public void SetNewLine(){
		option = 3;
	}
	
	public void SetNewNode(){
		option = 1;
		height = 3;
	}

	public void SetNewWall(){
		option = 2;
		height = 3F;
	}

	public void SetDestroy(){
		option = 99;
	}

	public void SetExemplo(){
		option = 4;
		Debug.Log (option);
	}
}
