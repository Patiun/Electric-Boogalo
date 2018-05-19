using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Greg Kilmer
 * Function: Controls logic for generating hex based road units
 * Last Updated: 5/18/2018
 */

public class Hex_Road : MonoBehaviour {
	
	public Unit_Island island;
	public GameObject roadPrefab;
	public GameObject buildingPrefab;
	public GameObject bridgeUnitPrefab;
	public float buildingOffset = 0.5f;
	public float buildingChance = 80.00f;
	public float hexSize = 1.0f;
	public GameObject[] neighbors;
	public LayerMask layerMask;

	// Use this for initialization
	void Start () {
		neighbors = new GameObject[6];
		for (int n = 0; n < 6; n++) {
			neighbors [n] = null;
		}
		GatherNeighbors ();
		GenerateMissingNeighbors ();
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


	private void GenerateMissingNeighbors () {
		buildingChance = island.buildingDensity;
		for (int i = 0; i < 6; i++) {
			if (island.canBuild) {
				if (neighbors [i] == null) {
					Vector3 dir = Quaternion.AngleAxis (60 * i, transform.up) * transform.forward;
					if (Random.Range (0, 100) <= buildingChance) {
						GameObject newBuilding = Instantiate (buildingPrefab);
						Vector3 newPos = transform.position + dir.normalized * hexSize;
						newPos.y += buildingOffset;
						newBuilding.transform.position = newPos;
						neighbors [i] = newBuilding;
						island.AddHex (newBuilding);
					} else {
						GameObject newRoad = Instantiate (roadPrefab);
						newRoad.transform.position = transform.position + dir.normalized * hexSize;
						neighbors [i] = newRoad;
						newRoad.GetComponent<Hex_Road> ().island = island;
						island.AddHex (newRoad);
					}
				}
			} else if (neighbors [i] == null) {
				if (Random.Range (0.0f, 100.0f) <= island.bridgeDensity) {
					Vector3 dir = Quaternion.AngleAxis (60 * i, transform.up) * transform.forward;
					GameObject bridgeUnit = Instantiate (bridgeUnitPrefab);
					Vector3 newPos = transform.position + dir.normalized * hexSize;
					bridgeUnit.transform.position = newPos;
					bridgeUnit.transform.rotation = Quaternion.Euler(new Vector3(0,60*i,0));
					neighbors [i] = bridgeUnit.GetComponent<Unit_Bridge> ().startingBridge;
				} else {
					Vector3 dir = Quaternion.AngleAxis (60 * i, transform.up) * transform.forward;
					GameObject newBuilding = Instantiate (buildingPrefab);
					Vector3 newPos = transform.position + dir.normalized * hexSize;
					newPos.y += buildingOffset;
					newBuilding.transform.position = newPos;
					neighbors [i] = newBuilding;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
}
