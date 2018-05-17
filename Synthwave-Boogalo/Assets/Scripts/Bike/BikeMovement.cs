using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Greg Kilmer
 * Function: Contols the movement of a bike object. Intended for use with both player and enemy bikes.
 * Last Updated: 5/17/2018
 */

public class BikeMovement : MonoBehaviour {
	public GameObject bike;
	public float accelerationRate;
	public float decelerationRate;
	public float topSpeed;

	public float leanRate;

	public float turnRate;
	//public float maxTurn;

	public Vector3 curVelocity;

	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		curVelocity = rb.velocity;
		//Curb Velocity to topSpeed
		if (rb.velocity.magnitude >= topSpeed) {
			rb.velocity = rb.velocity.normalized* topSpeed;
		}

		//DEBUG
		Debug.DrawRay(transform.position,transform.right*10,Color.red);
	}

	//Increase speed in forward direction
	public void Accelerate() {
		rb.AddForce (transform.forward * accelerationRate);
	}

	//Decrease speed in the forward direction
	public void Decelerate() {
		rb.AddForce (transform.forward * -decelerationRate);
	}

	//Turn left using handle bars
	public void TurnLeft() {
		float turnAmt = -turnRate * rb.velocity.magnitude;
		transform.RotateAround (transform.position, transform.up, turnAmt);
		rb.velocity = Quaternion.AngleAxis (turnAmt, transform.up) * rb.velocity;
	}

	//Turn right using handle bars
	public void TurnRight() {
		float turnAmt = turnRate * rb.velocity.magnitude;
		transform.RotateAround (transform.position, transform.up, turnAmt);
		rb.velocity = Quaternion.AngleAxis (turnAmt, transform.up) * rb.velocity;
	}

	//Slides bike left by leaning left
	public void LeanLeft(float percentageOfLeanRate) {
		//TODO revamp to use velocity scaling only in the forward direction
		rb.AddForce ((transform.right + transform.up * Mathf.Tan (transform.rotation.eulerAngles.z)) * -leanRate * percentageOfLeanRate);
	}

	//Slides bike right by learning right
	public void LeanRight(float percentageOfLeanRate) {
		//TODO revamp to use velocity scaling only in the forward direction
		rb.AddForce ((transform.right + transform.up * Mathf.Tan (transform.rotation.eulerAngles.z)) * leanRate * percentageOfLeanRate);
	}
}
