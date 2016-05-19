public class StateMachine <T> {
	
	private T agent;
	private State<T> currentState;
	private State<T> globalState;
	private State<T> previousState;

	public void Awake () {
		this.currentState = null;
		this.previousState = null;
		this.globalState = null;
	}

	public void Init (T agent, State<T> startState, State<T> global) {
		this.agent = agent;
		this.currentState = startState;
		this.previousState = startState;
		//initialize global state
		this.globalState = global;
	}

	public void Update () {
		if (this.currentState != null) this.currentState.Execute(this.agent);
		//in update call global state every time
		if(this.globalState != null) this.globalState.Execute (this.agent);
		// send delay massage every update cycle
		// otherwise it wont be sent at all
		MessageDispatcher.Instance.DispatchDelayedMessage ();
	}
	
	public void ChangeState (State<T> nextState) {
		if(this.previousState != this.currentState ) this.previousState = this.currentState; // save current state into previous
		if (this.currentState != null) this.currentState.Exit(this.agent);// exit current state
		this.currentState = nextState;// assign nextstate
		if (this.currentState != null) this.currentState.Enter(this.agent);// enter current state
	}

	public void revertPreviousState(){
		ChangeState (this.previousState);// set previous state as the next state
	}
	//====================
	//message response
	public bool HandleMessage(Telegram msg){
		//first see if the current state is valid and it can handle the message 
		if (this.currentState != null && this.currentState.OnMessage (this.agent, msg)) {
			return true;
		}
		//if not, and if a gloable sate has been implemented, send it to global
		if (this.globalState != null && this.globalState.OnMessage (this.agent, msg)) {
			return true;
		}

		return false;
	}
	//====================
	//sence response
	public bool HandleSense(ModalityType modality){
		if (this.currentState != null && this.currentState.OnSense(this.agent, modality)) {
			return true;
		}
	//only do this in the globalstate, it's dummy to extend into every state
		if (this.globalState != null && this.globalState.OnSense(this.agent, modality)) {
			return true;
		}

		return false;
	
	}

}