using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Greg Kilmer
 * Function: Controls logic for generating hex based road units
 * Last Updated: 5/27/2018
 */

public class Hex_Road : MonoBehaviour {

	public enum RoadType {STRAIGHT,ELBOW_L,ELBOW_R,FORK};
	public bool usingDensity = true;
	public int straightDensity, elbowLDensity, elbowRDensity, forkDensity;
	private RoadType[] types = new RoadType[]{RoadType.STRAIGHT,RoadType.STRAIGHT,RoadType.STRAIGHT,RoadType.STRAIGHT,RoadType.ELBOW_L,RoadType.ELBOW_R,RoadType.FORK,RoadType.FORK};

	public RoadType roadType;
	public bool forceRoadType;
	public Unit_Island island;
	public GameObject roadPrefab;
	public GameObject buildingPrefab;
	public GameObject bridgeUnitPrefab;
	public float buildingOffset = 0.5f;
	public float buildingChance = 80.00f; //DEPRECATED
	public float hexSize = 1.0f;
	public bool hasBridge;
	public GameObject[] neighbors;
	public LayerMask layerMask;
	public GameObject model;

	// Use this for initialization
	void Start () {
		if (usingDensity) {
			types = new RoadType[straightDensity + elbowRDensity + elbowLDensity + forkDensity];
			int count = 0;
			for (int i = count; i < straightDensity; i++) {
				types [i] = RoadType.STRAIGHT;
				count++;
			}
			for (int i = count; i < straightDensity + elbowRDensity; i++) {
				types [i] = RoadType.ELBOW_R;
				count++;
			}
			for (int i = count; i < straightDensity + elbowRDensity + elbowLDensity; i++) {
				types [i] = RoadType.ELBOW_L;
				count++;
			}
			for (int i = count; i < straightDensity + elbowRDensity + elbowLDensity + forkDensity; i++) {
				types [i] = RoadType.FORK;
				count++;
			}
		}

		if (!forceRoadType) {
			int ind = Random.Range (0, types.Length - 1);
			roadType = types [ind];
		}
		neighbors = new GameObject[6];
		for (int n = 0; n < 6; n++) {
			neighbors [n] = null;
		}
		if (model == null) {
			model = Instantiate (GetComponent<Model_Hex> ().GetRoadModel (roadType));
			model.transform.parent = transform;
			model.transform.position = transform.position;
			model.transform.rotation = transform.rotation;
		}
		GatherNeighbors ();
		//GenerateMissingNeighbors ();
		GenerateNeighborsByRoadType();
	}

	private void GatherNeighbors() {
		for (int i = 0; i < 6; i++) {
			RaycastHit hit;
			Vector3 dir = Quaternion.AngleAxis (60 * i, transform.up) * transform.forward;
			//Debug.DrawRay (transform.position, dir * hexSize, Color.red);
			Physics.Raycast (transform.position, dir, out hit, hexSize, layerMask.value);
			if (hit.collider != null) {
				//Debug.Log (hit.collider.tag);
				neighbors [i] = hit.collider.gameObject;
			}
		}
	}


	public void GenerateMissingNeighbors () {
		GatherNeighbors ();
		buildingChance = island.buildingDensity; //DEPRECATED
		for (int i = 0; i < 6; i++) {
			if (neighbors [i] == null) {
				GenerateBuilding (i);
			}
		}
	}

	public void GenerateNeighborsByRoadType() {
		switch (roadType) {
		case RoadType.STRAIGHT:
			if (neighbors [0] == null) {
				GenerateRoad (0);
			}
			if (neighbors [3] == null) {
				GenerateRoad (3);
			}
			break;
		case RoadType.ELBOW_L:
			if (neighbors [5] == null) {
				GenerateRoad (5);
			}
			if (neighbors [3] == null) {
				GenerateRoad (3);
			}
			break;
		case RoadType.ELBOW_R:
			if (neighbors [1] == null) {
				GenerateRoad (1);
			}
			if (neighbors [3] == null) {
				GenerateRoad (3);
			}
			break;
		case RoadType.FORK:
			if (neighbors [1] == null) {
				GenerateRoad (1);
			}
			if (neighbors [3] == null) {
				GenerateRoad (3);
			}
			if (neighbors [5] == null) {
				GenerateRoad (5);
			}
			break;
		}
		GatherNeighbors ();
	}

	private void GenerateBuilding(int neighborID) {
		//if (island.canBuild) {
			Vector3 dir = Quaternion.AngleAxis (60 * neighborID, transform.up) * transform.forward;
			GameObject newBuilding = Instantiate (buildingPrefab);
			Vector3 newPos = transform.position + dir.normalized * hexSize;
			newPos.y += buildingOffset;
			newBuilding.transform.position = newPos;
			neighbors [neighborID] = newBuilding;
			island.AddBuilding (newBuilding);
			//island.AddHex (newBuilding); //TODO switch to storing buildings
		//}
	}

	private void GenerateBridge(int neighborID) {
		if (island.CanBuildBridge() && !hasBridge) {
			Vector3 dir = Quaternion.AngleAxis (60 * neighborID, transform.up) * transform.forward;
			GameObject bridgeUnit = Instantiate (bridgeUnitPrefab);
			Vector3 newPos = transform.position + dir.normalized * hexSize;
			bridgeUnit.transform.position = newPos;
			bridgeUnit.transform.rotation = Quaternion.LookRotation (dir);
			neighbors [neighborID] = bridgeUnit.GetComponent<Unit_Bridge> ().startingBridge;
			bridgeUnit.GetComponent<Unit_Bridge> ().oiginatingHex = this.gameObject;
			//island.AddBridge (bridgeUnit);
			hasBridge = true;
		}
	}

	private void GenerateRoad(int neighborID) {
		if (island.canBuild) {
			Vector3 dir = Quaternion.AngleAxis (60 * neighborID, transform.up) * transform.forward;
			GameObject newRoad = Instantiate (roadPrefab);
			newRoad.transform.position = transform.position + dir.normalized * hexSize;
			newRoad.transform.rotation = Quaternion.LookRotation (dir);
			neighbors [neighborID] = newRoad;
			newRoad.GetComponent<Hex_Road> ().island = island;
			island.AddHex (newRoad);
		} else {
			GenerateBridge (neighborID);
		}
	}

	private void ChooseModel() {
		//TODO fill this in
	}
	
	// Update is called once per frame
	void Update () {
	}
}
