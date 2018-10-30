using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using uZone;

public class uZoneInstance : uLink.MonoBehaviour {
	
	
    void Awake()
    {
		uZone.InstanceManager.Server.Initialise();
    }

	void Start()
	{
		//for(int i = 0; i < uZone.InstanceManager.Server.cmdArgs.Count; i++)
		//{
			//if (i == 1)
			//{
			uZone.InstanceManager.Server.AddListener(this.gameObject);
			uLink.Network.InitializeServer(32, 25000); 
			//}
		//}

	}

	void OnGUI()
	{
		if(uZone.InstanceManager.Server.cmdArgs != null)
			GUI.Box(new Rect(10, 5, 230, 130), "");
		GUI.Label(new Rect(10, 5, 230, 35), uZone.InstanceManager.Server.cmdArgs[0]);
		GUI.Label(new Rect(10, 40, 230, 20), "ID: " + uZone.InstanceManager.Server.cmdArgs[0]+ "  " + uZone.InstanceManager.Server.instanceId);
		GUI.Label(new Rect(10, 60, 230, 20), "Port: " + uZone.InstanceManager.Server.gameServerPort.ToString());
		if(uZone.InstanceManager.Server.cmdArgs != null && uZone.InstanceManager.Server.cmdArgs.Count() > 1)
		{
			string argString = "";
			for(int i = 1; i < uZone.InstanceManager.Server.cmdArgs.Count(); i++)
				argString += uZone.InstanceManager.Server.cmdArgs[i] + " ";
			GUI.Label(new Rect(10, 80, 230, 35), "Parameters:\n" + argString);
		}
	}
	
	void uZone_OnDisconnected()
	{
		//Try and reconnect to uZone in case the connection is dropped.
		uZone.InstanceManager.Server.Reconnect();
	}
	
	void uZone_OnTerminate()
	{
		UnityEngine.Debug.Log("Terminating instance ...");	
	}
	
	void uZone_OnConnected(string id)
	{
		UnityEngine.Debug.Log("Instance connected.");
	}
}
