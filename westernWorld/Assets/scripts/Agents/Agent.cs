using UnityEngine;
using System.Collections;

public enum AgentName{
	MINER_BOB,
	WIFE_ELSA
};

static class Constants{
	public const double SEND_MSG_IMMEDIATELY = 0.0;
}

abstract public class Agent: MonoBehaviour{

	//this one need to be modified into instance format
	// testing the deleget method 
	public Vector3 Target;

	//======================
	public Sensor sensor;
	public bool sightOn, hearOn, smellOn;
	public double threshold;
	// every agent has their unique id
	private int m_ID;
	// the possible id, accessable by all instances
	private static int m_iNextValidID = (int)AgentName.MINER_BOB;
	
	public int agentID {
		get{
			return m_ID;
		}
		// set the ID
		set{
			if(value >= m_iNextValidID){
				m_ID = value;
				m_iNextValidID = value + 1;
			}
			else
				Debug.LogWarning ("ID should be equal or greater than: " + m_iNextValidID);
		}
	}

	public Agent(){}

	//all agent must implement an update function
	public abstract void Update();

	//subclass can communicate using message
	//this function will be called by the messagedispatcher 
	public abstract bool HandleMessage(Telegram msg);

	public abstract bool HandleSense(ModalityType modality);
	
}
