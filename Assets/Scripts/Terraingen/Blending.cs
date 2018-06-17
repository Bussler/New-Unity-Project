using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blending {

    private float al1 = 0.0f;
    private float al2 = 0.0f;
    private float al3 = 0.0f;
    private float al4 = 0.0f;
	

    public Color[,] generateBlend(Texture2D image1, Texture2D image2, Texture2D image3, Texture2D image4,TerrainData terrainData,int width,int height)
    {
        Color[,] colorArray = new Color[width, height];
        float[,] terrainheights = terrainData.GetHeights(0,0,width,height);

        Vector3[,] normals = generateNomals(terrainheights,width,height);

        for (int y=0;y<height;y++)
        {
            for (int x=0;x<width;x++)
            {

                calculateAlphas(terrainheights[x,y],1.0f-normals[x,y].z);
                //Debug.Log("al1: " + al1+" al2: "+al2+" al3: "+al3+" al4: "+al4);

                Color im1C = image1.GetPixel(x%image1.width, y%image1.height);
                Color im2C = image2.GetPixel(x % image2.width, y % image2.height);
                Color im3C = image3.GetPixel(x % image3.width, y % image3.height);
                Color im4C = image4.GetPixel(x % image4.width, y % image4.height);

                Color result=new Color();
                result.r = al4 * im4C.r + (1.0f - al4) * (al3 * im3C.r + (1.0f - al3) * (al2 * im2C.r + (1.0f - al2) * 0.4f * im1C.r));
                result.g = al4 * im4C.g + (1.0f - al4) * (al3 * im3C.g + (1.0f - al3) * (al2 * im2C.g + (1.0f - al2) * 0.4f * im1C.g));
                result.b = al4 * im4C.b + (1.0f - al4) * (al3 * im3C.b + (1.0f - al3) * (al2 * im2C.b + (1.0f - al2) * 0.4f * im1C.b));

                //Debug.Log("Color: x: "+x+" y: "+y+" rgb: "+result.r+" "+result.g+" "+result.g);

                colorArray[x, y] = result;

            }
        }

        return colorArray;
    }

    private void calculateAlphas(float height, float slope)
    {
        //Debug.Log("Height: " + height + " slope: " + slope);
        al4 = height * slope*0.8f;
        al3 = height/2.0f;
        al2 = (height * slope)*1.2f;
    }

    private Vector3[,] generateNomals(float[,] heights, int width, int height)
    {
        Vector3[,] normals = new Vector3[width, height];

        for (int y=0;y<height;y++)
        {
            for (int x=0;x<width;x++)
            {
                float x_dv = 0.0f;
                float y_dv = 0.0f;
                float z_dv = 1.0f;

                if (x == 0)
                {
                    x_dv = (heights[x+1,y] - heights[x,y]) / 2;
                }
                else if (x == width - 1)
                {
                    x_dv = (heights[x,y] - heights[x-1,y]) / 2;
                }
                else
                {
                    x_dv = (heights[x+1,y] - heights[x-1,y]) / 2;
                }

                if (y == 0)
                {
                    y_dv = (heights[x,y+1] - heights[x,y]) / 2;
                }
                else if (y == height - 1)
                {
                    y_dv = (heights[x,y] - heights[x,y-1]) / 2;
                }
                else
                {
                    y_dv = (heights[x,y+1] - heights[x,y-1]) / 2;
                }

                x_dv *= width;
                y_dv *= height;
                float normal_length = Mathf.Sqrt((x_dv * x_dv) + (y_dv * y_dv) + (z_dv * z_dv));//lenght
                Vector3 temp = new Vector3(((-x_dv) / normal_length), ((-y_dv) / normal_length), z_dv / normal_length);//normalize
                normals[x, y] = temp;
            }
        }

        return normals;
    }
}
