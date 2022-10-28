using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class WorldModel : MonoBehaviour
{

    private int screenSizeX = 140;
    private int screenSizeY = 68;
    private int blockSize = 16;
    private float relativePosX = 0;
    private float relativePosY = 0;
    Vector2 velocity = new Vector2();
    [SerializeField]
    private GameObject blockPrefab;
    private List<List<GameObject>> scene = new List<List<GameObject>>();
    [SerializeField]
    private worldGeneration worldGen;
    [SerializeField]
    private WorldRedrawer worldRedrawer;
    private int worldOffsetX;
    private int worldOffsetY;

    public void generateWorld()
    {
        worldGen.generateWorld();
        worldOffsetX = worldGen.getWorldSize(true) / 2 - (screenSizeX / 2);
        worldOffsetY = worldGen.getWorldSize(false) / 2 - (screenSizeY / 2);
        Debug.Log("X offset is " + worldOffsetX + " and Y offset is " + worldOffsetY);
        scene = worldRedrawer.drawWorld(worldGen.getWorld(), scene, screenSizeY, screenSizeX, blockPrefab, worldOffsetX, worldOffsetY);
    }
    public void drawWorld()
    {

        Debug.Log("start redrawing");
        

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

    private void blockMove(Vector2 velocity)
    {

        for (int i = 0; i < screenSizeY; i++)
        {
                
            for (int j = 0; j < screenSizeX; j++)
            {

                if (scene[i][j] != null)
                {

                    scene[i][j].GetComponent<block>().move(velocity);

                }

            }

        }
    }

    public void destroyBlock(Vector2 mousePos)
    {

        Debug.Log("relative x = " + relativePosX + " and relative y = " + relativePosY);
        int x = (int)Math.Round((mousePos.x - relativePosX) / blockSize);
        int y = (int)Math.Round((1080 - mousePos.y - relativePosY) / blockSize);
        Destroy(scene[y][x]);

    }

    private void Update()
    {
        // velocity is in units/seconds; there are blockSize pixels in a single unit and Time.deltaTime is time since last frame
        relativePosX -= blockSize*(velocity.x)*Time.deltaTime;
        relativePosY += blockSize*(velocity.y)*Time.deltaTime;
        if ((relativePosX > 100) || (relativePosX < -100))
        {
            Debug.Log("old off set is " + worldOffsetX + " relative x pos is " + relativePosX + " new offset should be " + (worldOffsetX + (int)(relativePosX / 16)));
            worldOffsetX = (worldOffsetX + (int)(relativePosX / 16));
            worldOffsetY = worldOffsetY + (int) (relativePosY / 16);
            Debug.Log("new offset is " + worldOffsetX);
            scene = worldRedrawer.redrawWorld(scene, worldGen.getWorld(), screenSizeX, screenSizeY, worldOffsetX, worldOffsetY, blockPrefab);
            relativePosX = 0;
            this.blockMove(velocity);
        }
       
    }

}