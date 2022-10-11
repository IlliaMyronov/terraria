using System;
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
    GameObject[,] scene = new GameObject[120, 68];

    // variable for knowing if the button was pressed previously
    private bool moving;

    // string to send messages to a block
    private string notification;

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
                    tile.GetComponent<block>().velocity = 0.0f;
                    scene[j, i] = tile;

                }

            }

        }

    }


    private void Update() 
    {
 

        if (Input.GetKeyDown(KeyCode.A) == true)
        {
            notification = "moveLeft";
            moving = true;
            notify(notification);
            Debug.Log("Move left");
        }

        else if (Input.GetKeyDown(KeyCode.D) == true)
        {

            notification = "moveRight";
            moving = true;
            notify(notification);
            Debug.Log("move right");
        }

        else if (Input.GetKeyDown(KeyCode.A) == false && Input.GetKeyDown(KeyCode.D) == false && moving == true)
        {
            notification = "stop";
            moving = false;
            notify(notification);
            Debug.Log("stop");
        }

        if (Input.GetMouseButtonDown(0))
        {
            destroyBlock(Input.mousePosition);
        }

    }

    private void notify(string message)
    {
        for (int i = 0; i < numOfBlocksY; i++)
        {

            for (int j = 0; j < numOfBlocksX; j++)
            {

                if (scene[j, i] != null)
                {


                    scene[j, i].GetComponent<block>().notify(message);

                }

            }

        }
    } 

    private void destroyBlock(Vector2 mouse)
    {
        int x = (int) Math.Round(mouse.x / numOfBlocksX);
        int y = (int)Math.Round(mouse.y / numOfBlocksY);
        Destroy(scene[x, y]);
    }

}
