using AssemblyCSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JanelaDeErroController : MonoBehaviour{
    public GameObject janelaErro;

    void Start() { 
        
    }

    void Update() {

    }

    /**
     * <summary>
     * Cria uma janela de erro com um botão de Ok
     * </summary>
     * <param name="msg">Messagem que vai mostrar na janela de erro</param>
     * <param name="exe">Define uma função que vai ser executada ao clicar no botão de Ok</param>                              
     * */
    public void JanelaOk(string msg, Execute exe) {
        Instantiate(janelaErro);
        // Definindo a messagem de erro
        Text text = GameObject.Find("TextMenssagem").GetComponent<Text>();
        text.text = msg;

        // Definido a funcao que vai ser executada ao apertar o botão de ok
		JanelaDeErroView jder = GameObject.Find("JanelaDeErro(Clone)").GetComponent<JanelaDeErroView>();
        jder.FuncaoOK = exe;
    }
		

    /**
     * <sumary>
     * Cria uma jenalela de erro com os botões sim e ok
     * </sumary>
     * <param name="msg"> Messagem que vai mostrar na janela de erro</param>
     * <param name="fSim"> Define uma função que vai ser executada ao clicar no botão de Sim </param>
     * <param name="fNao"> Define uma função que vai ser executada ao clicar no botaão de Não</param>
     **/

    public void JanelaSimNao(string msg, Execute fSim, Execute fNao)
    {
        Instantiate(janelaErro);
        // Definindo a messagem de erro
        Text text = GameObject.Find("TextMenssagem").GetComponent<Text>();
        text.text = msg;

        JanelaDeErroView jder = GameObject.Find("JanelaDeErro").GetComponent<JanelaDeErroView>();
        // Definido a funcao que vai ser executada ao apertar o botão de Sim
        jder.FuncaoSim = fSim;
        // Definido a funcao que vai ser executada ao apertar o botão de Não
        jder.FuncaoNao = fNao;

        // Mostrar os botões e sim e não
        GameObject.Find("ButtonOk").SetActive(false);
        GameObject.Find("ButtonSim").SetActive(true);
        GameObject.Find("ButtonNao").SetActive(true);
        Destroy(janelaErro);
    }
}
