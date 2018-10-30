using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using uZone;

public class uZoneClientGUI : MonoBehaviour {

    public string host = "127.0.0.1";
	public string port = "12345";
	public string game = "";
	public string instanceID = "";
	public string nodeID = "";
	public List<string> cmdArg = new List<string>();
	
	
	//GUI
	private Vector2 scrollViewVector = Vector2.zero;
	private Vector2 scrollViewVector2 = Vector2.zero;
	private Vector2 scrollViewVector3 = Vector2.zero;
	private string tempArg = "";
	private static List<Node> parsedNodes = new List<Node>();
	private static List<GameInstance> parsedInstances = new List<GameInstance>();
	private static List<string> instanceType = new List<string>();


    void Awake()
    {
		uZone.InstanceManager.Initialise();	
    }

	void Start()
	{
		/*
		 * This is called to make the object subscribe to events
		 * and have its named callbacks executed. These are defined
		 * at the end of this script file.
		 */
		uZone.InstanceManager.AddListener(this.gameObject);
		
		//One way of defining a callback. This is equivalent to connectedCallbackTwo.
		EventCallback connectedCallbackOne =
			arg =>
		{
			string id = (string) arg;
			UnityEngine.Debug.Log("Registered with ID: !" + id);
		};
		
		//This sets the callback to be executed on the event where the application
		//successfuly connects to uZone to be connectedCallbackOne.
		uZone.InstanceManager.SetCallback(EventCode.UzoneConnected, connectedCallbackOne);
		
		//A second call will overwrite the previously assigned callback.
		uZone.InstanceManager.SetCallback(EventCode.UzoneConnected, connectedCallbackTwo);
		
		//Calling this method will remove whatever callback is currently set for this event.
		//Also note that this is global for the application, and not just for this object.
		uZone.InstanceManager.DeleteCallback(EventCode.UzoneConnected);

	}
	
	//Second way of defining a callback. This is equivalent to connectedCallbackOne.
	void connectedCallbackTwo(object arg)
	{
		string id = (string) arg;
		UnityEngine.Debug.Log("Registered with ID: !" + id);	
	}

	void OnGUI()
	{
		string status;
		string status2;
		
		if(uZone.InstanceManager.isConnected)
			status = "Connected";
		else
			status = "Disconnected";
		if(uZone.InstanceManager.isListening)
			status2 = "Listening for data ...";
		else
			status2 = "Idle";
		
		GUI.Box(new Rect(180, 20, 150, 80), "Status");
		GUI.Label(new Rect(190, 40, 140, 20), status); 
		GUI.Label(new Rect(190, 60, 140, 25), status2);
		
		
		GUI.Box(new Rect(10, 20, 160, 350), "Control");
		
		if(!uZone.InstanceManager.isConnected)
		{
			if (GUI.Button (new Rect (20, 50, 140, 20), "Connect"))
			{
				uZone.InstanceManager.Connect(host, Int32.Parse(port));
			}
		}
		else
		{
			if (GUI.Button (new Rect (20, 50, 140, 20), "Disconnect"))
			{
				uZone.InstanceManager.Disconnect();	
			}
		}
		
		GUI.Label(new Rect(20, 80, 35, 20), "Host:");
		host = GUI.TextArea(new Rect (65, 80, 95, 20), host);
		GUI.Label (new Rect(20, 110, 35, 20), "Port:");
		port = GUI.TextArea (new Rect(65, 110, 95, 20), port);
		
		if(GUI.Button(new Rect(20, 140, 140, 20), "Start Instance"))
		{
			if(game != "")
			{
				if(nodeID != "")
					uZone.InstanceManager.StartGameInstance(game, nodeID, cmdArg);
				else
					uZone.InstanceManager.StartGameInstance(game, cmdArg);
			}
		}
		if(GUI.Button(new Rect(20, 170, 140, 20), "Stop Instances"))
		{
			if(instanceID != "")
				uZone.InstanceManager.StopInstance(instanceID);
			else if(nodeID != "")
				uZone.InstanceManager.StopNodeInstances(nodeID);
			else if(game != "")
				uZone.InstanceManager.StopGameTypeInstances(game);
			else
				uZone.InstanceManager.StopAllInstances();
		}
		if(GUI.Button(new Rect(20, 200, 140, 20), "List Instances"))
		{
			if(nodeID != "")
				uZone.InstanceManager.ListNodeInstances(nodeID);
			else if(game != "")
				uZone.InstanceManager.ListInstances(game);
			else
				uZone.InstanceManager.ListAllInstances();
		}
		if(GUI.Button(new Rect(20, 230, 140, 20), "List Instance Types"))
		{
			uZone.InstanceManager.ListInstanceTypes();
		}
		if(GUI.Button (new Rect(20, 260, 140, 20), "List Nodes"))
		{
			uZone.InstanceManager.ListAvailableNodes();	
		}
		if(GUI.Button (new Rect(20, 290, 140, 60), "Clear Selection"))
		{
			nodeID = "";
			instanceID = "";
			game = "";
		}
		
		
		
		
		
		
		/////////////////////
		
		GUI.Box(new Rect(180, 110, 140, 130), "Available Types");
		int pos = 135;
		scrollViewVector3 = GUI.BeginScrollView (new Rect(180, 135, 157, 100),
							scrollViewVector3, new Rect(180, 135, 140, instanceType.Count * 30));
		foreach(string type in instanceType)
		{
			if(GUI.Button(new Rect(190, pos, 120, 23), type))
				game = type;
			pos += 50;
		}
		GUI.EndScrollView ();
		
		/////////////////////
		
		GUI.Box (new Rect(180, 250, 140, 120), "Parameters");
		tempArg = GUI.TextArea(new Rect(190, 280, 120, 20), tempArg);
		if(GUI.Button(new Rect(190, 310, 50, 20), "Add"))
		{
			cmdArg.Add(tempArg);
			tempArg = "";
		}
				if(GUI.Button(new Rect(260, 310, 50, 20), "Clear"))
		{
			cmdArg.Clear();
		}
			
		/////////////////////
		
		GUI.Box(new Rect(340, 20, 200, 350), "Available Instances");
		pos = 40;
		scrollViewVector = GUI.BeginScrollView (new Rect (340, 40, 217, 320), 
							scrollViewVector, new Rect (340, 40, 200, parsedInstances.Count * 115));

		foreach(GameInstance instance in parsedInstances)
		{
			if(GUI.Button(new Rect(480, pos, 55, 20), "Select"))
			{
				instanceID = instance.id;
			}
			GUI.Label(new Rect(350, pos, 130, 115), "ServerID: " + instance.id + "\n" +
													"IP: " + instance.ip + "\n" +
													"Port: " + instance.port + "\n" +
													"Type: " + instance.type);
			pos += 115;
		}
		GUI.EndScrollView();
		
		//////////////////////
		
		pos = 40;
		GUI.Box(new Rect(560, 20, 200, 350), "Connected Nodes");
		scrollViewVector2 = GUI.BeginScrollView (new Rect (560, 40, 217, 320), 
							scrollViewVector2, new Rect (560, 40, 200, parsedNodes.Count * 90));
		foreach(Node node in parsedNodes)
		{
			if(GUI.Button(new Rect(700, pos, 55, 20), "Select"))
			{
				nodeID = node.id;	
			}
			GUI.Label(new Rect(570, pos, 130, 120), "ID: "		+ node.id + "\n" +
													"Hostname: "+ node.hostname + "\n" +
													"IP: "		+ node.ip);
			pos+=90;
		}
		GUI.EndScrollView();
		
	}
	
		/*
		 * Below are all named callbacks defined. These will automatically
		 * be invoked if the GameObject holding them has been added with AddListener.
		 * It is important that they are correctly named, and take the right type as
		 * their parameter.
		 */
	
		public void uZone_OnConnected(string registeredId)
		{
			UnityEngine.Debug.Log("Client connected to uZone with ID: " + registeredId);
		}
	
		public void uZone_OnNodeConnected(Node connectedNode)
		{
			UnityEngine.Debug.Log("Node " + connectedNode.id + " connected");
		}
	
		public void uZone_OnNodeDisconnected(string nodeId)
		{
			UnityEngine.Debug.Log("Node " + nodeId + " disconnected");
		}
	
		public void uZone_OnNodeListReceived(List<Node> runningNodes)
		{
			parsedNodes = runningNodes;
			UnityEngine.Debug.Log("Received Node-list");	
		}
	
		public void uZone_OnInstanceStarted(GameInstance gameInstance)
		{
			UnityEngine.Debug.Log("Instance " + gameInstance.id + " started");
			UnityEngine.Debug.Log("By request #" + gameInstance.requestId);
			parsedInstances.Add(gameInstance);
		}
	
		public void uZone_OnInstanceStopped(string id)
		{
			UnityEngine.Debug.Log("Instance "+ id + " was stopped");
			uZone.InstanceManager.ListAllInstances();
		}
	
		public void uZone_OnInstanceListReceived(List<GameInstance> instanceList)
		{
			parsedInstances = instanceList;	
			UnityEngine.Debug.Log("Received list with " + instanceList.Count.ToString() + " running instances");
		}
	
		public void uZone_OnInstanceTypeListReceived(List<string> receivedList)
		{
			instanceType = receivedList;
			UnityEngine.Debug.Log("Received list of available games");
		}
	
		public void uZone_OnError(ErrorCode errorCode)
		{
			UnityEngine.Debug.Log("Received error: " + errorCode.ToString());
		}
	
		public void uZone_OnDisconnected()
		{
			UnityEngine.Debug.Log("Client disconnected from uZone");
		}
}
