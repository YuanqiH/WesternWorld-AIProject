using UnityEngine;


public enum ModalityType {Sight, Hearing, Smell};

abstract public class Modality {

	public ModalityType Type;
	public double inverseSpeed;
	public Vector3 SignalPosition; // can access by find attached object
	abstract public bool extraCheck(Sensor testSenor);
}


public class sightModality: Modality {
	// set instance
	private sightModality(){}
	static readonly sightModality instance = new sightModality();
	public static sightModality Instance {
		get {return instance;}
	}

	public void Awake(){
	this.Type = ModalityType.Sight;
	this.inverseSpeed = 0.0;
	//this.SignalPosition // can access by find attached object
	}

	public override bool extraCheck(Sensor testSenor){ //TODO make this for all kinds of modalities
		if (testSenor.orientationType == OrientationType.Omni)
			return true;
		else // checking cone
			return false;
	}
}
//==================================
//could extend to different modality and related extraCheck function
public class hearModality: Modality{
	public override bool extraCheck(Sensor testSenor){
		return true;
	}
}

public class smellModality: Modality{
	public override bool extraCheck(Sensor testSenor){
		return true;
	}
}



