using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AssemblyCSharp{
    public class JanelaDeErroView : MonoBehaviour {

        private Execute funcaoOK;
        private Execute funcaoSim;
        private Execute funcaoNao;

        // Use this for initialization
        void Start () {
		
	    }
	
	    // Update is called once per frame
	    void Update () {
		
	    }

        public void EventClickButtonOK()
        {
			if (this.funcaoOK != null) funcaoOK.Execute ();
			Destroy (GameObject.Find ("JanelaDeErro(Clone)"));
        }

        public void EventClickButtonSim()
        {
            if (this.funcaoSim != null) funcaoSim.Execute();
			Destroy (GameObject.Find ("JanelaDeErro(Clone)"));
        }

        public void EventClickButtonNao()
        {
            if (this.funcaoNao != null) funcaoNao.Execute();
			Destroy (GameObject.Find ("JanelaDeErro(Clone)"));
        }

        public Execute FuncaoOK
        {
            get
            {
                return funcaoOK;
            }

            set
            {
                funcaoOK = value;
            }
        }

        public Execute FuncaoSim
        {
            get
            {
                return funcaoSim;
            }

            set
            {
                funcaoSim = value;
            }
        }

        public Execute FuncaoNao
        {
            get
            {
                return funcaoNao;
            }

            set
            {
                funcaoNao = value;
            }
        }

    }
}