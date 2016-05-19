using UnityEngine;

public sealed class GlobalState : State<Bob> {
	
	static readonly GlobalState instance = new GlobalState();
	
	public static GlobalState Instance {
		get {
			return instance;
		}
	}
	
	static GlobalState () {}
	private GlobalState () {}

	public override void Enter (Bob agent) {}

	public override void Execute (Bob agent) {

	}

	public override void Exit (Bob anget) {}

	// check if wife ask me home in global state
	public override bool OnMessage(Bob agent, Telegram msg){ 		
		switch (msg.Msg) {
		case (int)message_type.Msg_NeedHusband:// if the message is home, start to cook
		{
			Debug.Log (" Message handled by " + agent.name + 
			           " at time: " + Time.time);
			Debug.Log( "\n" + agent.name + " : Babe, Im on my way back! ");
			agent.Target = gameManager.instance.gameInfo.HomeAdd.position;
			agent.ChangeState(MoveState.Instance);
		}
		return true;
		}//end switch
	return false; // if not into switch
	}

	public override bool OnSense(Bob agent, ModalityType modality){ // TODO: could be more complicated 
		switch (modality) {
		case ModalityType.Sight:
			Debug.Log(agent.name + ": I have see the light in agent");
			agent.simpleSenseResponse();
			return true;
		case ModalityType.Hearing:
			return true;
		case ModalityType.Smell:
			return true;
		default:
			Debug.LogError("no matching modality type in handel sense in " + agent.name);
			return false;
		}
		return false;// in not into the switch 
	}
	
}
