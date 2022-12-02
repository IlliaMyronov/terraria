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
    public List<List<GameObject>> DrawWorld(List<List<int>> world, List<List<GameObject>> sceneChunks, Vector2Int chunksPerScreen, BlockSystem blockSys)
    {

        worldSize = new Vector2Int(world[0].Count, world.Count);
        terrain = new GameObject(name = "terrain");

        for(int i = 0; i < chunksPerScreen.y; i++) 
        {

            sceneChunks.Add(new List<GameObject>());
            for(int j = 0; j < chunksPerScreen.x; j++)
            {

                GameObject newChunk = Instantiate(chunkPrefab) as GameObject;
                newChunk.transform.parent = terrain.transform;
                Vector2 chunkPos = new Vector2(j * chunkSize - (chunksPerScreen.x / 2) * chunkSize, (chunksPerScreen.y / 2) * chunkSize - (i * chunkSize));
                newChunk.transform.position = chunkPos;
                Vector2Int worldSize = new Vector2Int(world[0].Count, world.Count);
                Vector2Int arrayPos = FindArrPos(chunkPos, worldSize);

                newChunk.GetComponent<Chunk>().PopulateChunk(world, arrayPos, terrain, blockSys);

                sceneChunks[i].Add(newChunk);

            }

        }

        Debug.Log("done drawing the map");
        return sceneChunks;

    }

    public List<List<GameObject>> RedrawWorld(List<List<int>> world, List<List<GameObject>> sceneChunks, Vector2 charPos, Vector2 lastCharPos, BlockSystem blockSys, float redrawDistance)
    {

        sceneChunks = RedrawWorldX(world, sceneChunks, charPos.x, blockSys, redrawDistance, lastCharPos.x);
        sceneChunks = RedrawWorldY(world, sceneChunks, charPos.y, blockSys, redrawDistance, lastCharPos.y);
        return sceneChunks;

    }

    private List<List<GameObject>> RedrawWorldX(List<List<int>> world, List<List<GameObject>> sceneChunks, float playerX, BlockSystem blockSys, float redrawDistance, float lastCharPosX)
    {
        // if player moved enough in x direction start redrawing world
        if (Math.Abs(Math.Abs(playerX) - Math.Abs(lastCharPosX)) > redrawDistance)
        {
            if (playerX > lastCharPosX)
            {
                for (int i = 0; i < sceneChunks.Count; i++)
                {
                    // deleting far left chunk
                    sceneChunks[i][0].GetComponent<Chunk>().DestroyChunk();
                    Destroy(sceneChunks[i][0]);
                    sceneChunks[i].RemoveAt(0);

                    // adding new chunk on far right
                    GameObject newChunk = Instantiate(chunkPrefab) as GameObject;
                    newChunk.transform.parent = terrain.transform;
                    Vector2 chunkPos = new Vector2(sceneChunks[i][sceneChunks[i].Count - 1].transform.position.x + chunkSize, 
                                                   sceneChunks[i][sceneChunks[i].Count - 1].transform.position.y);
                    newChunk.transform.position = chunkPos;
                    Vector2Int arrayPos = FindArrPos(chunkPos, worldSize);

                    newChunk.GetComponent<Chunk>().PopulateChunk(world, arrayPos, terrain, blockSys);

                    sceneChunks[i].Add(newChunk);

                }
            }

            else
            {
                for (int i = 0; i < sceneChunks.Count; i++)
                {
                    // deleting far right chunk
                    sceneChunks[i][sceneChunks[i].Count - 1].GetComponent<Chunk>().DestroyChunk();
                    Destroy(sceneChunks[i][sceneChunks[i].Count - 1]);
                    sceneChunks[i].RemoveAt(sceneChunks[i].Count - 1);

                    // adding new chunk on far left
                    GameObject newChunk = Instantiate(chunkPrefab) as GameObject;
                    newChunk.transform.parent = terrain.transform;
                    Vector2 chunkPos = new Vector2(sceneChunks[i][0].transform.position.x - chunkSize, sceneChunks[i][0].transform.position.y);
                    newChunk.transform.position = chunkPos;

                    Vector2Int arrayPos = FindArrPos(chunkPos, worldSize);
                    newChunk.GetComponent<Chunk>().PopulateChunk(world, arrayPos, terrain, blockSys);

                    sceneChunks[i].Insert(0, newChunk);

                }
            }
        }

        return sceneChunks;

    }

    private List<List<GameObject>> RedrawWorldY(List<List<int>> world, List<List<GameObject>> sceneChunks, float playerY, BlockSystem blockSys, float redrawDistance, float lastCharPosY)
    {
        // if character moved enough in y direction redraw world
        if (Math.Abs(playerY - lastCharPosY) > redrawDistance) 
        {
            sceneChunks.Insert(0, new List<GameObject>());

            if(playerY > lastCharPosY)
            {
                // add blocks above character and delete from below
                for(int i = 0; i < sceneChunks[1].Count; i++)
                {
                    // deleting lowest chunk
                    Vector2Int chunkToDelete = new Vector2Int(sceneChunks[sceneChunks.Count - 1].Count - 1, sceneChunks.Count - 1);
                    sceneChunks[chunkToDelete.y][chunkToDelete.x].GetComponent<Chunk>().DestroyChunk();
                    Destroy(sceneChunks[chunkToDelete.y][chunkToDelete.x]);
                    sceneChunks[chunkToDelete.y].RemoveAt(chunkToDelete.x);

                    // add a new chunk on top
                    GameObject newChunk = Instantiate(chunkPrefab) as GameObject;
                    newChunk.transform.parent = terrain.transform;
                    Vector2 chunkPos = new Vector2(sceneChunks[1][i].transform.position.x, sceneChunks[1][i].transform.position.y + chunkSize);
                    newChunk.transform.position = chunkPos;
                    Vector2Int arrayPos = FindArrPos(chunkPos, worldSize);
                    newChunk.GetComponent<Chunk>().PopulateChunk(world, arrayPos, terrain, blockSys);

                    sceneChunks[0].Add(newChunk);
                }
            }
            sceneChunks.RemoveAt(sceneChunks.Count - 1);

        }
        
        return sceneChunks;

    }

    private Vector2Int FindArrPos(Vector2 pos, Vector2Int worldSize)
    {
        return new Vector2Int((int)(pos.x + (worldSize.x / 2)), (int)Math.Abs(pos.y - (worldSize.y / 2)));
    }

}
