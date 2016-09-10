using UnityEngine;
using System.Collections;

/***
 * Flight Simulator style camera allowing free movement in the x and z axis and rotation in all 3 axis.
 * 
 * Uses unity axises LeftRight, ForwardBackward, Pitch, Yaw and Roll.
 */
public class FlightSimulatorCameraController : MonoBehaviour {

    // move/rotate speed factors
    public float forwardBackwardSpeed = 100f;
    public float leftRightMoveSpeed = 80f;
    public float pitchSpeed = 50f;
    public float yawSpeed = 50f;
    public float rollSpeed = 50f;

    void FixedUpdate() {
        float moveSpeedModifier = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;

        transform.Translate(Time.deltaTime * new Vector3(
            Input.GetAxis("LeftRight") * leftRightMoveSpeed * moveSpeedModifier,
            0,
            Input.GetAxis("ForwardBackward") * forwardBackwardSpeed * moveSpeedModifier
        ));
        transform.Rotate(Time.deltaTime * new Vector3(
            Input.GetAxis("Pitch") * pitchSpeed,
            Input.GetAxis("Yaw") * yawSpeed,
            Input.GetAxis("Roll") * rollSpeed * moveSpeedModifier
        ));
    }

}
