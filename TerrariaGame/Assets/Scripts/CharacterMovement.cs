using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    private void Update()
    {
        Ray ray = new Ray(this.transform.position, Vector3.forward);
        RaycastHit2D hitDetector = Physics2D.Raycast(this.transform.position, Vector3.forward);
        Debug.DrawRay(this.transform.position, Vector3.forward * 100, Color.red);
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(hitDetector.distance + "      " + hitDetector.transform);
        }
        
       
    }
}
