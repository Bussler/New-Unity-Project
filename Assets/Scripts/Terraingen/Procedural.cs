using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Procedural : MonoBehaviour{


	public int width = 1024;
	public int height = 1024;
	public int depth = 200;

	private float offsetX = 100f;
	private float offsetY = 100f;
    //private TerrainTreePLacement placement;

    public float scale = 2f;
	private TerrainData data;
	void Start() {
        // width = Global.size;
        //height = Global.size;

        offsetX = Random.Range(0f, 999f);
        offsetY = Random.Range(0f, 999f);

        //placement = this.GetComponent<TerrainTreePLacement>();
       // placement.Place();
        

    }

    public void OnDestroy()
    {

        //generate and safe terrain and colormaps for the next run

        Terrain terrain = this.GetComponent<Terrain>();
        data = terrain.terrainData;
        data.heightmapResolution = width;
        data.size = new Vector3(width, height, depth);
        terrain.terrainData = GenerateTerrain(terrain.terrainData);

        Blending blender = new Blending();//init blender
        Texture2D grass = Resources.Load("gras15") as Texture2D;//1
        Texture2D wood = Resources.Load("Holz-34") as Texture2D;//2
        Texture2D mud = Resources.Load("mud02") as Texture2D;//3
        Texture2D stone = Resources.Load("rock8") as Texture2D;//4

        Color[,] colorArray = blender.generateBlend(grass, wood, mud, stone, terrain.terrainData, width, height);

        //use this colorArray to calculate a texture and apply it to the terrain
        Texture2D image = new Texture2D(width, height);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                image.SetPixel(x, y, colorArray[x, y]);
            }
        }

        //save pic
        var Bytes = image.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/Resources/TerrainColorMap.png", Bytes);
    }

    TerrainData GenerateTerrain(TerrainData terrainData) {
		
		//terrainData.heightmapResolution = data.heightmapWidth + 1;
		terrainData.size = (new Vector3(width, depth, height));
		terrainData.SetHeights(0, 0, GenerateHeights());
		return terrainData;
	}

	float[,] GenerateHeights() {
		float[,] heights = new float[height, width];
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				heights[i, j] = CalculateHeight(i, j);
			}
		}

		return heights;
	}

	float CalculateHeight(float x, float y) {
		float xCoord = x / height * scale + offsetX;
		float yCoord = y / width * scale + offsetY;

		return Mathf.PerlinNoise(xCoord, yCoord);
	}
	
}
