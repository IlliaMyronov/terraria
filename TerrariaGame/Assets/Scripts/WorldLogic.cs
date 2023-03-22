using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WorldLogic : MonoBehaviour
{
    [SerializeField] WorldGenerator worldGenerator;
    [SerializeField] WorldDrawer worldDrawer;
    [SerializeField] Vector2Int chunksPerScreen;
    [SerializeField] BlockSystem blockSys;
    [SerializeField] Camera cam;
    [SerializeField] float redrawDistance;
    [SerializeField] GameObject character;

    private List<List<GameObject>> scene;
    private List<List<int>> world;
    private List<List<int>> background;
    private Vector2 lastRedrawPos;
    private Vector2 resolution;

    private void Awake()
    {

        scene = new List<List<GameObject>>();
        world = worldGenerator.GetWorld();
        background = worldGenerator.GetBackground();
        background = worldGenerator.GetBackground();

        // currently set to standard resolution make editable later
        resolution = new Vector2(1920, 1080);

        Physics2D.IgnoreLayerCollision(12, 11);

    }

    public void CreateWorld()
    {
        blockSys.GenerateList();
        worldGenerator.GenerateWorld();
        cam.transform.position = new Vector3(worldGenerator.getSpawnLocation().x, worldGenerator.getSpawnLocation().y, cam.transform.position.z);
        worldDrawer.DrawWorld(world, background, scene, chunksPerScreen, worldGenerator.getSpawnLocation(), blockSys);
        lastRedrawPos = cam.transform.position;
        character.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, 0);
    }

    private void Update()
    {

        // redraw the world if we got close to an edge
        if ((Math.Abs(Math.Abs(cam.transform.position.x) - Math.Abs(lastRedrawPos.x)) > redrawDistance ||
            Math.Abs(cam.transform.position.y - lastRedrawPos.y) > redrawDistance))
        {
            Debug.Log("sending redraw request, camera pos is " + cam.transform.position);
            worldDrawer.RedrawWorld(world, background, scene, cam.transform.position, lastRedrawPos, blockSys, redrawDistance);
            lastRedrawPos = cam.transform.position;
        }

        // LMB is pressed so block should be destroyed
        if (Input.GetMouseButtonDown(0)) 
        {
            //finding camera dimensions in units
            Vector2 camSize = new Vector2(2 * cam.orthographicSize * cam.aspect, 2 * cam.orthographicSize);

            Vector2 clickPos = cam.ScreenToWorldPoint(Input.mousePosition);

            Vector2Int chunkClickedOn = new Vector2Int(Mathf.FloorToInt((clickPos.x - scene[0][0].transform.position.x) / worldDrawer.GetChunkSize()), Mathf.FloorToInt((scene[0][0].transform.position.y  - clickPos.y) / worldDrawer.GetChunkSize()));

            Debug.Log(new Vector2Int(Mathf.FloorToInt(clickPos.x - scene[chunkClickedOn.y][chunkClickedOn.x].transform.position.x + 0.5f), Mathf.FloorToInt(scene[chunkClickedOn.y][chunkClickedOn.x].transform.position.y - clickPos.y + 0.5f)));
            scene[chunkClickedOn.y][chunkClickedOn.x].GetComponent<Chunk>().DestroyMyBlock(new Vector2Int(Mathf.FloorToInt(clickPos.x - scene[chunkClickedOn.y][chunkClickedOn.x].transform.position.x + 0.5f), Mathf.FloorToInt(scene[chunkClickedOn.y][chunkClickedOn.x].transform.position.y - clickPos.y + 0.5f)), true);
        }
    }
}






/*
              
            Debug.Log(Input.mousePosition);

            

            //finding array position of block to destroy in the world list.
            Vector2Int blockToDestroy = new Vector2Int((int)(cam.transform.position.x - camSize.x / 2 + Math.Round(camSize.x * (Input.mousePosition.x / resolution.x) + 0.5)), 
                                                       (int)(Mathf.Abs(cam.transform.position.y - (world.Count + 1)) - Math.Round(camSize.y * (Input.mousePosition.y / resolution.y) + 0.5)));

            if (world[blockToDestroy.y][blockToDestroy.x] != -1) 
            {

                // set needed block in world array to -1
                world[blockToDestroy.y][blockToDestroy.x] = -1;

                // delete needed block from the screen.
                Vector2Int topLeftChunk = scene[0][0].GetComponent<Chunk>().myArrayPos;

                Vector2Int blockToChunkDistance = blockToDestroy - topLeftChunk;

                Vector2Int chunkToDeleteFrom = new Vector2Int(Mathf.FloorToInt(blockToChunkDistance.x / worldDrawer.GetChunkSize()),
                                                              Mathf.FloorToInt(blockToChunkDistance.y / worldDrawer.GetChunkSize()));

                scene[chunkToDeleteFrom.y][chunkToDeleteFrom.x].GetComponent<Chunk>().DestroyMyBlock
                     (new Vector2Int(blockToChunkDistance.x - (chunkToDeleteFrom.x * worldDrawer.GetChunkSize()),
                                     blockToChunkDistance.y - (chunkToDeleteFrom.y * worldDrawer.GetChunkSize())), true);

            }

*/