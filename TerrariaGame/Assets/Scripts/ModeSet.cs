using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ModeSet : MonoBehaviour
{
    private string DestructionMode;
    private string BuildingMode;
    private bool isDestructionMode;

    void Awake()
    {
        DestructionMode = "Destruction Mode is active";
        BuildingMode = "Building Mode is active";
        this.GetComponent<TMPro.TextMeshProUGUI>().text = DestructionMode;
        isDestructionMode = true;
    }

    public void ChangeMode()
    {
        if (isDestructionMode)
            this.GetComponent<TMPro.TextMeshProUGUI>().text = BuildingMode;

        else
            this.GetComponent<TMPro.TextMeshProUGUI>().text = DestructionMode;

        isDestructionMode = !isDestructionMode;
    }

    public bool IsDestruction()
    {
        return isDestructionMode;
    }
}
