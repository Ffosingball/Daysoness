using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private float speed;
    private float distancePassed=0f;
    [SerializeField] private float maxAllowedDistance = 30f;


    public void setSpeed(float _speed)
    {
        speed=_speed;
    }


    void Update()
    {
        transform.Translate(new Vector3(0,1,0)*speed*Time.deltaTime);
        distancePassed+=speed*Time.deltaTime;

        if (distancePassed>maxAllowedDistance)
            Destroy(gameObject);
    }
}
