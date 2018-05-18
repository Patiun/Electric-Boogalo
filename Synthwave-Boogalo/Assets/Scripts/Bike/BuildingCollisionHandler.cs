using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCollisionHandler : MonoBehaviour {

	public List<string> dangerousTags;

	void OnTriggerEnter(Collider col) {
		Debug.Log (col.gameObject.tag);
		if (dangerousTags.Contains(col.gameObject.tag)) {
			Debug.Log ("Crashed into " + col.gameObject.tag);
		}
	}
}
