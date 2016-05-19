using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;




public class boardManager : MonoBehaviour {

	// Using Serializable allows us to embed a class with sub properties in the inspector.
	[Serializable]
	public class Count{
		public int maximal;
		public int minimal;

		public Count(int min, int max){
			maximal = max;
			minimal = min;
		}
	}
	
	public int columns = 8; //size of the boardmap
	public int rows = 8;
	public Count sandCount = new Count (2, 4); // random nember of geometries
	public Count riverCount = new Count(2,5);
	public Count swampCount = new Count(2,5);
	public Count iceCount =  new Count( 3,8);

	public GameObject locationMine;	//location of western world
	public GameObject locationShack; 
	public GameObject[] Background; //background of map
	public GameObject[] outwallTile; // array of prefabs for map
	public GameObject[] sandTile;
	public GameObject[] riverTile;
	public GameObject[] swampTile;
	public GameObject[] iceTile;



	private Transform boardHodler;   //Clears our list gridPositions and prepares it to generate a new board. boardHodler; //A variable to store a reference to the transform of our Board object.
	private List<Vector3> gridPositions = new List<Vector3>(); // a list of locations possible to put tile

	//Clears our list gridPositions and prepares it to generate a new board.
	void initialiseList(){

		//clear our list
		gridPositions.Clear ();
		 // save all the path positions 
		for (int x=1; x< this.columns-1; x++) {
			for(int y=1; y<this.rows-1; y++){
				//add position to the list 
				gridPositions.Add(new Vector3(x,y, 0.0f));
			}
		}// end for 
	}

	//set up outwall and background for the map
	void boardSetup(){

		pathFinder.Instance.MapSize = new Vector3 (columns, rows, 0); // for path finder

		boardHodler = new GameObject ("Board").transform;

		for (int x = -1; x <  this.columns+ 1; x++) {

			for( int y = -1; y  < this.rows+1 ; y++){

				// create a new gameobject for background 
				GameObject toInstantiate = Background[Random.Range(0,Background.Length)]; 
				// set the boarder as walls 
				if(x == -1|| x ==  this.columns || y == -1|| y == this.rows){
					toInstantiate = outwallTile[Random.Range(0,outwallTile.Length)];
				}
				//instantiate a prefabs, set the position and cast it in gameobject
				Vector3 localLocation = new Vector3(x,y,0.0f);
				GameObject Instance = Instantiate(toInstantiate, localLocation, Quaternion.identity) as GameObject;
				// everytime save the new position and the instance
				pathFinder.Instance.tileDicts.Add(localLocation, Instance);
				//set the parent of the new map
				Instance.transform.SetParent(boardHodler);

			}
		}
	
	}

	// return a random position based on gridposition list
	Vector3 randomPosition(){
	
		int randomIndex = Random.Range (0, gridPositions.Count);

		Vector3 randomPosition = gridPositions [randomIndex];
		//remove the picked position 
		gridPositions.RemoveAt (randomIndex);

		return randomPosition;
	}


	// set a number of tile by user setting number of that tile
	void layoutObjectAtRandom(GameObject[] tileArray, int minimal, int maximal){
		
		int gameObjectCount = Random.Range (minimal, maximal+1);

		for (int i = 0; i < gameObjectCount; i++) {
			//get a random position
			Vector3 localPosition= randomPosition();
			//choose a tile from the array
			GameObject tileChosen =  tileArray[ Random.Range(0,tileArray.Length)];

			GameObject instance = Instantiate(tileChosen,localPosition,Quaternion.identity) as GameObject;
			// update the dictionary, copy the clone to the dictionary
			pathFinder.Instance.tileDicts[localPosition] = instance;
		}
	}

	public void setupScene(int level){
		{ // set the mapsize
			float levelColumn, levelRow;
			levelColumn = levelRow = Math.Max (10, Mathf.Pow (2, level));
			this.columns = (int)levelColumn;
			this.rows = (int)levelRow;
			pathFinder.Instance.MapSize = new Vector3(levelColumn,levelRow,0);
		}
		//create a outwall and foor
		boardSetup ();
		//reset the list of gridposition
		initialiseList ();

		// set all the possible geometries
		layoutObjectAtRandom (sandTile, sandCount.minimal, sandCount.maximal);
		layoutObjectAtRandom (swampTile, swampCount.minimal, swampCount.maximal);
		layoutObjectAtRandom (iceTile, iceCount.minimal, iceCount.maximal);
		layoutObjectAtRandom (riverTile, riverCount.minimal, riverCount.maximal);

		//set the mine at particular position
		GameObject instance = Instantiate (locationMine, randomPosition(), Quaternion.identity) as GameObject;
		gameManager.instance.gameInfo.MineAdd = instance.transform;
		instance = Instantiate (locationShack, randomPosition(), Quaternion.identity) as GameObject;
		gameManager.instance.gameInfo.HomeAdd = instance.transform;

	}


}
