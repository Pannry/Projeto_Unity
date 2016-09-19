using UnityEngine;
using System.Collections;

public class ClickEvent : MonoBehaviour {
	
	Ray ray;
	RaycastHit hit;
	public GameObject prefab;
	// Use this for initialization
	void Start () {
		 	
	}
	
	// Update is called once per frame - Do jeito q tá aqui só funfa pra main camera
	// Mas agente pode fazer isso...
	void Update () {
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray,out hit))
		{
			if(Input.GetButtonDown("Fire1")){
				GameObject obj=Instantiate(prefab,new Vector3(hit.point.x,2.2F,hit.point.z), Quaternion.identity) as GameObject;
				obj.transform.Rotate(new Vector3(90F,0F,0F));
			}
		}	
	}
}


//void update antiga: Desce ai o unity
// if antigo Input.GetButtonDown("Fire1")
//			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
//			Debug.Log(Input.mousePosition);
//			cube.transform.localPosition = Input.mousePosition/50F;
////			Matrix4x4 m = Camera.main.cameraToWorldMatrix;
////			cube.transform.localPosition = m.MultiplyPoint(Input.mousePosition/50F);
//			cube.transform.localScale = new Vector3(10F,10F,10F);
//			cube.GetComponent<Renderer>().material.color = Color.red;
