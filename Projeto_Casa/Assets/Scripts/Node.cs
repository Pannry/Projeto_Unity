using System;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
	private GameObject objeto;
	private string rotulo;
	public Node (GameObject objeto, string rotulo)
	{
		this.objeto = objeto;
		this.rotulo = rotulo;
	}
	public GameObject GetObject(){
		return objeto;
	}
	public string GetLabel(){
		return rotulo;
	}
	public bool Compare(GameObject outro){
		return outro.Equals (objeto);
	}
	public static bool isNode(string tag){
		return (tag == Tags.EmbutidaLaje() || tag == Tags.QuadroEletrico());
	}
}


