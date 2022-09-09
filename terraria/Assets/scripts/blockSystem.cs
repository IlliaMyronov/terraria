using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockSystem : MonoBehaviour
{

    // lists for storing solid blocks
    [SerializeField]
    private Sprite[] solidBlocks;
    [SerializeField]
    private string[] solidNames;

    // list to store all existing blocks
    public Block[] allBlocks;

    private void Awake()
    {

        // Initialize allBlocks array
        allBlocks = new Block[solidBlocks.Length];

        // int to create block IDs
        int newBlockID = 0;

        // filling all blocks array with blocks
        for (int i = 0; i < solidBlocks.Length; i++)
        {

            allBlocks[newBlockID] = new Block(newBlockID, solidNames[i], solidBlocks[i]);
            Debug.Log("Solid block: allBlocks[" + newBlockID + "]" + solidNames[i]);
            newBlockID++;

        }

    }
}

public class Block
{

    private int blockID;
    private string blockName;
    private Sprite blockSprite;

    public Block(int id, string myName, Sprite mySprite)
    {
        blockID = id;
        blockName = myName;
        blockSprite = mySprite;
    }

}
