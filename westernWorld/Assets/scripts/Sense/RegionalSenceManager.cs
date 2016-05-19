using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct Notification{
	public double time;
	public Sensor sensor;
	public Signal signal;
}

public class ReginalSenceManager{

	public List<Sensor> Sensorlist;
	// reuse the priorityQ, notice the priority value should be as constructor value
	public PriorityQueue<Notification> NotificationQ;	
	private Dictionary<Sensor,List<Location>> Pathes; // for each sensor 
	private int propagationTotal;
	//==============================================
	//instance maanger
	ReginalSenceManager(){
		Sensorlist = new List<Sensor>(); // no need for sort, check every one in the list
		NotificationQ = new PriorityQueue<Notification>();
		Pathes = new Dictionary<Sensor,List<Location>> ();
	} // constructor
	static readonly ReginalSenceManager instance = new ReginalSenceManager();
	public static ReginalSenceManager Instance{
		get {
			return instance;
		}
	}
	
	public void RegisterSensor(Sensor newSensor){ // to do maybe not work 
		Sensorlist.Add (newSensor);
		Pathes.Add (newSensor, new List<Location> ());
	}

	public void AddSignal(Signal signal){ // call by the signal is introduced in the game
		//============================
		// agregation phase

		foreach (var tempSensor in Sensorlist) {
			//=============================
			// testing phase
			// check the signal modality first 
			if(tempSensor.detectsModality(signal.modality) ==  false)
				continue;
			// find the distance of the signal and check range
			List<Location> temp_path = new List<Location>();
			pathFinder.Instance.sensePathFinding(clampPosition(signal.Position),clampPosition(tempSensor.position),ref temp_path,ref propagationTotal); // TODO 
			float distance = temp_path.Count;
			Pathes[tempSensor] = temp_path;
			//Debug.Log ("The sence distance is" + distance);
			if( signal.maximumRange < distance )
				continue;
			// find the arrived intensity and check the threshold
			double intensity = signal.Intensity - propagationTotal;
			//Debug.Log("final intensity value is "+ intensity);
			if( intensity < tempSensor.Threshold)
				continue;

			// perform additional modality check
			if( signal.modality.extraCheck(tempSensor) == false) // TODO: make this extracheck better
				continue;

			// compute the exact time fo reciving the modality, send immediately by setting v-1 = 0
			double time = Time.time + distance * signal.modality.inverseSpeed;  // TODO: implement this in future

			//create a mew notification and enqueue
			Notification tempNotification = new Notification();
			tempNotification.sensor =  tempSensor;
			tempNotification.signal = signal;
			tempNotification.time = time;
			this.NotificationQ.Enqueue(tempNotification, (int)tempNotification.time);
			//==========================
			//notification phase
			sendSignal(); // send this signal immediately 
		}// end forEach

	} // end add signal

	public void sendSignal(){ // method for the add signal use
		double currentTime = Time.time;
		while (NotificationQ.Count > 0) { // if there is at one notification in the queue
		
			Notification tempNotification = NotificationQ.peek();
			if(tempNotification.time < currentTime){
				tempNotification.sensor.Notify(tempNotification.signal); 
				NotificationQ.Dequeue();// delete the front item simply 
			}
			else
				break;
		}// end while 

	}

	public void displaySensePath(){
		foreach (KeyValuePair<Sensor,List<Location>> m_path in Pathes) {
			pathFinder.Instance.DisplayPath (m_path.Value, GraphicType.PropagationGraph);
		}
	}

	public void resetSensePath(){
		foreach (KeyValuePair<Sensor,List<Location>> m_path in Pathes) {
			pathFinder.Instance.ResetPathColor (m_path.Value);
			m_path.Value.Clear();
		}
	}
	
	private Vector3 clampPosition(Vector3 position){
		position.x = Mathf.FloorToInt (position.x+0.5f);
		position.y = Mathf.FloorToInt (position.y+0.5f);
		position.z = 0;
		return position;
	}

}