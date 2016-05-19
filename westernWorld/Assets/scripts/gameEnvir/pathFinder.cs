using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class pathFinder {
	
	//instance this pathFinder
	private pathFinder(){
		tileDicts = new Dictionary<Vector3, GameObject> ();

	}
	readonly static pathFinder instance = new pathFinder();
	public static pathFinder Instance {
		get {
			return instance;
		}
	}

	//public Vector3 startPosition;
	//public Vector3 targetPosition;
	public Vector3 MapSize;

	public Color CosthighlightColor = new Color((float)46/255, (float)224/255, (float)225/225, 0.8f);
	public Color SenseHighlightColor =  new Color((float)232/255, (float)56/255, (float)149/225, 0.6f);
	private Color highlightColor;
	public Color resetColor = new Color (1.0f, 1.0f, 1.0f, 1.0f);
	public bool onShowpath = true;
	public bool OnFind = true;
	
	//make a dictionary for saving the tiles 
	public Dictionary<Vector3, GameObject> tileDicts; // This is initilised in boardManager

//=======================
// a* for geometry finding
	public void pathFinding(Vector3 start, Vector3 target, ref List<Location> out_path){
			
		if (OnFind) {
		//preparing costmap(search for) A* algorithm 
		//Only can be initialise after tileDicts's initialisation 
			Grid localGrid = new Grid ((int)MapSize.x, (int)MapSize.y, GraphicType.CostGraph); 
			//Debug.Log("start point is " + start + "/ target point is " + target);
			start = clampPosition(start);
			target = clampPosition(target);
			var astar = new AstarFinding (localGrid, new Location ((int)start.x, (int)start.y), 
			                              new Location ((int)target.x, (int)target.y));
			PathReverse (astar,ref out_path, start, target);
			//OnFind = false;
			//Debug.Log ("A NEW PATH HAS BEEN FOUND");
		} 
	}

	private Vector3 clampPosition( Vector3 target){
		Vector3 result = new Vector3 (Mathf.Clamp (target.x, 0, MapSize.x-1), Mathf.Clamp (target.y, 0, MapSize.y-1), 0);
		return result;
	}

	private void PathReverse(AstarFinding astar,ref List<Location> path, Vector3 start, Vector3 target){
		path.Clear ();
		Location Current = new Location ((int)target.x, (int)target.y);
		path.Add (Current);
		while (!Current.Equals(new Location((int)start.x,(int)start.y))) {
			Current = astar.cameFrom[Current];
			path.Add(Current);
		}
		path.Reverse ();
	}
// =======================================
// using A* for sence path finding 

	public void sensePathFinding(Vector3 sourcePosition, Vector3 sensorPosition, ref List<Location> out_path, ref int TotalAttenuation){

		//preparing costmap(search for) A* algorithm 
		//Only can be initialise after tileDicts's initialisation 
			Grid localGrid = new Grid ((int)MapSize.x, (int)MapSize.y, GraphicType.PropagationGraph); 
			//Debug.Log("start point is " + sourcePosition + "/ target point is " + sensorPosition);
			sourcePosition = clampPosition(sourcePosition);
			sensorPosition = clampPosition(sensorPosition);
			var astar = new AstarFinding (localGrid, new Location ((int)sourcePosition.x, (int)sourcePosition.y), 
			                              new Location ((int)sensorPosition.x, (int)sensorPosition.y));

			PathReverse (astar,ref out_path, sourcePosition, sensorPosition); // get distance from here
			GetTotalCost(astar,ref TotalAttenuation, sensorPosition);;
			//Debug.Log ("A sence path has been found");


	}

	private void GetTotalCost(AstarFinding astar, ref int attenuation, Vector3 target){
		Location current = new Location ((int)target.x, (int)target.y);
		attenuation = astar.costSoFar [current]; 
	}



//============================================
//display the path
	public void DisplayPath(List<Location> path, GraphicType type){
		switch (type) {
		case GraphicType.CostGraph:
			highlightColor = CosthighlightColor;
			break;
		case GraphicType.PropagationGraph:
			highlightColor = SenseHighlightColor;
			break;
		default:
			Debug.LogWarning("Display type is wrong !");
			break;
		}
		// highlight the path found 
		if (onShowpath){
			// it's not a good idea to reset all the tite
			//ResetPathColor(); 
			foreach(var tempLocation in path){
				tileDicts[new Vector3(tempLocation.x,tempLocation.y,0)]
			.gameObject.GetComponent<Renderer>().material.color = highlightColor;
			}

		} else if (!onShowpath) {
			ResetALLColor();
		}
	}

	public void ResetALLColor(){
		foreach (KeyValuePair<Vector3, GameObject> temp in tileDicts) {
			temp.Value.GetComponent<Renderer> ().material.color = resetColor;
		}
	}

	public void ResetPathColor(List<Location> path){
		if (path.Count > 0) {
			foreach (var tempLocation in path) {
				tileDicts [new Vector3 (tempLocation.x, tempLocation.y, 0)]
				.gameObject.GetComponent<Renderer> ().material.color = resetColor;
			}
		} else
			Debug.LogWarning ("reset Path is empty");

	}

	public void ResetTile(){
		tileDicts.Clear ();
	}

// ===================================================================
// debug finding result
	void DrawGrid(Grid grid, AstarFinding astar) {
		String s = "";
		// Print out the cameFrom array
		for (var y = 0; y < this.MapSize.y ; y++)
		{
			for (var x = 0; x < this.MapSize.x; x++)
			{
				Location id = new Location(x, y);
				Location ptr = id;
				if (!astar.cameFrom.TryGetValue(id, out ptr))
				{
					ptr = id;
				}
				if (ptr.x == x+1) { s +="\u2192 "; }
				else if (ptr.x == x-1) { s +="\u2190 "; }
				else if (ptr.y == y+1) { s +="\u2193 "; }
				else if (ptr.y == y-1) { s +="\u2191 "; }
				else { s += "* "; }
			}
			s += "\n ";
		}
		Debug.Log (s);
	}
	
	
	public void testFinding(){
		Debug.Log ("the mapsize is " + MapSize);
		Debug.Log ("the number of tileDicts" + tileDicts.Count);
	}

}
