using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using static Telegram;

public class MessageDispatcher {
	

	private List<Telegram> PriorityQ =  new List<Telegram>();

	//this method is utilized by dispatchmessage or dispatchDelayedMessage
	private void Discharge(Agent pReceiver, Telegram msg){
		//call the handle message in the agent with new telegram
		pReceiver.HandleMessage (msg);
	}

	private MessageDispatcher(){}

	//set the instance 
	readonly static MessageDispatcher instance = new MessageDispatcher();
	public static MessageDispatcher Instance{
		get{
			return instance;
		}
	}

	//send message to another agent
	public void DispatchMessage(double delay,
	                            int sender,
	                            int receiver,
	                            int msg,
	                            DExtraInfo extraInfo){

		//get the receiver of the message
		Agent pReceiver = agentManager.Instance.GetAgentFromID(receiver);

		//create the telegram
		Telegram telegram = new Telegram( 0 , sender, receiver, msg, extraInfo);

		//if there is no delay, route the telegram immediately
		if (delay <= 0.0) {
			//send the telegram to the recipient
			this.Discharge (pReceiver, telegram);
		} else {
			//set the time stamp to the telegram
			double currentTime = Time.time;
			telegram.DispatchTime = currentTime + delay;
			//put it in the queue
			PriorityQ.Add(telegram);

		}


	}

	//this called every main loop
	public void DispatchDelayedMessage(){
	
		//fist get the current time
		double currentTime = Time.time;
		if (PriorityQ.Count > 0) {// it contain any telegram
			if ((PriorityQ[0].DispatchTime < currentTime) && 
		       (PriorityQ[0].DispatchTime >0)) {
		
				//read the telegram from the beginning of the queue
				Telegram telegram = PriorityQ [0];

				//find the recipient
				Agent pReceiver = agentManager.Instance.GetAgentFromID (telegram.Receiver);

				//send the telegram to the recipient
				this.Discharge (pReceiver, telegram);

				//remove it from the queue
				PriorityQ.RemoveAt (0);
			}
		}
	}

}

 
