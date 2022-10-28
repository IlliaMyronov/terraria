using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class worldController : MonoBehaviour
{
    // reference to scripts to control the world
    [SerializeField]
    private WorldModel worldModel;

    private string buttonHeld;
    private string notification;

    private void Start()
    {
        worldModel.generateWorld();
        worldModel.drawWorld();

    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A) && buttonHeld == "A")
            buttonHeld = string.Empty;

        if (Input.GetKeyUp(KeyCode.D) && buttonHeld == "D")
            buttonHeld = string.Empty;

        if (Input.GetKeyDown(KeyCode.A) == true && buttonHeld != "A")
        {
            buttonHeld = "A";
            notification = "moveLeft";
            worldModel.move(notification);
            Debug.Log("Move left");
        }

        else if (Input.GetKeyDown(KeyCode.D) == true && buttonHeld != "D")
        {

            buttonHeld = "D";
            notification = "moveRight";
            worldModel.move(notification);
            Debug.Log("move right");
        }

        else if (buttonHeld != "A" && buttonHeld != "D" && buttonHeld != "stop")
        {
            buttonHeld = "stop";
            notification = "stop";
            worldModel.move(notification);
            Debug.Log("stop");
        }

        if (Input.GetMouseButtonDown(0))
        {
            worldModel.destroyBlock(Input.mousePosition);
            Debug.Log("mouse key pressed");
        }

    }

}