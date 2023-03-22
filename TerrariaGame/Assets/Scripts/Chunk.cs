using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;
    private int chunkSize;
    private List<List<List<GameObject>>> chunkBlocks;
    public Vector2Int myArrayPos;

    private void Awake()
    {
        chunkSize = 16;
        chunkBlocks = new List<List<List<GameObject>>>();

    }

    public int getChunkSize()
    {
        return chunkSize;
    }
    public void PopulateChunk(List<List<int>> world, List<List<int>> background, Vector2Int arrayPos, GameObject terrain, BlockSystem blockSys)
    {
        // saving array position to make it easier to access array position for the new chunks.
        myArrayPos= arrayPos;

        for (int i = 0; i < chunkSize; i++)
        {
            chunkBlocks.Add(new List<List<GameObject>>());

            for (int j = 0; j < chunkSize; j++)
            {

                Vector2Int currentBlock = new Vector2Int(arrayPos.x + j, arrayPos.y + i);

                // add a list which will contain information both about solid and backing blocks
                chunkBlocks[i].Add(new List<GameObject>());


                if (world[currentBlock.y][currentBlock.x] != -1)
                {
                    // add a solid block to the first position of the array

                    GameObject tile = Instantiate(blockPrefab) as GameObject;
                    tile.name = blockSys.allBlocks[world[currentBlock.y][currentBlock.x]].getName();
                    tile.transform.position = new Vector2(this.transform.position.x + j, this.transform.position.y - i);
                    tile.transform.parent = this.transform;
                    tile.GetComponent<SpriteRenderer>().sortingLayerName = "Solid";
                    tile.layer = 10;
                    tile.AddComponent<BoxCollider2D>();
                    tile.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
                    tile.GetComponent<SpriteRenderer>().sprite = blockSys.allBlocks[world[currentBlock.y][currentBlock.x]].getSprite();
                    chunkBlocks[i][j].Add(tile);
                }
                else
                {
                    // add a null on the place of a solid block if there should not be one

                    chunkBlocks[i][j].Add(null);
                }

                if (background[currentBlock.y][currentBlock.x] != -1)
                {
                    // add a background block (same as a solid block but we have to shift array position in Block System)

                    GameObject tile = Instantiate(blockPrefab) as GameObject;
                    tile.name = blockSys.allBlocks[background[currentBlock.y][currentBlock.x] + blockSys.backingStart].getName();
                    tile.transform.position = new Vector2(this.transform.position.x + j, this.transform.position.y - i);
                    tile.transform.parent = this.transform;
                    tile.GetComponent<SpriteRenderer>().sortingLayerName = "Backing";
                    tile.layer = 11;
                    tile.AddComponent<BoxCollider2D>();
                    tile.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
                    tile.GetComponent<SpriteRenderer>().sprite = blockSys.allBlocks[background[currentBlock.y][currentBlock.x] + blockSys.backingStart].getSprite();
                    chunkBlocks[i][j].Add(tile);
                }
                else
                {
                    // add a null on the spot of a background block

                    chunkBlocks[i][j].Add(null);
                }

            }
        }
    }

    public void DestroyChunk()
    {

        for (int i = chunkSize - 1; i > -1; i--)
        {

            for (int j = chunkSize - 1; j > -1; j--)
            {

                int numOfBlocks = chunkBlocks[i][j].Count;

                for(int l = 0; l < numOfBlocks; l++)
                {

                    if (chunkBlocks[i][j][0] != null)
                    {
                        Destroy(chunkBlocks[i][j][0]);
                    }
                    chunkBlocks[i][j].RemoveAt(0);

                }
            }
        }
    }

    public void DestroyMyBlock(Vector2Int arrayPos, bool isSolid)
    {
        if (isSolid)
        {
            Destroy(chunkBlocks[arrayPos.y][arrayPos.x][0]);
            chunkBlocks[arrayPos.y][arrayPos.x][0] = null;
        }
        else
        {
            Destroy(chunkBlocks[arrayPos.y][arrayPos.x][1]);
            chunkBlocks[arrayPos.y][arrayPos.x][1] = null;
        }
    }
}