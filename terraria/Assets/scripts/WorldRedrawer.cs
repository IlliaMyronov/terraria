using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorldRedrawer : MonoBehaviour
{

    private int oldOffsetX;
    private int oldOffsetY;
    private int dOffsetX;
    private int dOffsetY;
    private float xLocation;
    private float yLocation;
    private int lastOffsetX;
    private int lastOffsetY;
    public List<List<GameObject>> drawWorld(List<List<int>> world, List<List<GameObject>> scene, int screenSizeY, int screenSizeX, GameObject blockPrefab, int offsetX, int offsetY)
    {
        oldOffsetX = offsetX;
        oldOffsetY = offsetY;
        Debug.Log("initial x offset is " + offsetX);
        for (int i = 0; i < screenSizeY; i++)
        {
            scene.Add(new List<GameObject>());
            for (int j = 0; j < screenSizeX; j++)
            {

                if (world[i + offsetY][j + offsetX] != -1)
                {
                    GameObject tile = Instantiate(blockPrefab) as GameObject;
                    tile.transform.position = new Vector2(j - screenSizeX / (float)2.0, screenSizeY / (float)2.0 - i);
                    scene[i].Add(tile);

                }

                else
                {
                    scene[i].Add(null);
                }

            }

        }

        return scene;

    }


    public List<List<GameObject>> drawWorldV2 (List<List<GameObject>> scene, List<List<GameObject>> world, int screenSizeY, int screenSizeX, int offsetX, int offsetY, GameObject blockPrefab)
    {

        return scene;

    }


    public List<List<GameObject>> redrawWorldV2(List<List<GameObject>> scene, List<List<int>> world, GameObject blockPrefab, int screenSizeX, int screenSizeY, float relativeX, float relativeY, int blockSize)
    {

        if (relativeX > 100)
        {
            
            for(int i = 0; i < screenSizeX; i++)
            {

                // n is number of blocks we moved by
                int n = (int)(relativeX / blockSize);

                for (int j = 0; j < n; j++)
                {

                    // check if there is a tile on the location we are filling now
                    if (world[i][(lastOffsetX / 16) + screenSizeX - n + j] != -1)
                    {
                        // add a block and give it needed coordinates
                        GameObject tile = Instantiate(blockPrefab) as GameObject;
                        tile.transform.position = new Vector2(((float)screenSizeX / 2.0f) - n + j + (relativeX - (n * blockSize)) / (float)blockSize, screenSizeY / (float)2.0 - i);
                        scene[i].Add(tile);

                    }

                    else
                    {
                        // if there is no block at needed position add null instead of a block
                        scene[i].Add(null);
                    }

                    // deleting blocks from the left
                    if (scene[i][0] != null)
                    {
                        Destroy(scene[i][0]);
                    }
                    else
                        scene[i].RemoveAt(0);
                }
            }
        }
        return scene;

    }


    public List<List<GameObject>> redrawWorld(List<List<GameObject>> scene, List<List<int>> world, int screenSizeX, int screenSizeY, int offsetX, int offsetY, GameObject blockPrefab) 
    {
        dOffsetX = offsetX - oldOffsetX;
        dOffsetY = offsetY - oldOffsetY;
        Debug.Log(offsetX + "    " + oldOffsetX + "      " + dOffsetX);

        if(dOffsetX > 5)
        {
            for(int i = 0; i < screenSizeY; i++)
            {

                for(int j = 0; j < Mathf.Abs(dOffsetX); j++)
                {
                    // adding a tile to the end of an array
                    if (world[i + offsetY][oldOffsetX + screenSizeX + j] != -1)
                    {

                        GameObject tile = Instantiate(blockPrefab) as GameObject;
                        tile.transform.position = new Vector2(screenSizeX / 2 - dOffsetX + j, screenSizeY / (float)2.0 - i);
                        scene[i].Add(tile);

                    }

                    else
                    {
                        scene[i].Add(null);
                    }

                    // deleting a tile from the start of an array

                    if (scene[i][0] != null) 
                    {
                        Destroy(scene[i][0]);
                    }
                        
                    scene[i].RemoveAt(0);

                    
                }
            }
        }

        return scene;
    }
    

}
