using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Greg Kilmer
 * Function: Controls logic for a game island
 * Last Updated: 5/22/2018
 */

public class Unit_Bridge : MonoBehaviour {

	public Unit_Island.Biome startingBiome, endingBiome;
	public int currentSize;
	public int maxLength;
	public bool canBuild = true;
	public GameObject bridgePrefab,startingBridge;
	public List<GameObject> bridgePieces;

	// Use this for initialization
	void Start () {
		bridgePieces = new List<GameObject> ();
		GameObject firstBridge = Instantiate (bridgePrefab);
		bridgePrefab.transform.position = transform.position;
		bridgePrefab.transform.rotation = transform.rotation;
		startingBridge = firstBridge;
		startingBridge.GetComponent<Hex_Bridge> ().unit = this;
		AddBridgePiece (startingBridge);
	}

	public void AddBridgePiece(GameObject bridgePiece) {
		if (!bridgePieces.Contains (bridgePiece)) {
			currentSize++;
			bridgePieces.Add (bridgePiece);
			if (bridgePiece.GetComponent<Hex_Bridge> ().unit != this) {
				bridgePiece.GetComponent<Hex_Bridge> ().unit = this;
			}
			if (canBuild && currentSize >= maxLength) {
				canBuild = false;
			}
			bridgePiece.name = "Bridge_Piece " + currentSize;
			bridgePiece.transform.parent = transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (canBuild && currentSize >= maxLength) {
			canBuild = false;
		}
	}
}
