using UnityEngine;
using System.Collections;

//declare delegate for extraxInfo
public delegate void DExtraInfo();

// specify message type
public enum message_type{
	Msg_HiHoneyImHome,
	Msg_StewReay,
	Msg_NeedHusband
};

//package message into telegram, together with additional inforn
public class Telegram{

	public int Sender;// agent who send the msg
	public int Receiver;// agent who receive msg

	//message itself, defined by enum
	public int Msg;

	//messages can be dispatched immediately or delayed for a specified amount of time.
	public double DispatchTime;

	//any additional information,
	// can use new void to create new function
	//using delegate to make method as a variable !
	public event DExtraInfo ExtraInfo;

	//constracter 
	public Telegram(double dispatchTime, 
	                int sender, 
	                int receiver,
	                int msg,
	                DExtraInfo extrainfo){

		this.Sender = sender;
		this.Receiver = receiver;
		this.Msg = msg;
		this.DispatchTime = dispatchTime;
		this.ExtraInfo += extrainfo; // adding the local extra info method to the new telegram
	}

}

