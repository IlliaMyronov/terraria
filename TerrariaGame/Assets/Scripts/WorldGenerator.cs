using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    // variables for specification of world generation
    // variables with multiplier ending are coeficients for perlin value
    // variables with size ending are maximum size of this objects in blocks
    [SerializeField] private int skySize;
    [SerializeField] private int groundSize;
    [SerializeField] private float hillMultiplier;
    [SerializeField] private float hillSize;
    [SerializeField] private int caveSize;
    [SerializeField] private float caveSizeMultiplier;
    [SerializeField] private float caveEdgeMultiplier;

    // variables for perlin noise
    private Vector2 perlinOffset;
    private float yForPerlinHill;
    private float yForPerlinCaveEdge;

    // 2D array list to store every block
    private List<List<int>> world = new List<List<int>>();
    private List<List<int>> background = new List<List<int>>();

    // variable that stores the size of the world
    private Vector2Int worldSize;

    // variable to store spawn location of a player
    private Vector2Int spawnLocation;

    private void Awake()
    {
        worldSize = new Vector2Int(256, skySize + groundSize + caveSize);
    }
    public void GenerateWorld()
    {

        // pick a random position to generate world
        perlinOffset = new Vector2(Random.Range(0f, 9999f), Random.Range(0f, 9999f));
        yForPerlinHill = Random.Range(0f, 99999f);
        yForPerlinCaveEdge = Random.Range(0f, 99999f);

        //loop that generates y values of the world
        for (int i = 0; i < worldSize.y; i++)
        {

            world.Add(new List<int>());
            background.Add(new List<int>());

            // loop that generates x values of the world
            for (int j = 0; j < worldSize.x; j++)
            {

                // check if current block is above hill, if so add empty block
                if (i < (skySize + (hillSize * GeneratePerlinValue(j, (int)yForPerlinHill, hillMultiplier)) - (hillSize / 2)))
                {
                    world[i].Add(-1);
                    background[i].Add(-1);
                }
                else
                {

                    // since the block is inside the cave we need background wall
                    background[i].Add(0);

                    // if current block is below ground layer fill it with cave block, else put ground layer.
                    if (i > skySize + groundSize - (hillSize * GeneratePerlinValue(j, (int)yForPerlinCaveEdge, caveEdgeMultiplier)))
                    {
                        if (GeneratePerlinValue(j, i, caveSizeMultiplier) < 0.5)
                        {
                            world[i].Add(1);
                        }
                        else
                        {
                            world[i].Add(-1);
                        }
                    }
                    else
                    {
                        world[i].Add(0); 
                    }

                }

            }

        }

        this.GenerateBlobs();

        this.findSpawnLocation();

    }

    private float GeneratePerlinValue(int x, int y, float scale)
    {
        return Mathf.PerlinNoise((((float)x / 1920) * scale + perlinOffset.x), 
                                 (((float)y / 1080) * scale + perlinOffset.y));
    }

    public List<List<int>> GetWorld()
    {
        return world;
    }

    public List<List<int>> GetBackground()
    {
        return background;
    }

    private void GenerateBlobs()
    {
        for (int i = 0; i < 10; i++)
        {
            ellipse newBlob = new ellipse(new Vector2(6, worldSize.x - 6), new Vector2(40, worldSize.y - 6), new Vector2(2, 6));

            for (int j = 0; j <= newBlob.GetRadii().y * 2; j++)
            {
                for (int l = 0; l <= newBlob.GetRadii().x * 2; l++)
                {
                    Vector2 pointCoordinates = new Vector2(newBlob.GetCenter().x - Mathf.Round(newBlob.GetRadii().x) + l, newBlob.GetCenter().y - Mathf.Round(newBlob.GetRadii().y) + j);

                    if (newBlob.IsInside(pointCoordinates))
                    {
                        if (world[(int)pointCoordinates.y][(int)pointCoordinates.x] != -1)
                        {
                            world[(int)pointCoordinates.y][(int)pointCoordinates.x] = 2;
                        }
                    }
                }
            }
        }
    }

    private void findSpawnLocation()
    {
        spawnLocation.x = worldSize.x / 2;

        int yLocationCounter = worldSize.y / 2;

        bool run = true;

        while (run)
        {

            if (background[yLocationCounter][spawnLocation.x] == -1)
            {
                yLocationCounter -= 5;
                run = false;
            }

            else
                yLocationCounter--;
        }

        spawnLocation.y = yLocationCounter;
    }

    public Vector2Int getSpawnLocation()
    {
        return spawnLocation;
    }

    public Vector2Int GetWorldSize()
    {
        return worldSize;
    }

    public void ChangeBlock(Vector2Int arrPos, int changeTo, bool isSolid)
    {
        if (isSolid)
        {
            world[arrPos.y][arrPos.x] = changeTo;
        }

        else
        {
            background[arrPos.y][arrPos.x] = changeTo;
        }
    }
}

public class ellipse
{
    private Vector2 coordinates;
    private Vector2Int radiuses;
    public ellipse() { }

    // initiallize random ellipse with given ranges
    public ellipse(Vector2 xRange, Vector2 yRange, Vector2 RadiusRange)
    {
        coordinates = new Vector2Int((int)Random.Range(xRange.x, xRange.y), (int)Random.Range(yRange.x, yRange.y));
        radiuses = new Vector2Int(Random.Range((int)RadiusRange.x, (int)RadiusRange.y), Random.Range((int)RadiusRange.x, (int)RadiusRange.y));
    }

    // check if given point is inside the ellipse
    public bool IsInside(Vector2 point)
    {
        // arteficially shifting center of the ellipse to coordinates (0, 0) for easier usage of ellipse formula
        point.x = point.x - coordinates.x;
        point.y = point.y - coordinates.y;

        // using ellipse formula to check if point lies inside ellise (formula is (x / xRadius)^2 + (y / yRadius)^2 = 1)
        if (Mathf.Pow(point.x / radiuses.x, 2) + Mathf.Pow(point.y / radiuses.y, 2) <= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Vector2 GetRadii()
    {
        return radiuses;
    }

    public Vector2 GetCenter()
    {
        return coordinates;
    }
}