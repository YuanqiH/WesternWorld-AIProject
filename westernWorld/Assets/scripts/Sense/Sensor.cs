using UnityEngine;
using System.Collections;

public enum OrientationType{Omni, Cone};

public class Sensor {

	private bool sight, hear, smell; // for setting in the game
	private Agent attachedAgent;

	public Vector3 position{
		get{
			return localTrans.localPosition;
		}
	} // could be wrong, or use transform ?
	public Transform localTrans;
	public OrientationType orientationType; // can be extended to Cone mode
	public double Threshold;

	public virtual bool detectsModality(Modality modality){// capability
	
		switch (modality.Type) {
		case ModalityType.Sight:
			return sight;
		case ModalityType.Hearing:
			return hear;
		case ModalityType.Smell:
			return smell;
		default:
			Debug.LogWarning("No Madality has been chosen");
			return false;
		}// end switch 
	}


	public virtual bool Notify(Signal signal){ 
		return attachedAgent.HandleSense(signal.modality.Type);
	}  // recive signal here
	//TODO: how to set threshold and capability dynamicly 
	public void Init(Transform localtrans, OrientationType orientation,bool sight, bool hear, bool smell, double threshold, Agent agent){
		this.localTrans = localtrans;
		this.orientationType = orientation;
		this.sight = sight;
		this.hear = hear;
		this.smell = smell;
		this.Threshold = threshold;
		this.attachedAgent = agent;
	}

}
