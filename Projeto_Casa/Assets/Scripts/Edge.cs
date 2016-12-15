using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AssemblyCSharp
{
public class Edge : MonoBehaviour
{
	public GameObject inv,outv;
	public GameObject edge;
	public float height;
	public bool isVertical = false;

	public Edge (GameObject e, GameObject v1, GameObject v2)
	{
		edge = e;
		inv = v1;
		outv = v2;
		
	}
}
}


