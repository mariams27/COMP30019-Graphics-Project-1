using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Terrain))]
public class TerrainGenerator : MonoBehaviour {

    public float width = 1000;
    public float length = 1000;
    public float height = 600;

    public float waterLevel = 0.75f;
    public GameObject waterPlane;
    public DiamondSquareHeightMap dmap;

    Terrain terrain;

    void Start() {
        terrain = GetComponent<Terrain>();

        transform.position = new Vector3(-width / 2, 0, -length / 2);

        

        Generate();
    }

    void Update() {
        if (Input.GetKeyDown("space")) {
            Generate();
        }

        // Get renderer component (in order to pass params to shader)
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();

        if (dmap!=null)
        {
            // Pass updated light positions to shader
            renderer.material.SetFloat("_AverageHeight", this.dmap.GetAverageHeight());
            renderer.material.SetFloat("_TerrainHeight", this.height);
        }
    }

    void Generate() {
        dmap = new DiamondSquareHeightMap(7);
        terrain.terrainData.heightmapResolution = dmap.GetSize();
        terrain.terrainData.SetHeights(0, 0, dmap.GetData());
        terrain.terrainData.size = new Vector3(width, height, length);

        if (waterPlane) {
            float newWaterPlaneY = dmap.GetAverageHeight() * height * waterLevel;
            waterPlane.transform.position = new Vector3(
                waterPlane.transform.position.x,
                newWaterPlaneY,
                waterPlane.transform.position.z
            );
        }
	}

    public float getMapHeight()
    {
        return dmap.GetAverageHeight();
    }

}
