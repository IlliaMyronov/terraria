using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class world : MonoBehaviour
{

    // reference to the block systen
    private blockSystem blockSys;

    // variables for the block template
    [SerializeField]
    private GameObject blockPrefab;

    // size of one block is 16x16
    private int numOfBlocksX = 120;
    private int numOfBlocksY = 68;
    int[,] worldArr = new int[120, 68];

    private void Awake()
    {

        // 2d Array to store the world


        for (int i = 0; i < numOfBlocksY; i++)
        {

            for (int j = 0; j < numOfBlocksX; j++)
            {

                if (i > 22)
                {

                    worldArr[j, i] = 0;

                }

                else
                {

                    worldArr[j, i] = -1;

                }

            }
        }

        displayMap();
    }

    public void displayMap()
    {

        for (int i = 0; i < numOfBlocksY; i++)
        {

            for (int j = 0; j < numOfBlocksX; j++)
            {

                if (worldArr[j, i] != -1)
                {


                    GameObject tile = Instantiate(blockPrefab) as GameObject;
                    tile.transform.position = new Vector2(j - numOfBlocksX / (float)2.0, numOfBlocksY / (float)2.0 - i);
                    tile.GetComponent<block>().speed = 15.0f;

                }

            }

        }

    }

}
