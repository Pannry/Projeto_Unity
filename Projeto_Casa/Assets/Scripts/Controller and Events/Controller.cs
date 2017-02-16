using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp{
	public class Controller : MonoBehaviour {
		/// <summary>
		/// The popup open.
		/// Define se foi aberta alguma popup. Todo script que abrir uma janela e quiser fechar outra janela principal
		/// defe redefinir esta variavel.
		/// </summary>
		public bool popupOpen;
		public GameObject popupRatio;
		public GameObject popupInfo;
		private float xratio, zratio;
		Ray ray;
		RaycastHit hit;
		/// <summary>
		/// The height.
		/// Define a altura dos proximos objetos a serem criados.
		/// </summary>
		public float height;
		/// <summary>
		/// The prefab.
		/// Array de objetos selecionados na ferramenta Unity. Esses objetos vao gerar nos,arestas,paredes,etc.
		/// </summary>
		public GameObject[] prefab;
		private GameObject lastObject;
		/// <summary>
		/// The option.
		/// Esse atributo tambem e utilizado para calcular a posicao do objeto no array prefab.
		/// Sempre que usa-lo coloque o elemento prefab de acordo com sua respectiva opcao - 1.
		/// </summary>
		private int option = 0;
		private LinkedList<Edge> edges;
		private LinkedList<Node> nodes;

		void Start () {
			xratio = 0;
			zratio = 0;
			nodes = new LinkedList<Node> ();
			edges = new LinkedList<Edge> ();
			popupOpen = false;
		}

		/// <summary>
		/// Update this instance.
		/// Constantemente são chamados alguns métodos de controle para garantir o funcionamento correto do programa.
		/// </summary>
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
				ConduitInfo ();
				GarbageCollector ();
				//Debug.Log (":edges::" + edges.Count);
				//Debug.Log (":nodes::" + nodes.Count);
				GetTubulationSize ();
				Erase ();
			
			}	
		}

		/// <summary>
		/// Erase this instance.
		/// Apaga um determinado objeto.
		/// </summary>
		public void Erase(){
			string tag = hit.transform.gameObject.tag;
			if(Input.GetButtonDown("Fire1") && option == 99 && tag != Tags.SemTag()
				&& tag != Tags.Planta() ){
				Destroy (hit.transform.gameObject);
				//Node.DestroyNode (nodes,hit.transform.gameObject);
			}
		}

		/// <summary>
		/// Faz uma coleta de lixo, caso tenha algum nó ou aresta pendurado.
		/// Também garante a consistencia da rede, e finaliza as remoções.
		/// </summary>
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
						foreach (Conductor c in array[i].content) {
							if (c.GetGameObject () != null)
								Destroy (c.GetGameObject ());
							if (c.GetLabel() != null)
								Destroy (c.GetLabel());
						}
						Destroy (array [i].gameObject);
					} else {
						edges.AddLast (array [i]);
					}
				}
			}
		}

		/// <summary>
		/// Sets the ratio.
		/// Seleciona uma área e gera uma popup.
		/// Ali o usuário poderá informar qual o tamanho real daquela área.
		/// </summary>
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
		/// <summary>
		/// O usuário pode obter as informações do eletroduto, assim como adicionar novas informações.
		/// </summary>
		public void ConduitInfo(){
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

		/// <summary>
		/// Rotate. Simplesmente rotaciona o objeto em 90 graus.
		/// </summary>
		public void Rotate(){
			string tag = hit.transform.gameObject.tag;
			GameObject go = hit.transform.gameObject;
			if (Input.GetKeyDown (KeyCode.R) && Node.isNode(tag)) {
				go.transform.Rotate (new Vector3 (0F, 0F, 90F));
			}
		}

		/// <summary>
		/// Gets the size of the tubulation.
		/// </summary>
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

		/// <summary>
		/// Determines whether this node has a vertical edge.
		/// </summary>
		/// <returns> <c>Object</c> if this instance has vertical edge the specified node; otherwise, <c>null</c>.</returns>
		/// <param name="node">Node.</param>
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
		/// <summary>
		///  Determines whether this node has a vertical edge.
		///  A diferença é que esse método discerne entre um eletroduto que vai pra laje, ou o que vai para o chão.
		/// </summary>
		/// <returns> <c>Object</c> if this instance has vertical edge the specified node; otherwise, <c>null</c>.</returns>
		/// <param name="node">Node.</param>
		/// <param name="isDown">If set to <c>true</c> irá retornar um eletroduto que vai para o chão.</param>
		public Edge HasVerticalEdge(GameObject node, bool isDown){
			Edge[] es = new Edge[edges.Count];
			edges.CopyTo (es, 0);
			for (int i = 0; i < es.Length; i++) {
				if (es [i].inv.Equals (node) && es[i].isVertical && es[i].isDown == isDown) {
					return es[i];
				}
			}
			return null;
		}

		public void HandleConduitSelection(Vector2 _box_start_pos, Vector2 _box_end_pos){
			Vector3 a = Camera.main.ScreenToWorldPoint((Vector3)_box_start_pos);
			Vector3 b = Camera.main.ScreenToWorldPoint((Vector3)_box_end_pos);

			Debug.Log ("Start Positon: "+a + " End Position: "+b);
			ArrayList selectedEdges = new ArrayList ();
			foreach (Edge e in edges) {
				if (!e.isVertical) {
					if (a.x > b.x && e.gameObject.transform.position.x < a.x) {
						if (a.z > b.z && e.gameObject.transform.position.z < a.z) {
							if (e.gameObject.transform.position.x > b.x && e.gameObject.transform.position.z > b.z)
								selectedEdges.Add (e);
						} else if (a.z < b.z && e.gameObject.transform.position.z < b.z) {
							if (e.gameObject.transform.position.x > b.x && e.gameObject.transform.position.z > a.z)
								selectedEdges.Add (e);
						}
					} else if (a.x < b.x && e.gameObject.transform.position.x < b.x) {
						if (a.z > b.z && e.gameObject.transform.position.z < a.z) {
							if (e.gameObject.transform.position.x > a.x && e.gameObject.transform.position.z > b.z)
								selectedEdges.Add (e);
						} else if (a.z < b.z && e.gameObject.transform.position.z < b.z) {
							if (e.gameObject.transform.position.x > a.x && e.gameObject.transform.position.z > a.z)
								selectedEdges.Add (e);
						}
					}
				}
			}
			if (!popupOpen) {
				PopupInfo.CreateMultiEdgesInfoBox (popupInfo, this, edges, selectedEdges);
				popupOpen = true;
			}
			Debug.Log ("Selected Edges: " + selectedEdges.Count);
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
		/// <summary>
		/// Destroies a edge with a error.
		/// </summary>
		/// <param name="g">The <c>GameObject</c> of a edge component.</param>
		public void DestroyThisErrorEdge(GameObject g){
			Node.DestroyEdgeInNodes (nodes, g);
		}
	}
}
