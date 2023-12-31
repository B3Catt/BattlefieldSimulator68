﻿using UnityEngine;
using System.Collections;

public class csMouseOrbit : MonoBehaviour {

	public Transform Target;
	public float distance;

	public float xSpeed = 250.0f;
	public float ySpeed = 120.0f;

	public float yMinLimit = -20.0f;
	public float yMaxLimit = 80.0f;

	private float x = 0.0f;
	private float y = 0.0f;

	// Use this for initialization
	void Start () 
	{
		Vector3 angles = transform.eulerAngles;
		x = angles.x;
		y = angles.y;

		if(this.GetComponent<Rigidbody>() == true)
			GetComponent<Rigidbody>().freezeRotation = true;
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
		if(Input.GetKey(KeyCode.W))
		{
			if(distance >= 2)
				distance -= 3.0f * Time.deltaTime;
		}
		else if(Input.GetKey(KeyCode.S))
		{
			if(distance <= 30)
				distance += 3.0f * Time.deltaTime;
		}

		if (Target)
		{
			x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
			y += Input.GetAxis("Mouse Y") * ySpeed * 0.05f;

			y = ClampAngle(y,yMinLimit,yMaxLimit);

			Quaternion rotation = Quaternion.Euler(y,x,0);
			Vector3 position = rotation * new Vector3(0,0,-distance)+Target.position;

			transform.rotation = rotation;
			transform.position = position;
		}
	}

	float ClampAngle(float ag, float min, float max)
	{
		if (ag < -360)
			ag += 360;
		if (ag > 360)
			ag -= 360;
		return Mathf.Clamp (ag, min, max);
	}
}
