using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Greg Kilmer
 * Function: Controls logic for the entire map
 * Last Updated: 5/22/2018
 */

public class Hex_MapController : MonoBehaviour {

	public int numIslands = 0;
	public int maxIslands = 8;
	public bool canBuildIslands = true;
	public List<GameObject> islands;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void AddIsland(GameObject island) {
		if (!islands.Contains (island)) {
			numIslands++;
			islands.Add (island);
			island.name = "Island " + numIslands + " ("+island.GetComponent<Unit_Island>().biome+")";
			if (numIslands >= maxIslands) {
				canBuildIslands = false;
			}
		}
	}
}
