using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;
    private int chunkSize;
    private List<List<GameObject>> chunkBlocks;
    private void Awake()
    {
        chunkSize = 16;
        chunkBlocks = new List<List<GameObject>>();

    }

    public int getChunkSize()
    {
        return chunkSize;
    }
    public void PopulateChunk(List<List<int>> world, Vector2Int arrayPos, GameObject terrain, BlockSystem blockSys)
    {
        for (int i = 0; i < chunkSize; i++)
        {
            chunkBlocks.Add(new List<GameObject>());

            for (int j = 0; j < chunkSize; j++)
            {

                Vector2Int currentBlock = new Vector2Int(arrayPos.x + j, arrayPos.y + i);
                if (world[currentBlock.y][currentBlock.x] != -1)
                {
                    GameObject tile = Instantiate(blockPrefab) as GameObject;
                    tile.name = blockSys.allBlocks[world[currentBlock.y][currentBlock.x]].getName();
                    tile.transform.position = new Vector2(this.transform.position.x + j, this.transform.position.y - i);
                    tile.transform.parent = this.transform;
                    tile.GetComponent<SpriteRenderer>().sprite = blockSys.allBlocks[world[currentBlock.y][currentBlock.x]].getSprite();

                    chunkBlocks[i].Add(tile);
                }
                else 
                {
                    chunkBlocks[i].Add(null);
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

                if (chunkBlocks[i][j] != null)
                {
                    Destroy(chunkBlocks[i][j]);
                }
                chunkBlocks[i].RemoveAt(j);

            }

        }
    }
}
