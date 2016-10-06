using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClickEvent : MonoBehaviour {

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
	// Atributo que indica a opcao da ferramenta que esta sendo utilizada.
	// Esse atributo tambem e utilizado para calcular a posicao do objeto no array prefab.
	private int option = 0; // Coloque o elemento prefab de acordo com sua respectiva opcao - 1
	// Aqui eh um array de 2 celulas. Esse array eh utilizado para guardar posicoes dos indicadores
	// da parede, e criar uma nova parede.
	private GameObject[] squares;
	// Aqui eh uma lista de nos, futuramente vai ser um grafo.
	private LinkedList<Node> nodes;
	// Construtor, seta o square como um array de 2 celulas e inicializa a lista de nos.
	void Start () {
		squares = new GameObject[2];
		squares [0] = null;
		squares [1] = null;
		nodes = new LinkedList<Node> ();
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
			CreateNode (1);
			CreateNode (4);
			CreateWall ();
			CreateLine ();
			// Se a opcao de delecao estiver ativa, deleta-se o elemento que o raio colidir.
			// Note que o elemento sera deletado desde que ele tenha uma tag(Ou seja, nao seja o menu)
			// diferente de Untagged e planta.
			string tag = hit.transform.gameObject.tag;
			if(Input.GetButtonDown("Fire1") && option == 99 && tag != Tags.SemTag()
				&& tag != Tags.Planta() ){
				Destroy (hit.transform.gameObject);
			}
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
			nodes.AddLast (new Node (obj, obj.tag));
		}
	}

	// Cria a parede.
	public void CreateWall(){
		// Se o raio estiver colidindo com a planta, cria um marcador.
		if(Input.GetButtonDown("Fire1") && option == 2 && hit.transform.gameObject.tag == Tags.Planta() ){ 
			lastObject = Instantiate(prefab[option -1],new Vector3(hit.point.x,0,hit.point.z), Quaternion.identity) as GameObject;
			Transform tr = lastObject.transform;
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
		string tag = hit.transform.gameObject.tag;
		//GetButton para drag, Se a tag do meu objeto for nao nula:
		if(Input.GetButton("Fire1") && option == 3 && tag != Tags.SemTag()){ 
			// Para ser click and click, troque esse evento por true. Desse jeito é click and drag[ACIMA].
			// Quando eu começar a precionar o mouse, tambem sera interpretado um click, e se eu estiver
			// criando uma linha a partir de uma lampada a aresta sera aceita e podera ser arrastada
			// atravez do proximo if.
			if(Input.GetButtonDown("Fire1")&& tag == Tags.EmbutidaLaje()){
				// Note que eh criado um objeto a partir de um prefab.
				lastObject = Instantiate(prefab[option -1],new Vector3(hit.point.x,height,hit.point.z), Quaternion.identity) as GameObject;
				// Depois eu pego o renderizador da linha.
				LineRenderer lr = lastObject.GetComponent<LineRenderer>();
				lr.SetWidth(0.05F,0.05F);
				// E digo que a linha sera formada por 2 vertices.
				lr.SetVertexCount(2);
				// O primeiro vertice eh a posicao onde esse click foi identificado.
				lr.SetPosition(0,new Vector3(hit.point.x,height,hit.point.z));					
			}
			// Arrasta o ultimo objeto criado.
			if(lastObject != null){
				// Pego o renderizador de linha do ultimo objeto criado e mexo somente o ultimo vertice.
				// Isso da todo o efeito de drag.
				LineRenderer lr = lastObject.GetComponent<LineRenderer>();
				lr.SetPosition(1,new Vector3(hit.point.x,height,hit.point.z));

			}
		}
		if(Input.GetButtonUp("Fire1") && lastObject != null && option == 3){
			// Se a tag não for de nenhum nó, destrua...
			// Se a linha for solta em algum objeto que nao seja no, sera destruida.
			if (!Node.isNode(tag)) {
				Destroy (lastObject);
			}
			// Aqui, se o no em que a aresta foi solta for um quadro eletrico, eu crio outra aresta na vertical.
			if (tag == Tags.QuadroEletrico()) {
				GameObject obj = Instantiate(prefab[option -1],new Vector3(hit.point.x,height - 0.1F,hit.point.z), Quaternion.identity) as GameObject;
				LineRenderer r = obj.GetComponent<LineRenderer>();
				r.SetWidth(0.05F,0.05F);
				r.SetVertexCount(2);
				r.SetPosition(0,new Vector3(hit.point.x,height,hit.point.z));
				// A altura escolhida aqui eh exatamente a altura do quadro eletrico.
				r.SetPosition(1,new Vector3(hit.point.x,1.5F,hit.point.z));
			}
		}
	}

	// Metodo para botao setar opcao
	public void SetNewLine(){
		option = 3;
		height = 2.8F;
	}
	// Metodo para botao setar opcao
	public void SetNewEL(){
		option = 1;
		height = 2.8F;
	}
	// Metodo para botao setar opcao
	public void SetNewWall(){
		option = 2;
		height = 2.5F;
	}
	// Metodo para botao setar opcao
	public void SetDestroy(){
		option = 99;
	}

	//TODO falta criar metodo para o botao!!! E para movimento !! 


	// Metodo para botao setar opcao
	public void SetNewQE(){
		option = 4;
		height = 1.5F;

	}
}
