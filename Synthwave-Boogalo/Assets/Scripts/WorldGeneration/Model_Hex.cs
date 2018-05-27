using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Hex : MonoBehaviour {

	public GameObject model_a,model_b;

	public GameObject GetRoadModel(Hex_Road.RoadType roadType) {
		switch (roadType) {
		case Hex_Road.RoadType.FORK:
			return model_a;
			break;
		default:
			return model_b;
			break;
		}
	}
}