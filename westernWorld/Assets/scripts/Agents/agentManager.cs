using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class agentManager {

	private Dictionary<int,Agent> agentMaps;
	// create dictionaris 
	public agentManager(){
		agentMaps = new Dictionary< int , Agent > ();
	}

	// create instance
	static readonly agentManager instance = new agentManager ();

	public static agentManager Instance{
		get{
			return instance;
		}
	}

	// the pointer needs to be checked
	public void RegisterAgent(Agent NewAgent){
		agentMaps.Add (NewAgent.agentID, NewAgent);
	}

	// return the agent back from ID
	public Agent GetAgentFromID(int ID){
		if (agentMaps.Count !=  0 ){
			Agent tempAgent; 
			agentMaps.TryGetValue(ID, out tempAgent);
			return tempAgent;
		}
		else {
			Debug.LogError (" this not matching registered ID Entity..");
			return null;
		}
	}

	//method to remove from register
	public void RemoveAgent(Agent pAgent){
		agentMaps.Remove (pAgent.agentID);
	}

}
