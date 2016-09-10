using UnityEngine;
using System.Collections;
using System.Linq; // used for Sum of array
 
public class AssignSplatMap : MonoBehaviour {
 
    void Start () {
        assignTextures();
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            assignTextures();
        }
    }

    private void assignTextures()
    {
        Terrain terrain = (Terrain)GetComponent("Terrain");
        TerrainData terrainData = terrain.terrainData;

        float[,,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {
                // Normalise x/y coordinates to range 0-1 
                float n_y = (float)y / (float)terrainData.alphamapHeight;
                float n_x = (float)x / (float)terrainData.alphamapWidth;

                float height = terrainData.GetHeight(Mathf.RoundToInt(n_y * terrainData.heightmapHeight), Mathf.RoundToInt(n_x * terrainData.heightmapWidth));

                //Vector3 normal = terrainData.GetInterpolatedNormal(n_y, n_x);

                // Setup an array to record the mix of texture weights at this point
                float[] splatWeights = new float[terrainData.alphamapLayers];

                // Texture[0] has constant influence
                splatWeights[0] = 0.5f;

                float terrainHeight = terrainData.detailHeight;

                if (height <= (terrainHeight / 3))
                {
                    splatWeights[1] = 0.7f;
                }
                else if (height <= ((terrainHeight * 2) / 3))
                {
                    splatWeights[2] = 0.7f;
                }
                else if (height <= terrainHeight)
                {
                    splatWeights[3] = 0.7f;
                }

                // Sum of all textures weights must add to 1, so calculate normalization factor from sum of weights
                float z = splatWeights.Sum();

                for (int i = 0; i < terrainData.alphamapLayers; i++)
                {
                    splatWeights[i] /= z;
                    splatmapData[x, y, i] = splatWeights[i];
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, splatmapData);
    }
}