using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.Experimental.GraphView.GraphView;

public class WorldDrawer : MonoBehaviour
{
    [SerializeField] GameObject chunkPrefab;
    private static int chunkSize = 16;
    private Vector2Int worldSize;
    private GameObject terrain;
    public List<List<GameObject>> DrawWorld(List<List<int>> world, List<List<int>> background, List<List<GameObject>> sceneChunks, Vector2Int chunksPerScreen, Vector2Int spawnLocation, BlockSystem blockSys)
    {

        worldSize = new Vector2Int(world[0].Count, world.Count);
        terrain = new GameObject(name = "terrain");

        for (int i = 0; i < chunksPerScreen.y; i++)
        {

            sceneChunks.Add(new List<GameObject>());
            for (int j = 0; j < chunksPerScreen.x; j++)
            {

                GameObject newChunk = Instantiate(chunkPrefab) as GameObject;
                newChunk.transform.parent = terrain.transform;
                //((chunksPerScreen.x / 2) + j - spawnLocation.x / chunkSize) * chunkSize, (spawnLocation.y / chunkSize - i + (chunksPerScreen.y / 2)) * chunkSize
                Vector2 chunkPos = new Vector2(spawnLocation.x - ((chunksPerScreen.x / 2) * chunkSize) + (j * chunkSize), spawnLocation.y + ((chunksPerScreen.y / 2) * chunkSize) - (i * chunkSize));
                Debug.Log("chunk pos is " + chunkPos);
                newChunk.transform.position = chunkPos;
                Vector2Int arrayPos = new Vector2Int((int)chunkPos.x, spawnLocation.y - ((chunksPerScreen.y / 2) * chunkSize) + (i * chunkSize));
                
                newChunk.GetComponent<Chunk>().PopulateChunk(world, background, arrayPos, terrain, blockSys);

                sceneChunks[i].Add(newChunk);

            }

        }

        return sceneChunks;

    }

    public List<List<GameObject>> RedrawWorld(List<List<int>> world, List<List<int>> background, List<List<GameObject>> sceneChunks, Vector2 charPos, Vector2 lastCharPos, BlockSystem blockSys, float redrawDistance)
    {

        sceneChunks = RedrawWorldX(world, background, sceneChunks, charPos.x, blockSys, redrawDistance, lastCharPos.x);
        sceneChunks = RedrawWorldY(world, background, sceneChunks, charPos.y, blockSys, redrawDistance, lastCharPos.y);
        return sceneChunks;

    }

    private List<List<GameObject>> RedrawWorldX(List<List<int>> world, List<List<int>> background, List<List<GameObject>> sceneChunks, float playerX, BlockSystem blockSys, float redrawDistance, float lastCharPosX)
    {
        // if player moved enough in x direction start redrawing world
        if (Math.Abs(playerX - lastCharPosX) > redrawDistance)
        {

            Vector2Int chunkToDelete;
            Vector2Int chunkToInsert;
            Vector2 chunkPos;
            Vector2Int arrayPos;

            for (int i = 0; i < sceneChunks.Count; i++)
            {
                if (playerX > lastCharPosX)
                {
                    //finding the position in the array of the block to delete (far left)
                    chunkToDelete = new Vector2Int(0, i);

                    //finding the row in the array to add a new chunk
                    chunkToInsert = new Vector2Int(sceneChunks[i].Count - 1, i);

                    //finding position to place new chunk
                    chunkPos = new Vector2(sceneChunks[chunkToInsert.y][chunkToInsert.x].transform.position.x + chunkSize,
                                           sceneChunks[chunkToInsert.y][chunkToInsert.x].transform.position.y);

                    arrayPos = new Vector2Int(sceneChunks[chunkToInsert.y][chunkToInsert.x].GetComponent<Chunk>().myArrayPos.x + chunkSize,
                                              sceneChunks[chunkToInsert.y][chunkToInsert.x].GetComponent<Chunk>().myArrayPos.y);
                }
                else
                {
                    //finding the position in the array of the block to delete (far right)
                    chunkToDelete = new Vector2Int(sceneChunks[i].Count - 1, i);

                    //finding the row in the array to add a new chunk
                    chunkToInsert = new Vector2Int(0, i);

                    //finding position to place new chunk
                    chunkPos = new Vector2(sceneChunks[chunkToInsert.y][chunkToInsert.x].transform.position.x - chunkSize, 
                                           sceneChunks[chunkToInsert.y][chunkToInsert.x].transform.position.y);

                    arrayPos = new Vector2Int(sceneChunks[chunkToInsert.y][chunkToInsert.x].GetComponent<Chunk>().myArrayPos.x - chunkSize,
                                              sceneChunks[chunkToInsert.y][chunkToInsert.x].GetComponent<Chunk>().myArrayPos.y);
                }

                sceneChunks[chunkToDelete.y][chunkToDelete.x].GetComponent<Chunk>().DestroyChunk();
                Destroy(sceneChunks[chunkToDelete.y][chunkToDelete.x]);
                sceneChunks[chunkToDelete.y].RemoveAt(chunkToDelete.x);

                //adding a chunk
                GameObject newChunk = Instantiate(chunkPrefab) as GameObject;
                newChunk.transform.parent = terrain.transform;
                newChunk.transform.position = chunkPos;
                newChunk.GetComponent<Chunk>().PopulateChunk(world, background, arrayPos, terrain, blockSys);
                sceneChunks[chunkToInsert.y].Insert(chunkToInsert.x, newChunk);

            }
        }

        return sceneChunks;

    }

    private List<List<GameObject>> RedrawWorldY(List<List<int>> world, List<List<int>> background, List<List<GameObject>> sceneChunks, float playerY, BlockSystem blockSys, float redrawDistance, float lastCharPosY)
    {
        // if character moved enough in y direction redraw world
        if (Math.Abs(playerY - lastCharPosY) > redrawDistance)
        {

            //adding empty array to fill it with new chunks
            if (playerY > lastCharPosY)
            {
                sceneChunks.Insert(0, new List<GameObject>());
            }
            else
            {
                sceneChunks.Add(new List<GameObject>());
            }

            Vector2Int chunkToDelete;
            int chunkToAdd;
            Vector2 chunkPos;
            Vector2Int chunkReference;
            Vector2Int arrayPos;

            //loop that adds and deletes all needed chunks
            for (int i = 0; i < sceneChunks[1].Count; i++)
            {
                Debug.Log("inside for loop");
                //check from which side should we add a chunk and from which should we delete it
                if (playerY > lastCharPosY)
                {
                    //finding the position in the array of bottom right chunk to delete it
                    chunkToDelete = new Vector2Int(sceneChunks[sceneChunks.Count - 1].Count - 1, sceneChunks.Count - 1);

                    //finding the row in the array to add a new chunk
                    chunkToAdd = 0;

                    //finding position to place new chunk
                    chunkReference = new Vector2Int(i, 1);
                    chunkPos = new Vector2(sceneChunks[chunkReference.y][chunkReference.x].transform.position.x, 
                                           sceneChunks[chunkReference.y][chunkReference.x].transform.position.y + chunkSize);

                    //finding array position of new chunk
                    arrayPos = new Vector2Int(sceneChunks[chunkReference.y][chunkReference.x].GetComponent<Chunk>().myArrayPos.x,
                                              sceneChunks[chunkReference.y][chunkReference.x].GetComponent<Chunk>().myArrayPos.y - chunkSize);

                }

                else
                {
                    //finding the position of top right chunk to delete it
                    chunkToDelete = new Vector2Int(sceneChunks[0].Count - 1, 0);

                    //finding the row in the array to add a new chunk
                    chunkToAdd = sceneChunks.Count - 1;

                    //finding position to place new chunk
                    chunkReference = new Vector2Int(i, sceneChunks.Count - 2);
                    chunkPos = new Vector2(sceneChunks[chunkReference.y][chunkReference.x].transform.position.x, 
                                           sceneChunks[chunkReference.y][chunkReference.x].transform.position.y - chunkSize);

                    //finding array position of a new chunk
                    arrayPos = new Vector2Int(sceneChunks[chunkReference.y][chunkReference.x].GetComponent<Chunk>().myArrayPos.x,
                                              sceneChunks[chunkReference.y][chunkReference.x].GetComponent<Chunk>().myArrayPos.y + chunkSize);
                }

                //deleting a chunk
                sceneChunks[chunkToDelete.y][chunkToDelete.x].GetComponent<Chunk>().DestroyChunk();
                Destroy(sceneChunks[chunkToDelete.y][chunkToDelete.x]);
                sceneChunks[chunkToDelete.y].RemoveAt(chunkToDelete.x);

                //adding a chunk
                GameObject newChunk = Instantiate(chunkPrefab) as GameObject;
                newChunk.transform.parent = terrain.transform;
                newChunk.transform.position = chunkPos;
                newChunk.GetComponent<Chunk>().PopulateChunk(world, background, arrayPos, terrain, blockSys);
                sceneChunks[chunkToAdd].Add(newChunk);
            }

            //removing empty array after deleting chunks from it
            if (playerY > lastCharPosY)
            {
                sceneChunks.RemoveAt(sceneChunks.Count - 1);
            }
            else
            {
                sceneChunks.RemoveAt(0);
            }


        }

        return sceneChunks;

    }

    private Vector2Int FindArrPos(Vector2 pos, Vector2Int worldSize)
    {
        return new Vector2Int((int)(pos.x + (worldSize.x / 2)), (int)Math.Abs(pos.y - (worldSize.y / 2)));
    }

}