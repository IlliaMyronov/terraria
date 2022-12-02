using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    [SerializeField] WorldLogic worldLogic;
    private void Awake()
    {
        
        worldLogic.CreateWorld();

    }
}
