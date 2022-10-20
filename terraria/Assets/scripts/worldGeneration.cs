using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worldGeneration : MonoBehaviour
{
    // world size properties
    [SerializeField]
    private int numOfBlocksX = 120;
    private int numOfBlocksY = 68;
    public int[,] worldArr = new int[120, 68];

    public void generateWorld()
    {

        for (int i = 0; i < numOfBlocksY; i++)
        {

            for (int j = 0; j < numOfBlocksX; j++)
            {

                if (i > 34)
                {

                    worldArr[j, i] = 0;

                }

                else
                {

                    worldArr[j, i] = -1;

                }

            }
        }

        Debug.Log("Done creating world");

    }

}