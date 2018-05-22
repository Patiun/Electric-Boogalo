using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Author: Greg Kilmer
 * Function: Allows users to debug the system.
 * Last Updated: 5/22/2018
 */

public class DebugController : MonoBehaviour {

	public bool debug;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("tab")) {
			if (!debug) {
				debug = true;
				DebugPrint ("Debug mode enabled");
			} else {
				debug = false;
				DebugPrint ("Debug mode disabled");
			}
		}
		if (debug) {
			//Reload Scene
			if (Input.GetKeyDown("r")) {
				DebugPrint ("Reloading scene...");
				SceneManager.LoadScene(SceneManager.GetActiveScene ().name);
			}
		}
	}

	public void DebugPrint(string message) {
		Debug.Log ("[DEBUG] " + message);
	}
}
