using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Greg Kilmer
 * Function: Controls logic for generating hex based road units
 * Last Updated: 5/17/2018
 */

public class Hex_Road : MonoBehaviour {

	public GameObject roadPrefab;
	public GameObject buildingPrefab;
	public float buildingChance = 80.00f;
	public float size = 1.0f;
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
			Debug.DrawRay (transform.position, dir * size, Color.red);
			Physics.Raycast (transform.position, dir, out hit, size, layerMask.value);
			if (hit.collider != null) {
				Debug.Log (hit.collider.tag);
				neighbors [i] = hit.collider.gameObject;
			}
		}
	}


	private void GenerateMissingNeighbors () {
		for (int i = 0; i < 6; i++) {
			if (neighbors [i] == null) {
				Vector3 dir = Quaternion.AngleAxis (60 * i, transform.up) * transform.forward;
				if (Random.Range (0, 100) <= buildingChance) {
					GameObject newBuilding = Instantiate (buildingPrefab);
					newBuilding.transform.position = transform.position + dir.normalized*size;
					neighbors [i] = newBuilding;
				} else {
					GameObject newRoad = Instantiate (roadPrefab);
					newRoad.transform.position = transform.position + dir.normalized*size;
					neighbors [i] = newRoad;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
}
