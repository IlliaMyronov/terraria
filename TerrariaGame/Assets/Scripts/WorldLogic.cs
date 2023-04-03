using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WorldLogic : MonoBehaviour
{
    [SerializeField] WorldGenerator worldGenerator;
    [SerializeField] WorldDrawer worldDrawer;
    [SerializeField] BlockSystem blockSys;
    [SerializeField] GameObject modeSetter;
    [SerializeField] Camera cam;
    [SerializeField] float redrawDistance;
    [SerializeField] GameObject character;
    [SerializeField] Vector2Int chunksPerScreen;
    [SerializeField] GameObject blockSelector;

    private List<List<GameObject>> scene;
    private List<List<int>> world;
    private List<List<int>> background;
    private Vector2 lastRedrawPos;
    private Vector2 resolution;
    private Vector3 lastMousePos;

    private void Awake()
    {

        scene = new List<List<GameObject>>();
        world = worldGenerator.GetWorld();
        background = worldGenerator.GetBackground();
        background = worldGenerator.GetBackground();

        // currently set to standard resolution make editable later
        resolution = new Vector2(1920, 1080);

        Physics2D.IgnoreLayerCollision(12, 11);

        lastMousePos = Input.mousePosition;
    }

    public void CreateWorld()
    {
        blockSys.GenerateList();
        worldGenerator.GenerateWorld();
        worldDrawer.DrawWorld(world, background, scene, chunksPerScreen, worldGenerator.getSpawnLocation(), blockSys);

        character.transform.position = new Vector3(worldGenerator.getSpawnLocation().x, worldGenerator.getSpawnLocation().y, 0);
        cam.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, cam.transform.position.z);

        blockSelector.GetComponent<BlockSelector>().TurnOn(true);
        lastRedrawPos = cam.transform.position;

        character.GetComponent<SpriteRenderer>().sortingLayerName = "AboveSolid";
    }

    private void Update()
    {

        cam.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, cam.transform.position.z);
        // redraw the world if we got close to an edge
        if ((Math.Abs(Math.Abs(cam.transform.position.x) - Math.Abs(lastRedrawPos.x)) > redrawDistance ||
            Math.Abs(cam.transform.position.y - lastRedrawPos.y) > redrawDistance))
        {
            worldDrawer.RedrawWorld(world, background, scene, cam.transform.position, lastRedrawPos, blockSys, redrawDistance);
            lastRedrawPos = cam.transform.position;
        }

        // LMB is pressed so block should be destroyed
        if (Input.GetMouseButtonDown(0)) 
        {
            this.BuildDestroy(modeSetter.GetComponent<ModeSet>().IsDestruction());
        }

        if (Input.GetKeyDown(KeyCode.E) == true)
        {
            modeSetter.GetComponent<ModeSet>().ChangeMode();
        }

        if(Input.mousePosition != lastMousePos)
        {
            this.UpdateBlockSelector();
        }
    }

    private void BuildDestroy(bool isDestroying)
    {
        Vector2 clickPos = cam.ScreenToWorldPoint(Input.mousePosition);

        Vector2Int chunkClickedOn = new Vector2Int(Mathf.FloorToInt((clickPos.x - scene[0][0].transform.position.x + 0.5f) / worldDrawer.GetChunkSize()), Mathf.FloorToInt((scene[0][0].transform.position.y - clickPos.y + 0.5f) / worldDrawer.GetChunkSize()));

        Vector2Int posInChunk = new Vector2Int(Mathf.FloorToInt(clickPos.x - scene[chunkClickedOn.y][chunkClickedOn.x].transform.position.x + 0.5f),
                                               Mathf.FloorToInt(scene[chunkClickedOn.y][chunkClickedOn.x].transform.position.y - clickPos.y + 0.5f));

        Vector2Int arrPos = this.TransformWorldCoordinatesIntoArr(new Vector2(scene[chunkClickedOn.y][chunkClickedOn.x].transform.position.x + posInChunk.x, scene[chunkClickedOn.y][chunkClickedOn.x].transform.position.y - posInChunk.y));

        if (isDestroying)
        {

            if (posInChunk.x == 16)
                posInChunk.x = 0;
            if (posInChunk.y == 16)
                posInChunk.y = 0;

            scene[chunkClickedOn.y][chunkClickedOn.x].GetComponent<Chunk>().DestroyMyBlock(posInChunk, true);
            worldGenerator.ChangeBlock(arrPos, -1, true);
        }

        else
        {
            scene[chunkClickedOn.y][chunkClickedOn.x].GetComponent<Chunk>().CreateNewBlock(posInChunk, true, "dirt", blockSys.allBlocks[0].GetSprite());

            worldGenerator.ChangeBlock(arrPos, 0, true);
        }
    }

    private void UpdateBlockSelector()
    {
        blockSelector.transform.position = new Vector3Int(Mathf.FloorToInt(cam.ScreenToWorldPoint(Input.mousePosition).x + 0.5f), Mathf.FloorToInt(cam.ScreenToWorldPoint(Input.mousePosition).y + 0.5f), -1);
        
        Vector2Int arrPos = this.TransformWorldCoordinatesIntoArr(blockSelector.transform.position);

        if (modeSetter.GetComponent<ModeSet>().IsDestruction())
        {

            if (worldGenerator.GetWorld()[arrPos.y][arrPos.x] != -1)
            {
                blockSelector.GetComponent<BlockSelector>().TurnOn(true);
            }

            else
            {
                blockSelector.GetComponent<BlockSelector>().TurnOn(false);
            }

        }

        else
        {

            if (worldGenerator.GetWorld()[arrPos.y][arrPos.x] == -1)
            {
                blockSelector.GetComponent<BlockSelector>().TurnOn(true);
            }

            else
            {
                blockSelector.GetComponent<BlockSelector>().TurnOn(false);
            }
        }
    }

    private Vector2Int TransformWorldCoordinatesIntoArr(Vector3 coordinates)
    {
        // IMPORTANT figure out what the hell - 10 is doing in y value of vector 2
        return new Vector2Int(Mathf.FloorToInt(coordinates.x + 0.5f), Mathf.FloorToInt(Mathf.Abs(coordinates.y - worldGenerator.GetWorldSize().y) + 0.5f) - 2*((worldGenerator.GetWorldSize().y / 2) - worldGenerator.getSpawnLocation().y));
    }
}
