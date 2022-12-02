using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLogic : MonoBehaviour
{
    [SerializeField] WorldGenerator worldGenerator;
    [SerializeField] WorldDrawer worldDrawer;
    [SerializeField] Vector2Int chunksPerScreen;
    [SerializeField] BlockSystem blockSys;
    [SerializeField] Camera cam;
    [SerializeField] float redrawDistance;

    private List<List<GameObject>> scene;
    private List<List<int>> world;
    private Vector2 lastRedrawPos;

    private void Awake()
    {

        scene = new List<List<GameObject>>();
        world = worldGenerator.getWorld();
        lastRedrawPos = new Vector2(0, 0);
        
    }

    public void CreateWorld()
    {
        blockSys.GenerateList();
        worldGenerator.GenerateWorld();
        worldDrawer.DrawWorld(world, scene, chunksPerScreen, blockSys);
    }

    private void Update()
    {
        if (Math.Abs(Math.Abs(cam.transform.position.x) - Math.Abs(lastRedrawPos.x)) > redrawDistance ||
            Math.Abs(cam.transform.position.y - lastRedrawPos.y) > redrawDistance)
        {
            Debug.Log("sending redraw request, camera pos is " + cam.transform.position);
            worldDrawer.RedrawWorld(world, scene, cam.transform.position, lastRedrawPos, blockSys, redrawDistance);
            lastRedrawPos = cam.transform.position;
        }
    }
}
