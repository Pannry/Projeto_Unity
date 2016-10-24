using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Edge : MonoBehaviour
{
	public int vertex;
	public GameObject edge;
	public Edge (GameObject e, int v)
	{
		edge = e;
		vertex = v;
	}
}


