using UnityEngine;
using System.Collections;

public class Signal : MonoBehaviour {

	public int Intensity;
	public Vector3 Position{
		set{Position = value;}
		get{return transform.localPosition;}
	}
	public sightModality modality; 
	public float maximumRange;


	public void Awake(){
		modality = sightModality.Instance;
	}

}
