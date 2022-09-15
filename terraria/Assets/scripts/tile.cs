using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tile : MonoBehaviour
{

    private SpriteRenderer rend;

    [SerializeField]
    private Sprite mySprite;

    private void Awake()
    {

        rend = GetComponent<SpriteRenderer>();
        rend.sprite = mySprite;

    }
    
}
