using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Greg Kilmer
 * Function: Controls logic for hex tiles for bridges
 * Last Updated: 5/19/2018
 */

public class Hex_Bridge : MonoBehaviour {

	public GameObject previouseTile,nextTile;
	public GameObject bridgePrefab,islandPrefab,buildingPrefab;
	public LayerMask layerMask;
	public Unit_Bridge unit;
	public float hexSize;

	//DEBUG
	public float buildingEdgeSize;

	// Use this for initialization
	void Start () {
		CheckNeighbors ();
		GenerateBridge ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void CheckNeighbors() {
		RaycastHit hit;
		//Check forward
		if (Physics.Raycast (transform.position, transform.forward, out hit, hexSize, layerMask.value)) {
			nextTile = hit.collider.gameObject;
		} else {
			nextTile = null;
		}
		//Check behind
		if (Physics.Raycast (transform.position, transform.forward * -1, out hit, hexSize, layerMask.value)) {
			previouseTile = hit.collider.gameObject;
		} else {
			previouseTile = null;
		}
	}

	private void GenerateBridge() {
		if (unit.canBuild) { //TODO change to use a bridge unit handler
			if (nextTile == null) {
				GameObject newBridge = Instantiate (bridgePrefab);
				Vector3 newPos = transform.position + transform.forward * hexSize;
				newBridge.transform.position = newPos;
				newBridge.transform.rotation = transform.rotation;
				nextTile = newBridge;
				newBridge.GetComponent<Hex_Bridge> ().unit = unit;
				unit.AddBridgePiece (newBridge);
			}
		} else {
			EndBridge ();
		}
	}

	private void EndBridge() {
		for (int i = 0; i < buildingEdgeSize; i++) {
			//Generate Side Buildings
			GameObject newBuilding = Instantiate (buildingPrefab);
			Vector3 newPos = transform.position + (Quaternion.AngleAxis (60, transform.up) * (transform.forward * hexSize * (i+1)));
			newBuilding.transform.position = newPos;
			GameObject newBuilding2 = Instantiate (buildingPrefab);
			newPos = transform.position + (Quaternion.AngleAxis (-60, transform.up) * (transform.forward * hexSize * (i+1)));
			newBuilding2.transform.position = newPos;
		}
		//Generate Starting Road
		GameObject newIsland = Instantiate (islandPrefab);
		Vector3 newPos2 = transform.position + transform.forward * hexSize;
		newIsland.transform.position = newPos2;
		nextTile = newIsland.GetComponentInChildren<Hex_Road>().gameObject;
	}
}
