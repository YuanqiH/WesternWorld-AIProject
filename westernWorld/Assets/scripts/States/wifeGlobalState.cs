using UnityEngine;
using System.Collections;

public class wifeGlobalState : State<Elsa> {

	static readonly wifeGlobalState instance = new wifeGlobalState();
	
	public static wifeGlobalState Instance {
		get {
			return instance;
		}
	}
	
	static wifeGlobalState () {}
	private wifeGlobalState () {}
	
	public override void Enter (Elsa agent) {}
	
	public override void Execute (Elsa agent) {
		float callChance = Random.Range (0.0f, 1.0f);
		if (callChance < 0.02) {// 1 in 50 chance
			if(agent.NatureCall == false) // if not in the bathroom
				agent.NatureCall = true;
		}

		// handle husband msg sending
		if (agent.husbandCall == true) {
			MessageDispatcher.Instance.DispatchMessage(Constants.SEND_MSG_IMMEDIATELY,
			                                           agent.agentID,
			                                           (int)AgentName.MINER_BOB,
			                                           (int)message_type.Msg_NeedHusband,
			                                           null);
			agent.husbandCall = false;// reset the bool 
		}
	}
	
	public override void Exit (Elsa anget) {}

	// global state handle this message
	public override bool OnMessage(Elsa agent, Telegram msg){
		switch (msg.Msg) {
		case (int)message_type.Msg_HiHoneyImHome:// if the message is home, start to cook
		{
			Debug.Log (" Message handled by " + agent.name + 
			           " at time: " + Time.time);
			Debug.Log( "\n" + agent.agentID + " : Hi honey. Let me make you some of mah fine country stew ");

			agent.ChangeState(CookStew.Instance);
		}
			return true;
		}//end switch
		return false; // if not into switch
	}

	public override bool OnSense(Elsa agent, ModalityType modality){
		switch (modality) {
		case ModalityType.Sight:
			Debug.Log(agent.name + " : I have see the light in agent");
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
