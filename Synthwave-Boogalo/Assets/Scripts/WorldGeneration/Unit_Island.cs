using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Greg Kilmer
 * Function: Controls logic for a game bridge
 * Last Updated: 5/22/2018
 */

public class Unit_Island : MonoBehaviour {

	public enum Biome {DEFAULT,TOKYO};
	public Biome biome;
	public int maxSize;
	public int currentSize;
	public bool canBuild = true;
	public float buildingDensity;//Depricated
	public float bridgeDensity; //Depricated
	public int maxBridges;
	public int curBridges;
	public bool builtBuildings;
	public int buildingCount;
	public List<GameObject> hexes;
	public List<GameObject> bridges;
	public List<GameObject> buildings;
	public GameObject startingRoad;
	public Hex_MapController map;

	private GameObject allRoads, allBridges, allBuildings;

	// Use this for initialization
	void Start () {
		map = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Hex_MapController> ();
		map.AddIsland (this.gameObject);
		startingRoad.GetComponent<Hex_Road> ().island = this;
		allRoads = new GameObject ("Roads");
		allBridges = new GameObject ("Bridges");
		allBuildings = new GameObject ("Buildings");
		allRoads.transform.parent = transform;
		allBridges.transform.parent = transform;
		allBuildings.transform.parent = transform;
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
				//BuildBuildings (); //TODO find a better time to do this
			}
			hex.transform.parent = allRoads.transform;
			hex.name = "Road " + currentSize + " ("+hex.GetComponent<Hex_Road>().roadType+")";
		}
	}

	public void AddBridge(GameObject bridge) {
		if (!bridges.Contains (bridge)) {
			curBridges++;
			bridges.Add(bridge);
			if (curBridges >= maxBridges && !builtBuildings) {
				BuildBuildings();
			}
			bridge.transform.parent = allBridges.transform;
			bridge.name = "Bridge_Unit " + curBridges;
		}
	}

	public void AddBuilding(GameObject building) {
		if (!buildings.Contains (building)) {
			buildingCount++;
			building.name = "Building " + buildingCount;
			buildings.Add (building);
			building.transform.parent = allBuildings.transform;
		}
	}

	public void BuildBuildings() {
		builtBuildings = true;
		foreach (GameObject hex in hexes) {
			hex.GetComponent<Hex_Road> ().GenerateMissingNeighbors ();
		}
	}

	public bool CanBuildBridge() {
		return (map.canBuildIslands && curBridges < maxBridges);
	}
}
