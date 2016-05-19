using UnityEngine;

public class Bob : Agent {

	private StateMachine<Bob> stateMachine;
// ===================================================================
//parameter
	public int MealWaitingTime = 0;
	public int EatTimes = 0;
	private moveMent moveScript;
	public GameObject senseRecieveParticle;

// ===================================================================
	public void Awake () {
		this.stateMachine = new StateMachine<Bob>();
		this.stateMachine.Init(this, workState.Instance, GlobalState.Instance);
		this.agentID = (int)AgentName.MINER_BOB;
		//register in the agentmanager
		agentManager.Instance.RegisterAgent (this);
		moveScript = GetComponent<moveMent> ();
		// initiate sensor and register in sence manager
		this.sensor = new Sensor() ;
		this.sensor.Init(this.transform, OrientationType.Omni,sightOn,hearOn,smellOn,threshold,this);
		ReginalSenceManager.Instance.RegisterSensor (this.sensor);

	}
//=================
// functions used by states
	public void IncreaseWaitedTime (int amount) {
		this.MealWaitingTime += amount;
	}

	public void CreateTime () {
		this.EatTimes++;
		this.MealWaitingTime = 0;
	}

	public void moveTo(){
		this.Target = TOOLS.PositionClamp (this.Target);
		moveScript.setPathTarget (this.Target);
		moveScript.Move ();
	}

	public bool TargetArrival(){ //TODO: return true slight earlier than actual arrival 
		return ((Vector2)this.Target == (Vector2)TOOLS.PositionClamp(transform.localPosition));
	}

	public void simpleSenseResponse(){
		Instantiate (senseRecieveParticle, transform.localPosition, transform.localRotation);
	}

//=============================
//state set
	public void ChangeState (State<Bob> state) {
		this.stateMachine.ChangeState(state);
	}
	//go back to previous state
	public void RevertPreviousState(){
		this.stateMachine.revertPreviousState ();
	}

	public override void Update () {
		this.stateMachine.Update();
	}

	public override bool HandleMessage(Telegram msg){
		return this.stateMachine.HandleMessage (msg);
	}

	public override bool HandleSense(ModalityType modality){
		return this.stateMachine.HandleSense(modality);
	}
}
