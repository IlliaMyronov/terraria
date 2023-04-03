using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSystem : MonoBehaviour
{

    // lists for storing solid blocks
    [SerializeField] private Sprite[] solidBlocks;
    [SerializeField] private string[] solidNames;

    // lists for storing background blocks
    [SerializeField] private Sprite[] backingBlocks;
    [SerializeField] private string[] backingNames;

    // list to store all existing blocks
    public BlockInfo[] allBlocks;

    // integer to give access to the starting position of backing blocks in allBlocks array
    public int backingStart;

    public void GenerateList()
    {

        // initializing array for all blocks with correct size
        allBlocks = new BlockInfo[solidBlocks.Length + backingBlocks.Length];
        backingStart = solidBlocks.Length;

        // int to create block IDs
        int newBlockID = 0;

        // filling all blocks array with solid blocks
        for (int i = 0; i < solidBlocks.Length; i++)
        {

            allBlocks[newBlockID] = new BlockInfo(newBlockID, solidNames[i], solidBlocks[i], true);
            Debug.Log("Solid block: allBlocks[" + newBlockID + "]" + solidNames[i]);
            newBlockID++;

        }

        // filling all blocks array with backing blocks
        for (int i = 0; i < backingBlocks.Length; i++)
        {

            allBlocks[newBlockID] = new BlockInfo(newBlockID, backingNames[i], backingBlocks[i], false);
            Debug.Log("Backing block: allBlocks[" + newBlockID + "]" + backingNames[i]);
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
    private bool isSolid;

    public BlockInfo(int id, string myName, Sprite mySprite, bool myIsSolid)
    {
        blockID = id;
        blockName = myName;
        blockSprite = mySprite;
        isSolid = myIsSolid;
    }
    public Sprite GetSprite()
    {
        return blockSprite;
    }

    public string GetName()
    {
        return blockName;
    }

}