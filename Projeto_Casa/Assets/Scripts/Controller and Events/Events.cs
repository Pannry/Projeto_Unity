using System;
using UnityEngine;

namespace AssemblyCSharp
{
	/// <summary>
	/// Events.
	/// Todos são eventos chamados por botões.
	/// Determinam qual prefab será usado.
	/// </summary>
	public class Events : MonoBehaviour
	{
		private Controller myController;

		void Start(){
			myController = GetComponent<Controller> ();
		}

		public void SetSelectionBox(){
			myController.SetOption (17);
		}

		public void SetNewLine()
		{
			myController.SetOption (16);
		}
		public void SetNewLineDown()
		{
			myController.SetOption (15);
		}
		public void SetNewLineTop()
		{
			myController.SetOption (3);
		}
		// Metodo para botao setar opcao.
		public void SetNewEL()
		{
			myController.SetOption (1);
			myController.SetHeight( 2.8F);
		}
		// Metodo para botao setar opcao.
		public void SetNewWall()
		{
			myController.SetOption (2);
			myController.SetHeight (2.5F);
		}
		// Metodo para botao setar opcao.
		public void SetDestroy()
		{
			myController.SetOption (99);
		}

		// Metodo para botao setar opcao.
		public void SetMove()
		{
			myController.SetOption (98);
		}

		// Metodo para botao setar opcao.
		public void SetNewQE()
		{
			myController.SetOption (4);
			myController.SetHeight (1.5F);

		}

		// Metodo para botao setar opcao. Tomada Baixa
		public void SetNewTB()
		{
			myController.SetOption (5);
			myController.SetHeight (.3F);
		}
		//Tomada baixa universal.
		public void SetNewTBU()
		{
			myController.SetOption (6);
			myController.SetHeight (1.2F);
		}
		//Ponto Luz Parede.
		public void SetNewPLP()
		{
			myController.SetOption (7);
			myController.SetHeight (2F);
		}
		//Tomada para chuveiro eletrico.
		public void SetNewCE()
		{
			myController.SetOption (8);
			myController.SetHeight (2.2F);
		}
		//Interruptor 1 sessão
		public void SetNewIUS()
		{
			myController.SetOption (9);
			myController.SetHeight (1.2F);
		}
		//Interruptor 3 sessões
		public void SetNewITS()
		{
			myController.SetOption (10);
			myController.SetHeight (1.2F);
		}
		//Interruptor 2 sessões
		public void SetNewIDS()
		{
			myController.SetOption (11);
			myController.SetHeight (1.2F);
		}
		//Interruptor three way
		public void SetNewITW()
		{
			myController.SetOption (12);
			myController.SetHeight (1.2F);
		}
		//Haste Aterramento de Cobre
		public void SetNewHAC()
		{
			//TODO A Haste nao vai funcionar exatamente como um node comum.
		}
		//Pulsador Campainha
		public void SetNewPC()
		{
			myController.SetOption (13);
			myController.SetHeight (1.2F);
		}
		//Campainha Musical
		public void SetNewCM()
		{
			myController.SetOption (14);
			myController.SetHeight (2.5F);
		}
	}
}

