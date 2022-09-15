using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class world : MonoBehaviour
{

    // reference to the block systen
    private blockSystem blockSys;

    // variables for the block template
    [SerializeField]
    private GameObject tilePrefab;

    private int numOfBlocksX = 240;
    private int numOfBlocksY = 135;
    int[,] worldArr = new int[240, 135];

    private void Awake()
    {

        // 2d Array to store the world
        

        for (int i = 0; i < 135; i++)
        {

            for (int j = 0; j < 240; j++)
            {

                if (i > 45)
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

    public void displayMap ()
    {

        for (int i = 0; i < 135; i++)
        {

            for (int j = 0; j < 240; j++)
            {

                if (worldArr[j, i] != -1)
                {

                    
                    GameObject tile = Instantiate(tilePrefab) as GameObject;
                    tile.transform.position = new Vector2(j - numOfBlocksX / (float)2.0, numOfBlocksY / (float)2.0 - i);


                }

            }

        }

    }

}
