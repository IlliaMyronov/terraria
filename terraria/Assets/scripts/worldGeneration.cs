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
                if (generatePerlinValue(j, i) < 0.5)
                {

                    worldArr[i].Add(0);

                }

                else
                {

                    worldArr[i].Add(-1);

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