﻿using UnityEngine;
using System.Collections;

public class SunOrbit : MonoBehaviour {

    public float speed = 2.5f;

	void Update () {
        transform.RotateAround(Vector3.zero, Vector3.right, Time.deltaTime * speed);
	}

}
