﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LagStream : MonoBehaviour
{
	public float globalLagTime = 0.2f;
	public GameObject player;
	static List<LagObject> commandList = new List<LagObject>();
	PlayerMove playerScript;
	
	// Use this for initialization
	void Start ()
	{
		playerScript = player.GetComponent<PlayerMove>();
		Debug_ShowCommandList();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (Input.GetKeyDown(KeyCode.Return)) { globalLagTime = -1; }
		
		UpdateCommandListLags();

		if (commandList.Count > 0)
		{
			if (globalLagTime < 0.0f)
			{
				globalLagTime = 0.0f;

				foreach (LagObject command in commandList)
				{
					switch (command.GetCommand())
					{
						case "move left" : player.transform.Translate(-0.2f,0,0); break;
						case "move right" : player.transform.Translate(0.2f,0,0); break;
						case "jump" : player.transform.Translate(0,1,0); break;
					}
				}

				commandList.Clear();
			}
			else if (Time.time >= commandList[0].GetStartTime() + commandList[0].GetDelay())
			{
				playerScript.RunCommand(GetNextLagObject());
			}
		}
	}

	// Use this to delay an action made by the player
	public void AddNewCommand(LagObject newLagObject)
	{
		commandList.Add(newLagObject);
	}

	LagObject GetNextLagObject()
	{
		LagObject nextCommand;

		if (commandList.Count > 0)
		{
			nextCommand = commandList[0];

			commandList.RemoveAt(0);
		}
		else
		{
			nextCommand = null;
		}
		
		Debug.Log(nextCommand.GetCommand() + " " + nextCommand.GetStartTime() + " " + nextCommand.GetDelay());
		return nextCommand;
	}

	void UpdateCommandListLags()
	{
		foreach (LagObject command in commandList)
		{
			command.SetDelay(globalLagTime);
		}
	}

	public void Debug_ShowCommandList()
	{
		string commands = ""; 
		
		foreach (LagObject command in commandList)
		{
			commands += command.GetCommand() + ", ";
		}
	}
}