using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex_MapController : MonoBehaviour {

	public int numHexes = 0;
	public int maxHexes = 100;
	public bool canAddHexes = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (canAddHexes && numHexes >= maxHexes) {
			canAddHexes = false;
		}
	}
}
