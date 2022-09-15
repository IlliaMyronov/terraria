using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class world : MonoBehaviour
{
    // reference to the block systen
    private blockSystem blockSys;

    // variables for the block template
    private GameObject blockPrefab;
    private SpriteRenderer currentRend;
    // 2d Array to store the world
    int[,] world = new int[135, 240];

    public void Awake()
    {

        for (int i = 0; i < 135; i++)
        {

            for (int j = 0; j < 240; j++)
            {

                if (i > 45)
                {

                    world[i, j] = 0;

                }

                else
                {

                    world[i, j] = -1;

                }

            }
        }

    }

    public void drawWorld
    {

        for (int i = 0; i<world.Length; i++)
        {

            for (int j = 0; j<world[0].Length; j++)
            {

                if (world[i, j] != -1) 
                { 
                    


                }

            }

        }

    }
}
