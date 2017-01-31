using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp{
	public class Controller : MonoBehaviour {
		// Define se foi aberta alguma popup:
		public bool popupOpen;
		// Prefabs requiridos:
		public GameObject popupRatio;
		public GameObject popupInfo;
		private float xratio, zratio;
		// Raio utilizado para colidir e selecionar um objeto.
		Ray ray;
		// Guarda a colisao do raio.
		RaycastHit hit;
		// Altura do proximo objeto a ser criado.
		public float height;
		// Array de objetos selecionados na ferramenta Unity. Esses objetos vao gerar nos,arestas,paredes,etc.
		public GameObject[] prefab;
		// Como são utilizadas funcoes de drag, eu utilizo esse atributo que guarda o ultimo objeto que foi
		// criado para ficar redesenhando.
		private GameObject lastObject;
		// Esse atributo tambem e utilizado para calcular a posicao do objeto no array prefab.
		private int option = 0; // Coloque o elemento prefab de acordo com sua respectiva opcao - 1
		// Aqui eh uma lista de nos, futuramente vai ser um grafo.
		private LinkedList<Edge> edges;
		private LinkedList<Node> nodes;
		// Construtor, seta o square como um array de 2 celulas e inicializa a lista de nos.
		void Start () {
			xratio = 0;
			zratio = 0;
			nodes = new LinkedList<Node> ();
			edges = new LinkedList<Edge> ();
			popupOpen = false;
		}
			
		void Update () {
			// Se houver alguma popup aberta, não pode haver iteração com a planta.
			if (popupOpen)
				option = 0;
			// Defino meu raio a partir da posicao do mouse.
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			// Note que esse if acontece para qualquer colisao. A colisao fica em hit.
			if(Physics.Raycast(ray,out hit))
			{
				if (Input.GetButtonDown ("Fire1") && hit.transform.gameObject.tag == "Untagged")
					lastObject = null;
				Rotate ();
				SetConfigRatio ();
				Info ();
				GarbageCollector ();
				//Debug.Log (":edges::" + edges.Count);
				//Debug.Log (":nodes::" + nodes.Count);
				GetTubulationSize ();
				Erase ();
			}	
		}

		public void Erase(){
			string tag = hit.transform.gameObject.tag;
			if(Input.GetButtonDown("Fire1") && option == 99 && tag != Tags.SemTag()
				&& tag != Tags.Planta() ){
				Destroy (hit.transform.gameObject);
				//Node.DestroyNode (nodes,hit.transform.gameObject);
			}
		}

		//Destroy Edges
		public void GarbageCollector(){
			if (nodes != null && nodes.Count > 0) {
				Node[] array = new Node[nodes.Count];
				nodes.CopyTo (array,0);
				for (int i = 0; i < array.Length; i++) {
					if (array [i] == null) {
						nodes.Remove (array [i]);
					}
					if (array [i] != null && !Node.isNode (array [i].gameObject.tag)) {
						nodes.Remove (array [i]);
						Destroy (array [i].gameObject);
					}
				}
			}
			if (edges != null && edges.Count > 0) {
				Edge[] array = new Edge[edges.Count];
				edges.CopyTo (array, 0);
				edges = new LinkedList<Edge> ();
				for (int i = 0; i < array.Length; i++) {
					if (array [i].outv == null || array [i].inv == null) {
						Destroy (array [i].gameObject);
					} else {
						edges.AddLast (array [i]);
					}
				}
			}
		}
		//Definir a proporção da planta.
		public void SetConfigRatio(){
			string tag = hit.transform.gameObject.tag;
			float x,z;
			x = hit.point.x;
			z = hit.point.z;
			if (Input.GetKey(KeyCode.S) && !popupOpen) {
				GetComponent<AssemblyCSharp.Events> ().SetNewLine ();
				height = 1.5F;
				if (Input.GetKeyDown (KeyCode.S)) {
					lastObject = Instantiate(prefab[option -1],new Vector3(x,height,z), Quaternion.identity) as GameObject;
					LineRenderer lr = lastObject.GetComponent<LineRenderer>();
					lr.startColor = new Vector4 (1F, 1F, 0F, 0F);
					lr.SetWidth(0.01F,0.01F);
					lr.SetVertexCount(5);
					lr.SetPosition(0,new Vector3(x,height,z));
				}
				if (lastObject != null) {
					LineRenderer lr = lastObject.GetComponent<LineRenderer>();
					lr.SetPosition(1,new Vector3(hit.point.x,height,lr.GetPosition(0).z));
					lr.SetPosition(2,new Vector3(hit.point.x,height,hit.point.z));
					lr.SetPosition(3,new Vector3(lr.GetPosition(0).x,height,hit.point.z));
					lr.SetPosition(4,new Vector3(lr.GetPosition(0).x,height,lr.GetPosition(0).z));
				}

			}
			if (Input.GetKeyUp (KeyCode.S)&& !popupOpen) {
				string[] vec = { tag };
				//new RatioPopup (vec, this, lastObject);
				PopupRatio.CreateRatioBox (popupRatio,this,lastObject);
				lastObject = null;
				popupOpen = true;
				option = -1;
			}
		}

		public void SetRatios(float rx, float ry){
			xratio = rx;
			zratio = ry;
		}

		public float[] GetRatios(){
			return new float[] {xratio,zratio};
		}

		public void Info(){
			string tag = hit.transform.gameObject.tag;
			float x,z;
			x = hit.point.x;
			z = hit.point.z;
			if (Input.GetKeyDown (KeyCode.Mouse1)) {
				foreach (Edge e in edges) {
					if (!e.isVertical) {
						if (e.gameObject.GetComponent<BoxCollider> () == null) 
							e.gameObject.AddComponent<BoxCollider> ();
						e.gameObject.GetComponent<BoxCollider> ().size = new Vector3 (1, 0, 1);
					}
				}
			}
			if (Input.GetKey (KeyCode.Mouse1) && tag == "line" && !popupOpen) {
				option = -1;
				PopupInfo.CreateInfoBox(popupInfo,this,edges,hit.transform.gameObject);
				popupOpen = true;
				foreach (Edge e in edges) {
					if (!e.isVertical) {
						Destroy (e.gameObject.GetComponent<BoxCollider> ());
					}
				}
			}
		}

		// Funcao para rotacionar o objeto em 90 graos.
		public void Rotate(){
			string tag = hit.transform.gameObject.tag;
			GameObject go = hit.transform.gameObject;
			if (Input.GetKeyDown (KeyCode.R) && Node.isNode(tag)) {
				go.transform.Rotate (new Vector3 (0F, 0F, 90F));
			}
		}
						
		public void GetTubulationSize(){
			float result = 0;
			float resultrw = 0;
			/*foreach (Edge e in edges) {
				if (e.gameObject != null) {
					LineRenderer lr = e.edge.GetComponent<LineRenderer> ();
					Vector3 reworkedA = new Vector3 (lr.GetPosition (0).x * (1 / xratio),
						                   lr.GetPosition (0).y, lr.GetPosition (0).z * (1 / zratio));
					Vector3 reworkedB = new Vector3 (lr.GetPosition (1).x * (1 / xratio),
						                   lr.GetPosition (1).y, lr.GetPosition (1).z * (1 / zratio));
					result += Vector3.Distance (lr.GetPosition (0), lr.GetPosition (1));
					resultrw += Vector3.Distance (reworkedA, reworkedB);
				}
			}*/
		}


		public Edge HasVerticalEdge(GameObject node){
			Edge[] es = new Edge[edges.Count];
			edges.CopyTo (es, 0);
			for (int i = 0; i < es.Length; i++) {
				if (es [i].inv.Equals (node) && es[i].isVertical) {
					return es[i];
				}
			}
			return null;
		}

		public void SetOption(int op){
			option = op;
		}

		public void SetHeight(float h){
			height = h;
		}

		public int GetOption(){ 
			return option;
		}
		public float GetHeight(){ 
			return height;
		}
		public void InsertOnNodes(Node n){
			nodes.AddLast (n);
		}
		public void InsertOnEdges(Edge e){
			edges.AddLast (e);
		}
		public void DestroyThisErrorEdge(GameObject g){
			Node.DestroyEdgeInNodes (nodes, g);
		}
	}
}
