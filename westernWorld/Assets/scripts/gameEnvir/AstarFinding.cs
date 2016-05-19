using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unitilities.Tuples;

public interface WeightedGraphic<L>{
	int Cost(Location a, Location b);
	GraphicType GetGType();
	IEnumerable<Location> Neighbors(Location id);
}

public struct Location{

	public readonly int x,y;
	public Location( int x, int y){
		this.x = x;
		this.y = y;
	}
}

public enum GraphicType{ CostGraph, PropagationGraph }

// ===================================================================
// searching map
public class Grid : WeightedGraphic <Location>{

	public static readonly Location[] DIRS = new [] // neighbors dirction
	{
		new Location(1, 0),
		new Location(0, -1),
		new Location(-1, 0),
		new Location(0, 1)
	};
	
	public int width, height;
	public Dictionary<Location, int> CostDictionary = new Dictionary<Location,int>();
	public GraphicType Gtype = new GraphicType ();
	public Grid(int width, int height,GraphicType type ) // constructor
	{
		this.Gtype = type;
		this.width = width;
		this.height = height;
		//initialise the cost dictionary
		CostDictionary.Clear ();
		for (int x=0; x< this.width; x++) {
			for(int y=0; y<this.height; y++){
				int costValue = 0;
				switch(type){
				case GraphicType.CostGraph:
					//add position to the list 
					costValue = pathFinder.Instance.tileDicts[new Vector3(x,y,0)].gameObject.GetComponent<tileVar>().movingCost;
					break;
				case GraphicType.PropagationGraph:
					costValue = pathFinder.Instance.tileDicts[new Vector3(x,y,0)].gameObject.GetComponent<tileVar>().sightPropagationValue;
					break;
				default:
					Debug.LogError("there is not matched cost type for search map");
					break;
				}
				CostDictionary.Add(new Location(x,y),costValue);
			}
		}// end for 
	}
	
	public bool InBounds(Location id) // using size to set bouning 
	{
		return 0 <= id.x && id.x < width
			&& 0 <= id.y && id.y < height;
	}

	public int Cost(Location start, Location end){ // get from the dictionary
		return CostDictionary[end];
	}

	public IEnumerable<Location> Neighbors(Location id) // smart way to return neighbors
	{
		foreach (var dir in DIRS) {
			Location next = new Location(id.x + dir.x, id.y + dir.y);
			if (InBounds(next)) {  // make sure in the bounds
				yield return next;
			}
		}
	}

	public GraphicType GetGType(){
		return this.Gtype;
	}

}

// ===================================================================
// priorityQueue
public class PriorityQueue<T>
{	
	//Based on binary heap algorithmn at :https://visualstudiomagazine.com/Articles/2012/11/01/Priority-Queues-with-C.aspx?Page=1
	//sort the list while insert/remove.
	//complexity is o(lg n) for insert/remove 
	//sort form smallest -> biggest

	private List<Tuple<T, int>> elements = new List<Tuple<T, int>>();
	
	public int Count
	{
		get { return elements.Count; }
	}

	public void Enqueue(T item, int priority) 
	{
		Tuple<T,int> temp = new Tuple<T, int> (item, priority);
		elements.Add (temp);
		int child_index = elements.Count-1;
		while (child_index > 0) {
			int parent_index = (child_index-1)/2;
			if(elements[child_index].second >= elements[parent_index].second) break; // if children >= parent, done
				{// sweep parent & child
				Tuple<T,int> changeTemp = elements[child_index];
				elements[child_index] = elements[parent_index];
				elements[parent_index]= changeTemp;
				}
			child_index = parent_index;
		}
	}

	public T Dequeue(){
		//assume the list is not empty
		int last_index = elements.Count - 1;
		T frontItem = elements [0].first;
		elements [0] = elements [last_index]; // put last into front, and remove
		elements.RemoveAt(last_index);
		// resort the list 
		--last_index;
		int parent_index = 0;
		while (true) {
			int child_index = parent_index*2 +1; // left children
			if(child_index > last_index) break; // no children done
			int child_index_r = child_index +1; // check right children
			if( child_index_r <= last_index && elements[child_index_r].second < elements[child_index].second)
				child_index = child_index_r;
			if( elements[parent_index].second <= elements[child_index].second) break; // parent smaller, done
			{ // sweep parent and child
			Tuple<T,int> changeTemp = elements[child_index];
			elements[child_index] = elements[parent_index];
			elements[parent_index]= changeTemp;
			}
			parent_index = child_index;
		}
		return frontItem;
	}

	public T peek(){ // return smallest without delet
		T frontItem = elements [0].first;
		return frontItem;
	}


}

// ===================================================================
// A* algorithmn
// using delgate for good extension !!!!
public delegate int HeuristicFunction(Location a, Location b);

public class AstarFinding {

	public Dictionary<Location, Location> cameFrom // for path found 
		= new Dictionary<Location, Location>(); 
	public Dictionary<Location, int> costSoFar // closed list 
		= new Dictionary<Location, int>();
	public HeuristicFunction Heuristic;

	static public int Heuristic_Manhattan(Location a, Location b)
	{
		return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
	}

	static public int Heuristic_Euclidean(Location a, Location b){ // for signal
		return (int)Vector2.Distance (new Vector2(a.x,a.y),new Vector2(b.x,b.y));
	}
	
	public AstarFinding( WeightedGraphic<Location> graph, Location start, Location goal)
	{

		switch (graph.GetGType()) {
		case GraphicType.CostGraph:
			Heuristic = new HeuristicFunction(Heuristic_Manhattan);
		//Debug.Log ("Im using Heuristic_Manhattan");
			break;
		case GraphicType.PropagationGraph:
			Heuristic = new HeuristicFunction(Heuristic_Euclidean);
		//Debug.Log ("Im using Heuristic_Euclidean");
			break;
		default:
			Debug.LogError("No match graphic type in a* finding !");
			break;
		}

		var frontier = new PriorityQueue<Location>(); 
		frontier.Enqueue(start, 0); 
		
		cameFrom[start] = start;
		costSoFar[start] = 0;
		
		while (frontier.Count > 0)
		{
			var current = frontier.Dequeue(); // get the smallest in Q, and delete it.
			
			if (current.Equals(goal))
			{
				break;
			}
			
			foreach (var next in graph.Neighbors(current))
			{
				int newCost = costSoFar[current] + graph.Cost(current, next);
				if (!costSoFar.ContainsKey(next)|| newCost < costSoFar[next])
				{
					costSoFar[next] = newCost;
					int priority = newCost + Heuristic(next, goal);
					frontier.Enqueue(next, priority);
					cameFrom[next] = current;
				}
			}
		}
	}
}







