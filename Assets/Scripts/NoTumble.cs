using UnityEngine;
using System.Collections;

public class NoTumble : MonoBehaviour {

    public float drag = 0.5f;

    Rigidbody rb;

	void Start () {
        rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate () {
        rb.velocity = drag * rb.velocity;
        rb.angularVelocity = drag * rb.angularVelocity;
	}

}
