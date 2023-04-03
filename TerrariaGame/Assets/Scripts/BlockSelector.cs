using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSelector : MonoBehaviour
{
    [SerializeField] Camera cam;
    private Color color;
    private float SelectorOnTransparency;
    private float SelectorOffTransparency;

    private void Awake()
    {
        SelectorOnTransparency = 0.8f;
        SelectorOffTransparency = 0f;
        color = this.GetComponent<SpriteRenderer>().color;
        color.a = SelectorOffTransparency;
        this.GetComponent<SpriteRenderer>().color = color;
    }

    public void TurnOn(bool isOn)
    {
        if (isOn)
            color.a = SelectorOnTransparency;

        else
            color.a = SelectorOffTransparency;

        this.GetComponent<SpriteRenderer>().color = color;
    }
}
