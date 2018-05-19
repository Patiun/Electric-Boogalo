using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Greg Kilmer
 * Function: Controls logic for a game bridge
 * Last Updated: 5/19/2018
 */

public class Unit_Island : MonoBehaviour {

	public enum Biome {DEFAULT,TOKYO};
	public Biome biome;
	public int maxSize;
	public int currentSize;
	public bool canBuild = true;
	public float buildingDensity;
	public float bridgeDensity;
	public List<GameObject> hexes;
	public GameObject startingRoad;
	public Hex_MapController map;

	// Use this for initialization
	void Start () {
		map = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Hex_MapController> ();
		map.AddIsland (this.gameObject);
		startingRoad.GetComponent<Hex_Road> ().island = this;
		AddHex (startingRoad);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddHex(GameObject hex) {
		if (!hexes.Contains (hex)) {
			currentSize++;
			hexes.Add (hex);
			if (hex.tag == "Road") {
				hex.GetComponent<Hex_Road> ().island = this.gameObject.GetComponent<Unit_Island> ();
			}
			if (canBuild && currentSize >= maxSize) {
				canBuild = false;
			}
		}
	}

	public bool CanBuildBridge() {
		return map.canBuildIslands;
	}
}
