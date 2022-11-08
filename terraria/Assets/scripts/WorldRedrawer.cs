using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorldRedrawer : MonoBehaviour
{

    private int xOffset;
    private int yOffset;
    private int startPosX;
    private int startPosY;
    [SerializeField]
    private BlockSystem blockSys;
    public List<List<GameObject>> drawWorldV2 (List<List<GameObject>> scene, List<List<int>> world, GameObject blockPrefab, int screenSizeX, int screenSizeY)
    {

        startPosX = world[0].Count / 2;
        startPosY = world.Count / 2;
        xOffset = 0;
        yOffset = 0;

        for (int i = 0; i < screenSizeY; i++)
        {

            scene.Add(new List<GameObject>());

            for (int j = 0; j < screenSizeX; j++)
            {

                if (world[i + startPosY - (screenSizeY / 2)][j + startPosX - (screenSizeX / 2)] != -1)
                {
                    GameObject tile = Instantiate(blockPrefab) as GameObject;
                    //method set block sprite decides which sprite block should have
                    this.setBlockSprite(i + startPosY - (screenSizeY / 2), j + startPosX - (screenSizeX / 2), world, tile);
                    tile.transform.position = new Vector2(j - (screenSizeX / (float)2.0), (screenSizeY / (float)2.0) - i);
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

    public List<List<GameObject>> redrawWorldV3 (List<List<GameObject>> scene, List<List<int>> world, GameObject blockPrefab, Vector2 relativePos, int blockSize, int screenSizeX, int screenSizeY, Vector2 actualPos)
    {

        //character moved 100 or more pixels to the right
        if (relativePos.x > 100)
        {

            // n is number of blocks we moved by
            int n = (int)(relativePos.x / blockSize);

            // if error of shift is more than one block we have to fix this error by running algorithm one more time
            if ((actualPos.x - xOffset) > blockSize)
            {

                n++;
                Debug.Log("fixing the error");

            }

            //going through all blocks from top to bottom
            for (int i = 0; i < screenSizeY; i++)
            {

                //adding and deleting needed amount of blocks (n blocks)
                for (int j = 0; j < n; j++)
                {

                    // check if there is a tile on the location we are filling now, if there is, we will add a block
                    if (world[i][(int)(xOffset / blockSize) + startPosX + (screenSizeX / 2) + (j + 1)] != -1)
                    {   

                        GameObject tile = Instantiate(blockPrefab) as GameObject;
                        this.setBlockSprite(i, (int)(xOffset / blockSize) + startPosX + (screenSizeX / 2) + (j + 1), world, tile);
                        tile.transform.position = new Vector2((float)((xOffset / blockSize) + (screenSizeX / 2)) + j, screenSizeY / (float)2.0 - i);
                        scene[i].Add(tile);

                    }

                    // if there is no block at needed position add null instead of a block
                    else
                    {
                   
                        scene[i].Add(null);
                    }

                    // deleting blocks from the left
                    if (scene[i][0] != null)
                    {
                        Destroy(scene[i][0]);
                    }
                    scene[i].RemoveAt(0);

                }
            }

            // change position coordinates
            xOffset = xOffset + (n * blockSize);
        }

        // if character moved 100 blocks to the left (or more)
        else if(relativePos.x < -100)
        {

            // n is number of blocks we moved by
            int n = (int)(relativePos.x / blockSize);

            if (Math.Abs(actualPos.x - xOffset) > blockSize)
            {

                n--;
                Debug.Log("fixing the error");

            }

            for(int i = 0; i < screenSizeY; i++)
            {

                for(int j = 0; j < Math.Abs(n); j++)
                {

                    if (world[i][startPosX + (xOffset / blockSize) - (screenSizeX / 2) - (j + 1)] != -1)
                    {

                        GameObject tile = Instantiate(blockPrefab) as GameObject;
                        this.setBlockSprite(i, startPosX + (xOffset / blockSize) - (screenSizeX / 2) - (j + 1), world, tile);
                        tile.transform.position = new Vector2((xOffset / blockSize) - (screenSizeX / 2) - (j + 1), screenSizeY / (float)2.0 - i);
                        scene[i].Insert(0, tile);

                    }
                    else
                    {
                        scene[i].Insert(0, null);
                    }
                  
                    if (scene[i][scene[i].Count - 1] != null)
                    {
                        Destroy(scene[i][scene[i].Count - 1]);
                    }
                    scene[i].RemoveAt(scene[i].Count - 1);

                }

            }

            xOffset = xOffset + (n * blockSize);

        }

        return scene;

    }    

    private void setBlockSprite(int blockY, int blockX, List<List<int>> world, GameObject tile)
    {

        tile.GetComponent<block>().setSprite(blockSys.getBlock(world[blockY][blockX]).getSprite());

    }

}
