using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_fly : MonoBehaviour
{
    public float speed=10f;
    public Renderer rend;
    private int counter=0;

    private void Start() 
    {
        //gameObject.SetActive(true);
        counter++;
    }


    void FixedUpdate()
    {
        transform.Translate(new Vector3(0,1,0)*speed*Time.deltaTime);
        counter++;

        if (counter>100)
            DestroyImmediate(gameObject);
    }
}
