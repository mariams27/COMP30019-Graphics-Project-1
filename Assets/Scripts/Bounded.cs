using UnityEngine;
using System.Collections;

public class Bounded : MonoBehaviour {

    public bool boundX = true;
    public float minX = -500;
    public float maxX = 500;

    public bool boundY = false;
    public float minY = -500;
    public float maxY = 500;

    public bool boundZ = true;
    public float minZ = -500;
    public float maxZ = 500;

    void FixedUpdate() {
        Vector3 pos = transform.position;
        pos.x = boundX ? Mathf.Clamp(pos.x, minX, maxX) : pos.x;
        pos.y = boundY ? Mathf.Clamp(pos.y, minY, maxY) : pos.y;
        pos.z = boundZ ? Mathf.Clamp(pos.z, minZ, maxZ) : pos.z;
        transform.position = pos;
    }

}
