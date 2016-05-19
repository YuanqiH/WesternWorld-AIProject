using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public struct gameLocationInfo{
	public Transform HomeAdd;
	public Transform MineAdd;
}

public class gameManager : MonoBehaviour {

	public static gameManager instance = null; // accessable for other classes
	private boardManager boardScript;
	public int level = 3;
	public gameLocationInfo gameInfo = new gameLocationInfo();
	//run this before the start of the game
	void Awake(){
	
		if (instance == null)
			instance = this;
		//if the instance already exist but not this
		else if (instance != this)
			// destroy this gameobject, make sure that there is only one gameManager existing
			Destroy (gameObject);

		//DontDestroyOnLoad (gameObject);

		boardScript = GetComponent<boardManager> ();

		InitGame ();

	}

	void InitGame(){
		// using the setup scene in boardscript, passin the level
		boardScript.setupScene (level);

	}

	void Update(){

	}


}
