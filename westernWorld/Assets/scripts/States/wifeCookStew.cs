using UnityEngine;
using System.Collections;

public sealed class CookStew : State<Elsa> {
		
	static readonly CookStew instance = new CookStew();
		
	public static CookStew Instance {
			get {
				return instance;
			}
		}
		
	static CookStew () {}
	private CookStew () {}
		
		public override void Enter (Elsa agent) {
			Debug.Log("walking into kichen ...");

			if (agent.Cooking == false) {
				Debug.Log (" \n " + agent.agentID +
			           "puttin' the stew in the oven");

				// send a delayed messgage to herself
				MessageDispatcher.Instance.DispatchMessage(4.5, //delay
			                                           agent.agentID,//sender id
			                                           agent.agentID,//receiver id
			                                           (int)message_type.Msg_StewReay,//message
			                                           null);// no additional info
			 
				agent.Cooking = true;
			}

		}
		
		public override void Execute (Elsa agent) {
			Debug.Log("im cooking happly in to kichen...");
			if (agent.NatureCall == true) agent.ChangeState(VisitBathroom.Instance);
		}
		
		public override void Exit (Elsa agent) {
			Debug.Log("walk out kichen...");
		}
		
		//how to handle message in cooking state
		public override bool OnMessage(Elsa agent, Telegram msg){
			switch (msg.Msg) {
				case (int)message_type.Msg_StewReay:
				{
					Debug.Log("Stew ready! Let's eat ");
					//tell bob it's ready
					MessageDispatcher.Instance.DispatchMessage(Constants.SEND_MSG_IMMEDIATELY,
			                                           agent.agentID,
			                                           (int)AgentName.MINER_BOB,
			                                           (int)message_type.Msg_StewReay,
			                                           null);
					agent.Cooking = false;
					agent.ChangeState(DoHousework.Instance);
				}
			return true;
			}//end switch
		//if not go into switch return false
		return false;
		}
		
	public override bool OnSense(Elsa agent, ModalityType modality){return false;}
}
