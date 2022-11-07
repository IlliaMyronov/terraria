using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class WorldModel : MonoBehaviour
{

    private static Vector2 screenSize = new Vector2(120, 68);
    private int chunkSizeX = 140;
    private int chunkSizeY = 68;
    private int blockSize = 16;
    private Vector2 relativePos = new Vector2(0.0f, 0.0f);
    private Vector2 realPos = new Vector2(0.0f, 0.0f);
    Vector2 velocity = new Vector2();
    [SerializeField]
    private GameObject blockPrefab;
    private List<List<GameObject>> scene = new List<List<GameObject>>();
    [SerializeField]
    private WorldGeneration worldGen;
    [SerializeField]
    private WorldRedrawer worldRedrawer;
    [SerializeField]
    private GameObject cam;
    private Vector2 acceleration;
    private float friction = 10.0f;
    public void generateWorld()
    {
        worldGen.generateWorld();
        scene = worldRedrawer.drawWorldV2(scene, worldGen.getWorld(), blockPrefab, chunkSizeX, chunkSizeY);
    }
    public void drawWorld()
    {

        Debug.Log("start redrawing");
        
    }


    public void move(String moveType)
    {

        if (moveType == "moveLeft")
        {
            acceleration.x = -5.0f;
        }

        else if (moveType == "moveRight")
        {
            acceleration.x = 5.0f;
        }

        else if (moveType == "stop")
        {
            acceleration.x = 0.0f;
        }

    }

    private void moveChar(Vector2 velocity)
    {

        cam.GetComponent<Cam>().move(velocity);
        
    }

    public void destroyBlock(Vector2 mousePos)
    {

        Debug.Log("mouse pos x is " + mousePos.x + " mouse pos y is " + mousePos.y);
        int x = (int)Math.Round(((mousePos.x + relativePos.x) / blockSize) + (chunkSizeX - screenSize.x) / 2);
        int y = (int)Math.Round((1080 - mousePos.y - relativePos.y) / blockSize);
        Debug.Log("deleting block by coordinates x = " + x + ", y = " + y);
        Destroy(scene[y][x]);

    }

    private void Update()
    {
        // velocity is in units/seconds; there are blockSize pixels in a single unit and Time.deltaTime is time since last frame
        relativePos.x += blockSize*(velocity.x)*Time.deltaTime;
        relativePos.y += blockSize*(velocity.y)*Time.deltaTime;
        //character speeds up because button was pressed
        if (acceleration.x != 0.0f && Math.Abs(velocity.x) < 5.0f)
        {
            velocity.x = velocity.x + (acceleration.x * Time.deltaTime);
        }
        //character stops due to friction
        else if (Math.Abs(velocity.x) != 0 && acceleration.x == 0.0f)
        {

            if (Math.Abs(velocity.x) < 0.1f)
            {

                velocity.x = 0.0f;
                moveChar(velocity);
            }

            else
            {
                if (velocity.x > 0)
                    velocity.x = velocity.x - (friction * Time.deltaTime);
                else
                    velocity.x = velocity.x + (friction * Time.deltaTime);
            }
        } 
        
        velocity.y = velocity.y + (acceleration.y * Time.deltaTime);

        if (velocity.x != 0.0f)
            moveChar(velocity);

        if ((relativePos.x > 100) || (relativePos.x < -100))
        {
            Debug.Log("redrawing because relative pos of x is " + relativePos.x);
            scene = worldRedrawer.redrawWorldV3(scene, worldGen.getWorld(), blockPrefab, relativePos, blockSize, chunkSizeX, chunkSizeY, realPos);
            realPos.x += relativePos.x;
            realPos.y += relativePos.y;
            relativePos.x = 0;
            relativePos.y = 0;
            Debug.Log("relative position of X is " + relativePos.x);
        }
       
    }

}