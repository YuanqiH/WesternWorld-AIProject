using UnityEngine;
using System.Collections;

public class Elsa : Agent {

	private StateMachine<Elsa> stateMachine;

	private bool natureCall;
	private bool OnCooking;
	public bool husbandCall;
	public GameObject senseRecieveParticle;

	public void Awake(){
		stateMachine = new StateMachine<Elsa>();
		this.stateMachine.Init (this, DoHousework.Instance, wifeGlobalState.Instance);
		this.agentID = (int)AgentName.WIFE_ELSA;
		//register in the agentmanager
		agentManager.Instance.RegisterAgent (this);

		// initiate sensor and register in sence manager
		this.sensor = new Sensor() ;
		this.sensor.Init(this.transform, OrientationType.Omni,sightOn,hearOn,smellOn,threshold,this);
		ReginalSenceManager.Instance.RegisterSensor (this.sensor);

	}

	public void Start(){
		transform.position = gameManager.instance.gameInfo.HomeAdd.position;
	}

	public bool NatureCall{
		set{
			natureCall = value;
		}
		get{
			return natureCall;
		}
	}

	public bool Cooking{
		set{
			OnCooking = value;
		}
		get {
			return OnCooking;
		}
	}

	public bool CallHusband{
		set{
			husbandCall = value;
		}
		get{
			return husbandCall;
		}
	}

	public void simpleSenseResponse(){
		Instantiate (senseRecieveParticle, transform.localPosition, transform.localRotation);
	}

	public void OnCallHusband(){
		CallHusband = !CallHusband;
	}

	public void homeAddInfo(){

	}

	public void ChangeState (State<Elsa> state) {
		this.stateMachine.ChangeState(state);
	}

	//go back to previous state
	public void RevertPreviousState(){
		this.stateMachine.revertPreviousState ();
	}

	public override void Update(){
		this.stateMachine.Update();
	}

	public override bool HandleMessage(Telegram msg){
		return this.stateMachine.HandleMessage (msg);
	}

	public override bool HandleSense(ModalityType modality){
		return this.stateMachine.HandleSense(modality);
	}

}
