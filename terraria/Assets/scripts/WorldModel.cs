using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class WorldModel : MonoBehaviour
{

    private int screenSizeX = 120;
    private int screenSizeY = 68;
    private int blockSize = 16;
    private float relativePosX = 0;
    private float relativePosY = 0;
    Vector2 velocity = new Vector2();
    [SerializeField]
    private GameObject blockPrefab;
    private GameObject[,] scene = new GameObject[120, 68];

    public void drawWorld(int[,] world)
    {
        Debug.Log("start redrawing");
        for (int i = 0; i < screenSizeY; i++)
        {

            for (int j = 0; j < screenSizeX; j++)
            {

                if (world[j, i] != -1)
                {
                    GameObject tile = Instantiate(blockPrefab) as GameObject;
                    tile.transform.position = new Vector2(j - screenSizeX / (float)2.0, screenSizeY / (float)2.0 - i);
                    tile.GetComponent<block>().velocity = 0.0f;
                    scene[j, i] = tile;

                }

            }

        }

    }


    public void move(String moveType)
    {

        if (moveType == "moveLeft")
        {
            velocity.x = velocity.x - 5;
        }

        else if (moveType == "moveRight")
        {
            velocity.x = velocity.x + 5;
        }

        else if (moveType == "stop")
        {
            velocity.x = 0;
        }

        blockMove(velocity);

    }

    public void blockMove(Vector2 velocity)
    {
        for (int i = 0; i < screenSizeY; i++)
        {

            for (int j = 0; j < screenSizeX; j++)
            {

                if (scene[j, i] != null)
                {


                    scene[j, i].GetComponent<block>().move(velocity);

                }

            }

        }
    }

    public void destroyBlock(Vector2 mousePos)
    {

        Debug.Log("relative x = " + relativePosX + " and relative y = " + relativePosY);
        int x = (int)Math.Round((mousePos.x - relativePosX) / blockSize);
        int y = (int)Math.Round((1080 - mousePos.y - relativePosY) / blockSize);
        Destroy(scene[x, y]);

    }

    private void Update()
    {
        // velocity is in units/seconds; there are blockSize pixels in a single unit and Time.deltaTime is time since last frame
        relativePosX += blockSize*(velocity.x)*Time.deltaTime;
        relativePosY += blockSize*(velocity.y)*Time.deltaTime;
    }

}