using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Terrain))]
public class TerrainGenerator : MonoBehaviour {

    public float width = 1000;
    public float length = 1000;
    public float height = 600;

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
    }

    void Generate() {
        DiamondSquareHeightMap gen = new DiamondSquareHeightMap(7);

        terrain.terrainData.heightmapResolution = gen.GetSize();
        terrain.terrainData.SetHeights(0, 0, gen.GetData());
        terrain.terrainData.size = new Vector3(width, height, length);
	}

}
