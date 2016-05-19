using UnityEngine;

public sealed class WaitState : State<Bob> {

	static readonly WaitState instance = new WaitState();

	public static WaitState Instance {
		get {
			return instance;
		}
	}

	static WaitState () {}
	private WaitState () {}

	// let bob send message from at this state
	public override void Enter (Bob agent) {
		//let wife know i'm at home
		MessageDispatcher.Instance.DispatchMessage (Constants.SEND_MSG_IMMEDIATELY,
		                                            agent.agentID,
		                                            (int)AgentName.WIFE_ELSA,
		                                            (int)message_type.Msg_HiHoneyImHome,
		                                            null);
		Debug.Log (agent.name+" : Honey, im at home!!!");
	}

	public override void Execute (Bob agent) {
		agent.IncreaseWaitedTime(1); // wait till cook is ready
		//if (agent.WaitedLongEnough()) agent.ChangeState(CreateState.Instance);
	}

	public override void Exit (Bob agent) {
	}

	public override bool OnMessage(Bob agent, Telegram msg){ 
			switch (msg.Msg) {
			case (int)message_type.Msg_StewReay:// if the message is home, start to cook
			{
				Debug.Log (" Message handled by " + agent.name + 
				           " at time: " + Time.time);
				
				agent.ChangeState(EatState.Instance);
			}
				return true;
			}//end switch
			return false; // if not into switch
		} 

	public override bool OnSense(Bob agent, ModalityType modality){return false;}
}
