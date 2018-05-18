using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Biome {DEFAULT,TOKYO};

public class Hex_Bridge : MonoBehaviour {

	public Biome startingBiome,endingBiome; //Might not use
	public GameObject previouseTile,nextTile;
	public GameObject bridgePrefab,roadPrefab,buildingPrefab;
	public LayerMask layerMask;
	public float hexSize;

	//DEBUG
	public int id;
	public int maxNum;
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
		if (id < maxNum) { //TODO change to use a bridge unit handler
			if (nextTile == null) {
				GameObject newBridge = Instantiate (bridgePrefab);
				Vector3 newPos = transform.position + transform.forward * hexSize;
				newBridge.transform.position = newPos;
				nextTile = newBridge;
				newBridge.GetComponent<Hex_Bridge> ().id = id + 1;
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
		GameObject newRoad = Instantiate (roadPrefab);
		Vector3 newPos2 = transform.position + transform.forward * hexSize;
		newRoad.transform.position = newPos2;
		nextTile = newRoad;
	}
}
