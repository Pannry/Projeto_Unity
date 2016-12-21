
	using UnityEngine;
#if UNITY_EDITOR
	using UnityEditor;
#endif
	using System.Collections;
	using System.Collections.Generic;

	namespace AssemblyCSharp{
	public class ClickEvent : MonoBehaviour {
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
		// Utilizado para criacao de arestas.
		private GameObject lastNode; 
		private Edge tempEdge;
		// Atributo que indicreca a opcao da ferramenta que esta sendo utilizada.
		// Esse atributo tambem e utilizado para calcular a posicao do objeto no array prefab.
		private int option = 0; // Coloque o elemento prefab de acordo com sua respectiva opcao - 1
		// Aqui eh um array de 2 celulas. Esse array eh utilizado para guardar posicoes dos indicadores
		// da parede, e criar uma nova parede.
		private GameObject[] squares;
		// Aqui eh uma lista de nos, futuramente vai ser um grafo.
		private LinkedList<Edge> edges;
		private LinkedList<Node> nodes;
		private int totalNodes = 0;
		// Construtor, seta o square como um array de 2 celulas e inicializa a lista de nos.
		void Start () {
			squares = new GameObject[2];
			squares [0] = null;
			squares [1] = null;
			nodes = new LinkedList<Node> ();
			edges = new LinkedList<Edge> ();
			//nodes = GetComponentInParent<GameObject> ().AddComponent <LinkedList<Node>>() as LinkedList<Node>; 
		}

		// Funcao update que eh chamada a cada frame.
		void Update () {
			// Defino meu raio a partir da posicao do mouse.
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			// Note que esse if acontece para qualquer colisao. A colisao fica em hit.
			if(Physics.Raycast(ray,out hit))
			{
				// Se for clicado no nada, redefine o ultimo objeto selecionado para null.
				if (Input.GetButtonDown ("Fire1") && hit.transform.gameObject.tag == "Untagged")
					lastObject = null;
				// Chamada de opcoes.
				Rotate ();
				for (int i = 1; i <= prefab.Length; i++)
					CreateNode (i);
				SetConfigRatio ();
				CreateWall ();
				CreateLine ();
				Info ();
				Move ();
				GetTubulationSize ();
				// Se a opcao de delecao estiver ativa, deleta-se o elemento que o raio colidir.
				// Note que o elemento sera deletado desde que ele tenha uma tag(Ou seja, nao seja o menu)
				// diferente de Untagged e planta.
				string tag = hit.transform.gameObject.tag;
				if(Input.GetButtonDown("Fire1") && option == 99 && tag != Tags.SemTag()
					&& tag != Tags.Planta() ){
					Node.DestroyNode (nodes,hit.transform.gameObject);
					Destroy (hit.transform.gameObject);
				}
			}	
		}
		//Definir a proporção da planta.
		public void SetConfigRatio(){
			string tag = hit.transform.gameObject.tag;
			float x,z;
			x = hit.point.x;
			z = hit.point.z;
			if (Input.GetKey(KeyCode.S)) {
				SetNewLine ();
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
			if (Input.GetKeyUp (KeyCode.S)) {
				string[] vec = { tag };
				#if UNITY_EDITOR
				new RatioPopup (vec, this, lastObject);
				#endif
				lastObject = null;
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
					if(e.edge.GetComponent<BoxCollider>() == null)
						e.edge.AddComponent<BoxCollider> ();
				}
			}
			if (Input.GetKey (KeyCode.Mouse1) && tag == "line") {
				#if UNITY_EDITOR
				new InfoPopup (edges,hit.transform.gameObject);
				#endif
			}
		}

		// Funcao para rotacionar o objeto em 90 graos.
		public void Rotate(){
			string tag = hit.transform.gameObject.tag;
			GameObject go = hit.transform.gameObject;
			// Dado meu objeto, se ele for um no, eu rotaciono.
			if (Input.GetKeyDown (KeyCode.R) && Node.isNode(tag)) {
				go.transform.Rotate (new Vector3 (0F, 0F, 90F));
			}
		}

		// Cria um determinado no, recebe como parametro o indice(Numero da opcao) equivalente ao objeto no array.
		// Opcao vai de 1 a 4. A correcao ja eh feita, entao um elemento 4 no array nao chama esse metodo como
		// CreateNode(3), mas CreateNode(4).
		public void CreateNode(int index){
			string tag = hit.transform.gameObject.tag;
			// Se o raio estiver colidindo com a planta, cria o objeto a uma certa altura da planta.
			if(Input.GetButtonDown("Fire1") && option == index && tag == Tags.Planta() ){
				GameObject obj=Instantiate(prefab[option - 1],new Vector3(hit.point.x,height,hit.point.z), Quaternion.identity) as GameObject;
				obj.transform.Rotate(new Vector3(90F,0F,0F));
				Debug.Log (obj.name);
				nodes.AddLast (new Node (obj, obj.tag,obj.name));
				totalNodes++;
			}
		}

		// Cria a parede.
		public void CreateWall(){
			// Se o raio estiver colidindo com a planta, cria um marcador.
			if(Input.GetButtonDown("Fire1") && option == 2 && hit.transform.gameObject.tag == Tags.Planta() ){ 
				lastObject = Instantiate(prefab[option -1],new Vector3(hit.point.x,0,hit.point.z), Quaternion.identity) as GameObject;
				Debug.Log ("Objeto instanciado -- " + lastObject);
				// Se o primeiro marcador nao estiver criado, o segundo tambem nao foi criado, logo nao ha marcas
				// entao cria-se um marcador na primeira celula. Vai pro else no proximo marcador criado.
				if (squares [0] == null)
					squares [0] = lastObject;
				else {
					squares [1] = lastObject;
					// Guardo a posicao inicial do primeiro marcador.
					Vector3 pos = squares [0].transform.position;
					// Calculo o ponto medio, e coloco o primeiro marcador la.
					squares [0].transform.position = (pos + squares [1].transform.position) / 2;
					// Coloco o marcador a metade da altura que sera escalonado, para que a parede
					// fique exatamente do tamanho que eu quero, e tocando no chao.
					squares [0].transform.position += new Vector3 (0, height/2, 0);
					// Calculo quanto eu devo escalonar em cada eixo.
					float scaleX = (pos.x - squares [1].transform.position.x);
					float scaleY = (pos.y - squares [1].transform.position.y);
					float scaleZ = (pos.z - squares [1].transform.position.z);
					// Se o numero estiver negativo eu transformo.
					if(scaleX < 0)
						scaleX = scaleX*-1;
					if(scaleY < 0)
						scaleY = scaleY*-1;
					if(scaleZ < 0)
						scaleZ = scaleZ*-1;

					// Finalmente eu escalono o objeto.
					squares [0].transform.localScale += new Vector3 (scaleX, height, scaleZ);
					// Reseta os marcadores e deleta a parede(marcador2) nao utilizada.
					squares [0] = null;
					Destroy (squares [1]);
					squares [1] = null;
				}
			}

		}

		// Cria um eletroduto(linha).
		public void CreateLine(){
			// Pego as coordenadas do objeto em que o raio colide, então eu vou ter os vertices das
			// linhas centralizados no node.
			GameObject obj = hit.transform.gameObject;
			string tag = hit.transform.gameObject.tag;
			float x, z, currentHeight;
			x = obj.transform.position.x;
			currentHeight = obj.transform.position.y;
			z = obj.transform.position.z;
			//GetButton para drag, Se a tag do meu objeto for nao nula:
			if (Input.GetButton ("Fire1") && option == 3 && tag != Tags.SemTag ()) { 
				// Para ser click and click, troque esse evento por true. Desse jeito é click and drag[ACIMA].
				// Quando eu começar a precionar o mouse, tambem sera interpretado um click, e se eu estiver
				// criando uma linha a partir de uma lampada a aresta sera aceita e podera ser arrastada
				// atravez do proximo if.
				if (Input.GetButtonDown ("Fire1") && Node.isNode (tag)) {
					// Pegue a referência e a altura do primeiro objeto criado.
					height = currentHeight;
					lastNode = obj;
					// Note que eh criado um objeto a partir de um prefab.
					lastObject = Instantiate (prefab [option - 1], new Vector3 (x, height, z), Quaternion.identity) as GameObject;
					// Depois eu pego o renderizador da linha.
					LineRenderer lr = lastObject.GetComponent<LineRenderer> ();
					lr.SetWidth (0.1F, 0.1F);
					// E digo que a linha sera formada por 2 vertices.
					lr.SetVertexCount (2);
					// O primeiro vertice eh a posicao onde esse click foi identificado.
					lr.SetPosition (0, new Vector3 (x, height, z));
					tempEdge = new Edge (lastObject, obj, null);
				}
				// Arrasta o ultimo objeto criado.
				if (lastObject != null) {
					// Pego o renderizador de linha do ultimo objeto criado e mexo somente o ultimo vertice.
					// Isso da todo o efeito de drag.
					LineRenderer lr = lastObject.GetComponent<LineRenderer> ();
					lr.SetPosition (1, new Vector3 (hit.point.x, height, hit.point.z));

				}
			}
			if (Input.GetButtonUp ("Fire1") && lastObject != null && option == 3) {
				// Se a tag não for de nenhum nó, destrua...
				// Se a linha for solta em algum objeto que nao seja no, sera destruida.
				if (!Node.isNode (tag)) {
					Node.SearchNodeAndDestroyEdge (nodes, lastObject);
					Destroy (lastObject);
					tempEdge = null;
				} else {
					tempEdge = new Edge (tempEdge.edge, tempEdge.inv, obj);
					edges.AddLast (tempEdge);
					Node.SearchNodeAndAddEdge (nodes, lastNode, tempEdge, height);
					Node.SearchNodeAndAddEdge (nodes, obj, tempEdge, height);
					LineRenderer lr = lastObject.GetComponent<LineRenderer> ();
					lr.SetPosition (1, new Vector3 (x, height, z));
					// Aqui, se o no em que a aresta foi solta for um quadro eletrico, eu crio outra aresta na vertical.
					if (height > currentHeight) {
						GameObject linhaVertical = Instantiate (prefab [option - 1], new Vector3 (hit.point.x, height, hit.point.z), Quaternion.identity) as GameObject;
						LineRenderer r = linhaVertical.GetComponent<LineRenderer> ();
						r.SetWidth (0.1F, 0.1F);
						r.SetVertexCount (2);
						r.SetPosition (0, new Vector3 (x, height, z));
						// A altura escolhida aqui eh exatamente a altura do no baixo.
						r.SetPosition (1, new Vector3 (x, currentHeight, z));
						// Adiciona-se os 3 vertices para mover perpendicularmente ao node.
						tempEdge = new Edge (linhaVertical, lastNode, obj);
						tempEdge.isVertical = true;
						edges.AddLast (tempEdge);
						Node.SearchNodeAndAddEdge (nodes, lastNode, tempEdge, currentHeight);
						Node.SearchNodeAndAddEdge (nodes, obj, tempEdge, currentHeight);
						lastNode = null;
						tempEdge = null;
						lastObject = null;

					} else if (height < currentHeight) {
						GameObject linhaVertical = Instantiate (prefab [option - 1], new Vector3 (hit.point.x, height, hit.point.z), Quaternion.identity) as GameObject;
						LineRenderer r = linhaVertical.GetComponent<LineRenderer> ();
						float lastX, lastZ;
						lastX = lastNode.transform.position.x;
						lastZ = lastNode.transform.position.z;
						lr.SetPosition (0, new Vector3 (lastX, currentHeight, lastZ));
						lr.SetPosition (1, new Vector3 (x, currentHeight, z));
						r.SetWidth (0.1F, 0.1F);
						r.SetVertexCount (2);
						r.SetPosition (0, new Vector3 (lastX, height, lastZ));
						// A altura escolhida aqui eh exatamente a altura do quadro eletrico.
						r.SetPosition (1, new Vector3 (lastX, currentHeight, lastZ));
						// Adiciona-se os 3 vertices para mover perpendicularmente ao node.
						tempEdge = new Edge (linhaVertical, lastNode, obj); 
						tempEdge.isVertical = true;
						edges.AddLast (tempEdge);
						Node.SearchNodeAndAddEdge (nodes, lastNode, tempEdge, height); //sempre a altura mais baixa eh salva
						Node.SearchNodeAndAddEdge (nodes, obj, tempEdge, height);
						lastNode = null;
						tempEdge = null;
						lastObject = null;
					}
				}
			}
		}

		private void MoveEdges(){
			Node aux;
			foreach (Node n in nodes) {
				int count = 0;
				if (n.Compare (lastObject)) {
					foreach (Edge e in n.GetEdges()) {
						LineRenderer lr = e.edge.GetComponent<LineRenderer> ();
						//Se Reta horizontal...
						if (!e.isVertical) {
							if (lastObject.Equals (e.inv)) {
								lr.SetPosition (0, new Vector3 (lastObject.transform.position.x, lr.GetPosition (0).y,
									lastObject.transform.position.z));
							} else if (lastObject.Equals (e.outv)) {
								lr.SetPosition (1, new Vector3 (lastObject.transform.position.x, lr.GetPosition (1).y,
									lastObject.transform.position.z));
							}
						} else {
							//So move aresta vertical se estiver fazendo o mov apartir do node inferior.
							if (lastObject.transform.position.y == e.height) {
								Debug.Log ("Falta Mover Parte inferior...");
								lr.SetPosition (0, new Vector3 (lastObject.transform.position.x, lr.GetPosition (0).y,
									lastObject.transform.position.z));
								lr.SetPosition (1, new Vector3 (lastObject.transform.position.x, lr.GetPosition (1).y,
									lastObject.transform.position.z));
							
							}
							
						}
							
					}
				}
			}
		}

		public void Move(){
			// Pega a tag do objeto que o raio colidir.
			string tag = hit.transform.gameObject.tag;
			// Se esse objeto for um node eu vou move-lo.
			if (Input.GetButton ("Fire1") && option == 98 && Node.isNode(tag)) {
				// A partir do momento em que se segura o botao do mause, e detectado um click, e eu pego aquele node
				if (Input.GetButtonDown ("Fire1") && Node.isNode(tag)) {
					lastObject = hit.transform.gameObject;
				}
				// Fico mudando a posicao do node.
				if (lastObject != null && Node.isNode (lastObject.tag)) {
					MoveEdges ();
					lastObject.transform.position = new Vector3 (hit.point.x,lastObject.transform.position.y,hit.point.z);
				}
				// Quando soltar o botao o node vai parar de se mover, e eu tiro a referencia do lastobject.
				if (Input.GetButtonUp ("Fire1"))
					lastObject = null;
			}
		}
		public void GetTubulationSize(){
			float result = 0;
			float resultrw = 0;
			foreach (Edge e in edges) {
				LineRenderer lr = e.edge.GetComponent<LineRenderer> ();
				Vector3 reworkedA = new Vector3(lr.GetPosition (0).x * (1/xratio),
					lr.GetPosition (0).y,lr.GetPosition (0).z * (1/zratio));
				Vector3 reworkedB = new Vector3(lr.GetPosition (1).x * (1/xratio),
					lr.GetPosition (1).y,lr.GetPosition (1).z * (1/zratio));
				result += Vector3.Distance (lr.GetPosition (0), lr.GetPosition (1));
				resultrw+= Vector3.Distance (reworkedA, reworkedB);
			}
			Debug.Log ("Distancia total = " + result);
			Debug.Log ("Distancia total retrabalhada = " + resultrw);
		}

		// Metodo para botao setar opcao.
		public void SetNewLine(){
			option = 3;
		}
		// Metodo para botao setar opcao.
		public void SetNewEL(){
			option = 1;
			height = 2.8F;
		}
		// Metodo para botao setar opcao.
		public void SetNewWall(){
			option = 2;
			height = 2.5F;
		}
		// Metodo para botao setar opcao.
		public void SetDestroy(){
			option = 99;
		}

		// Metodo para botao setar opcao.
		public void SetMove(){
			option = 98;
		}

		// Metodo para botao setar opcao.
		public void SetNewQE(){
			option = 4;
			height = 1.5F;

		}

		// Metodo para botao setar opcao. Tomada Baixa
		public void SetNewTB(){
			option = 5;
			height = 0.30F;	
		}
		//Tomada baixa universal.
		public void SetNewTBU(){
			option = 6;
			height = 1.2F;
		}
		//Ponto Luz Parede.
		public void SetNewPLP(){
			option = 7;
			height = 2F;
		}
		//Tomada para chuveiro eletrico.
		public void SetNewCE(){
			option = 8;
			height = 2.2F;
		}
		//Interruptor 1 sessão
		public void SetNewIUS(){
			option = 9;
			height = 1.2F;			
		}
		//Interruptor 3 sessões
		public void SetNewITS(){
			option = 10;
			height = 1.2F;			
		}
		//Interruptor 2 sessões
		public void SetNewIDS(){
			option = 11;
			height = 1.2F;			
		}
		//Interruptor three way
		public void SetNewITW(){
			option = 12;
			height = 1.2F;			
		}
		//Haste Aterramento de Cobre
		public void SetNewHAC(){
			//TODO A Haste nao vai funcionar exatamente como um node comum.
		}
		//Pulsador Campainha
		public void SetNewPC(){
			option = 13;
			height = 1.2F;	
		}
		//Campainha Musical
		public void SetNewCM(){
			option = 14;
			height = 2.5F;	
		}
	}
	}
