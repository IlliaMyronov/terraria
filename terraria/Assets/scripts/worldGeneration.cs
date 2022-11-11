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
    int caveScale;
    [SerializeField]
    int groundScale;
    private float offSetX, offSetY;
    int caveStart = 60;
    int maxHillSize = 20;
    int skySize = 30;
    int caveSize = 60;

    private void Awake()
    {
        numOfBlocksY = caveStart + caveSize;
        numOfBlocksX = 240;
    }
    public void generateWorld()
    {
        offSetX = Random.Range(0f, 99999f);
        offSetY = Random.Range(0f, 99999f);

        for(int i = 0; i < numOfBlocksY; i++)
        {
            worldArr.Add(new List<int>());

            for (int j = 0; j < numOfBlocksX; j++)
            {
                
                if(i > skySize)
                {

                    int caveEdge = (int)(maxHillSize - ((2 * maxHillSize) * generatePerlinValue(j, caveStart, caveScale)));

                    if (i < (caveStart - caveEdge))
                    {

                        worldArr[i].Add(0);

                    }
                    else
                    {

                        this.generateCaveLevelBlock(i, j);

                    }

                }

                else
                {
                    worldArr[i].Add(-1);
                }
                
            }
            
        }

        this.cutOutTop();

        this.generateOres();
        Debug.Log("Done creating world");

    }

    private float generatePerlinValue(int j, int i, int scale)
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

    private void addBaseBlock(int y, int x)
    {
        // since there is a block, check what kind of a block should there be
        // if the block is on the very top it should be a grass
        if (y == 0)
        {
            worldArr[y].Add(1);
        }
        else
        {
            // if there is a block above than the block has to be dirt, else grass
            if (worldArr[y - 1][x] == -1)
            {
                worldArr[y].Add(1);
            }
            else
            {
                worldArr[y].Add(0);
            }
        }
    }

    private void cutOutTop()
    {
        for (int i = 0; i < numOfBlocksX; i++)
        {

            int hillSize = (int)(maxHillSize - ((2 * maxHillSize) * generatePerlinValue(i, skySize, groundScale)));
            for (int j = 0; j < Math.Abs(hillSize); j++)
            {

                if (hillSize > 0)
                {

                    worldArr[skySize - j][i] = 0;

                }
                else
                {

                    worldArr[skySize + j][i] = -1;

                }

            }

        }
    }
    private void generateCaveLevelBlock(int y, int x)
    {

        // if perlin noise returns value that is greater than 0.5 there should be no block
        if (generatePerlinValue(x, y, caveScale) < 0.5)
        {
            addBaseBlock(y, x);

        }

        else
        {

            worldArr[y].Add(-1);

        }

    }

    private void generateOres()
    {

        // choose random amount of gold nodes in the world
        int n = Random.Range(5, 10);

        // loop that runs for n number of nodes
        for (int i = 0; i < n; i++)
        {

            Vector2 goldNodeCenter = new Vector2(Random.Range(0, numOfBlocksX), Random.Range(caveStart, numOfBlocksY));
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

    }

}