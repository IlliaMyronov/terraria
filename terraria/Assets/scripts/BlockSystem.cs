using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSystem : MonoBehaviour
{

    // lists for storing solid blocks
    [SerializeField]
    private Sprite[] solidBlocks;
    [SerializeField]
    private string[] solidNames;

    // list to store all existing blocks
    public BlockInfo[] allBlocks;

    private void Awake()
    {

        // Initialize allBlocks array
        allBlocks = new BlockInfo[solidBlocks.Length];

        // int to create block IDs
        int newBlockID = 0;

        // filling all blocks array with blocks
        for (int i = 0; i < solidBlocks.Length; i++)
        {

            allBlocks[newBlockID] = new BlockInfo(newBlockID, solidNames[i], solidBlocks[i]);
            Debug.Log("Solid block: allBlocks[" + newBlockID + "]" + solidNames[i]);
            newBlockID++;

        }

    }

    public BlockInfo getBlock(int ID)
    {
        return allBlocks[ID];
    }
}

public class BlockInfo
{

    private int blockID;
    private string blockName;
    private Sprite blockSprite;

    public BlockInfo(int id, string myName, Sprite mySprite)
    {
        blockID = id;
        blockName = myName;
        blockSprite = mySprite;
    }
    public Sprite getSprite()
    {
        return blockSprite; 
    }

}