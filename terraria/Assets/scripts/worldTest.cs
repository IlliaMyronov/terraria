using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worldTest : MonoBehaviour
{

    [SerializeField]
    private GameObject tilePrefab;

    private int numOfBlocksX = 240;
    private int numOfBlocksY = 135;

    private void Awake()
    {

        for (int i = 0; i < numOfBlocksX; i++) 
        {

            for (int j = 0; j < numOfBlocksY; j++) 
            {

                GameObject tile = Instantiate(tilePrefab) as GameObject;
                tile.transform.position = new Vector2(i - numOfBlocksX / (float)2.0, numOfBlocksY/2 - j);

            }
            

        }

    }

}
