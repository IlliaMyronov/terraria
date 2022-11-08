using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class WorldGeneration : MonoBehaviour
{
    // world size properties
    private static int numOfBlocksX;
    private static int numOfBlocksY;
    List<List<int>> worldArr = new List<List<int>>();
    [SerializeField]
    int scale;
    private float offSetX, offSetY;

    private void Awake()
    {
        numOfBlocksY = 68;
        numOfBlocksX = 240;
    }
    public void generateWorld()
    {
        offSetX = Random.Range(0f, 99999f);
        offSetY = Random.Range(0f, 99999f);

        for (int i = 0; i < numOfBlocksY; i++)
        {
            worldArr.Add(new List<int>());
            for (int j = 0; j < numOfBlocksX; j++)
            {

                // if perlin noise returns value that is greater than 0.5 there should be no block
                if (generatePerlinValue(j, i) < 0.5)
                {
                    // since there is a block, check what kind of a block should there be
                    // if the block is on the very top it should be a grass
                    if (i != 0)
                    {
                        // if there is a block above than the block has to be dirt, else grass
                        if (worldArr[i - 1][j] != -1)
                        {
                            worldArr[i].Add(0);
                        }
                        else
                        {
                            worldArr[i].Add(1);
                        }
                    }
                    else
                    {
                        worldArr[i].Add(1);
                    }

                }

                else
                {

                    worldArr[i].Add(-1);

                }

            }
        }

        // choose random amount of gold nodes in the world
        int n = Random.Range(5, 10);

        // loop that runs for n number of nodes
        for (int i = 0; i < n; i++)
        {

            Vector2 goldNodeCenter = new Vector2(Random.Range(0, numOfBlocksX), Random.Range(0, numOfBlocksY));
            int goldNodeDiameter = Random.Range(3, 9);
            // goldNodeMiddle is the number of blocks to the left of the middle block
            int goldNodeMiddle = (int)Math.Ceiling((float)goldNodeDiameter / 2.0f) - 1;
            Debug.Log("mid is " + goldNodeMiddle);
            // loop that runs through columns for node
            for (int j = 0; j < goldNodeDiameter; j++)
            {

                // loop that runs number of blocks there are in this row
                int rowLength = (int)(goldNodeDiameter - (2 * Math.Abs(j - goldNodeMiddle)));
                Debug.Log("row length is " + rowLength + " in row number " + j);
                for (int l = 0; l < rowLength; l++)
                {
                    int x = (int)((goldNodeCenter.x - goldNodeMiddle) + ((goldNodeDiameter - rowLength) / 2) + l);
                    int y = (int)(goldNodeCenter.y - (Math.Ceiling((float)goldNodeDiameter / 2.0f) - 1) + j);
                    if (y < numOfBlocksY && y > 0 && x < numOfBlocksX && x > 0)
                    {
                        if (worldArr[y][x] != -1)
                        {
                            worldArr[y][x] = 2;
                        }
                    }
                        
                }

            }

        }

        Debug.Log("Done creating world");

    }

    private float generatePerlinValue(int j, int i)
    {
        return Mathf.PerlinNoise((((float)j * 16 / 1920) * scale + offSetX), (((float)i * 16 / 1080) * scale + offSetY));
    }

    public int getWorldSize (bool isX)
    {
        if(isX)
            return numOfBlocksX;
        return numOfBlocksY;
    }

    public List<List<int>> getWorld()
    {
        return worldArr;
    }

}