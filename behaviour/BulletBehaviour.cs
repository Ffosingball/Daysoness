using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private float speed;
    private float dmg;
    private float distancePassed=0f;
    [SerializeField] private float maxAllowedDistance = 30f;


    public void setSpeed(float _speed)
    {
        speed=_speed;
    }


    public void setDMG(float _dmg)
    {
        dmg=_dmg;
    }


    public float getDMG()
    {
        return dmg;
    }


    void Update()
    {
        transform.Translate(new Vector3(0,1,0)*speed*Time.deltaTime);
        distancePassed+=speed*Time.deltaTime;

        if (distancePassed>maxAllowedDistance)
            Destroy(gameObject);
    }
}
