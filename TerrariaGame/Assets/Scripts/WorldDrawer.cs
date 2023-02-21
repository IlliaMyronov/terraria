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
    Vector2Int worldSize;
    GameObject terrain;
    public List<List<GameObject>> DrawWorld(List<List<int>> world, List<List<int>> background, List<List<GameObject>> sceneChunks, Vector2Int chunksPerScreen, BlockSystem blockSys)
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
                Vector2 chunkPos = new Vector2(j * chunkSize - (chunksPerScreen.x / 2) * chunkSize, (chunksPerScreen.y / 2) * chunkSize - (i * chunkSize));
                newChunk.transform.position = chunkPos;
                Vector2Int worldSize = new Vector2Int(world[0].Count, world.Count);
                Vector2Int arrayPos = FindArrPos(chunkPos, worldSize);

                newChunk.GetComponent<Chunk>().PopulateChunk(world, background, arrayPos, terrain, blockSys);

                sceneChunks[i].Add(newChunk);

            }

        }

        Debug.Log("done drawing the map");
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

            for (int i = 0; i < sceneChunks.Count; i++)
            {
                if (playerX > lastCharPosX)
                {
                    //finding the position in the array of the block to delete (far left)
                    chunkToDelete = new Vector2Int(0, i);

                    //finding the row in the array to add a new chunk
                    chunkToInsert = new Vector2Int(sceneChunks[i].Count - 1, i);

                    //finding position to place new chunk
                    chunkPos = new Vector2(sceneChunks[i][sceneChunks[i].Count - 1].transform.position.x + chunkSize,
                                                    sceneChunks[i][sceneChunks[i].Count - 1].transform.position.y);
                }
                else
                {
                    //finding the position in the array of the block to delete (far right)
                    chunkToDelete = new Vector2Int(sceneChunks[i].Count - 1, i);

                    //finding the row in the array to add a new chunk
                    chunkToInsert = new Vector2Int(0, i);

                    //finding position to place new chunk
                    chunkPos = new Vector2(sceneChunks[i][0].transform.position.x - chunkSize, sceneChunks[i][0].transform.position.y);
                }

                sceneChunks[chunkToDelete.y][chunkToDelete.x].GetComponent<Chunk>().DestroyChunk();
                Destroy(sceneChunks[chunkToDelete.y][chunkToDelete.x]);
                sceneChunks[chunkToDelete.y].RemoveAt(chunkToDelete.x);

                //adding a chunk
                GameObject newChunk = Instantiate(chunkPrefab) as GameObject;
                newChunk.transform.parent = terrain.transform;
                newChunk.transform.position = chunkPos;
                Vector2Int arrayPos = FindArrPos(chunkPos, worldSize);
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
                    chunkPos = new Vector2(sceneChunks[1][i].transform.position.x, sceneChunks[1][i].transform.position.y + chunkSize);
                }
                else
                {
                    //finding the position of top right chunk to delete it
                    chunkToDelete = new Vector2Int(sceneChunks[0].Count - 1, 0);

                    //finding the row in the array to add a new chunk
                    chunkToAdd = sceneChunks.Count - 1;

                    //finding position to place new chunk
                    chunkPos = new Vector2(sceneChunks[sceneChunks.Count - 2][i].transform.position.x, sceneChunks[sceneChunks.Count - 2][i].transform.position.y - chunkSize);
                }
                Debug.Log("deleting chunk at " + sceneChunks[chunkToDelete.y][chunkToDelete.x].transform.position + " adding chunk at " + chunkPos);
                //deleting a chunk
                sceneChunks[chunkToDelete.y][chunkToDelete.x].GetComponent<Chunk>().DestroyChunk();
                Destroy(sceneChunks[chunkToDelete.y][chunkToDelete.x]);
                sceneChunks[chunkToDelete.y].RemoveAt(chunkToDelete.x);

                //adding a chunk
                GameObject newChunk = Instantiate(chunkPrefab) as GameObject;
                newChunk.transform.parent = terrain.transform;
                newChunk.transform.position = chunkPos;
                Vector2Int arrayPos = FindArrPos(chunkPos, worldSize);
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