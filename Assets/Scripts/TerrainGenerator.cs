using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Terrain))]
public class TerrainGenerator : MonoBehaviour {

    public int diamondSquareRecursions = 7;
    public float width = 1000;
    public float length = 1000;
    public float height = 600;

    public float waterLevel = 0.75f;

    public GameObject waterPlane;
    public SunOrbit sun;

    DiamondSquareHeightMap dmap;
    Terrain terrain;
    float averageHeight;

    void Start() {
        terrain = GetComponent<Terrain>();
        transform.position = new Vector3(-width / 2, 0, -length / 2);

        Generate();
    }

    void Update() {
        if (Input.GetKeyDown("space")) {
            Generate();
        }

        if (dmap!=null) {
            // Pass updated values to shader
            terrain.materialTemplate.SetFloat("_AverageHeight", averageHeight);
            terrain.materialTemplate.SetFloat("_WaterLevel", waterLevel);
            terrain.materialTemplate.SetColor("_SunColour", sun.color);
            terrain.materialTemplate.SetVector("_SunPosition", sun.GetWorldPosition());
        }
    }

    void Generate() {
        dmap = new DiamondSquareHeightMap(diamondSquareRecursions);
        averageHeight = dmap.GetAverageHeight() * height;

        terrain.terrainData.heightmapResolution = dmap.GetSize();
        terrain.terrainData.SetHeights(0, 0, dmap.GetData());
        terrain.terrainData.size = new Vector3(width, height, length);

        waterPlane.transform.localScale = new Vector3(width / 10, 1, length / 10);

        if (waterPlane) {
            float newWaterPlaneY = averageHeight * waterLevel;
            waterPlane.transform.position = new Vector3(
                waterPlane.transform.position.x,
                newWaterPlaneY,
                waterPlane.transform.position.z
            );
        }
	}
    
}
