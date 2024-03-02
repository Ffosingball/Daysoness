using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tree_b : MonoBehaviour
{
    public float treeType=1.1f;
    
    /*private void awake()
    {
        player = GetComponent<GameObject>();
    }*/

    private void Update() 
    {
        if(Movement.currentPos.y>=transform.position.y-treeType)
        {
            //Debug.Log("Pos: "+Movement.currentPos.y);
            Vector3 currentPosition = transform.position;
            currentPosition.z = -1f;
            transform.position = currentPosition;
        }
        else
        {
            Vector3 currentPosition = transform.position;
            currentPosition.z = -0.5f;
            transform.position = currentPosition;
        }
    }
}
