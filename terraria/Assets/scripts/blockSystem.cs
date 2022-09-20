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
    public BlockType[] allBlockTypes;

    private void Awake()
    {

        // Initialize allBlocks array
        allBlockTypes = new BlockType[solidBlocks.Length];

        // int to create block IDs
        int newBlockID = 0;

        // filling all blocks array with blocks
        for (int i = 0; i < solidBlocks.Length; i++)
        {

            allBlockTypes[newBlockID] = new BlockType(newBlockID, solidNames[i], solidBlocks[i]);
            Debug.Log("Solid block: allBlockTypes[" + newBlockID + "]" + solidNames[i]);
            newBlockID++;

        }

    }
}

public class BlockType
{

    private int blockID;
    private string blockName;
    private Sprite blockSprite;

    public BlockType(int id, string myName, Sprite mySprite)
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
