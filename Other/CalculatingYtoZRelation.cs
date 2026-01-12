using UnityEngine;

//This code calculates z position for the gameObject, so all objects in the world
//will be drew correctly objects lower by y coordinate will be in front of the objects 
//which have higher y coordinate
public class CalculatingYtoZRelation : MonoBehaviour
{
    //Distance between camera and grid
    [SerializeField] private float zRange=100f;
    //World size in unity units
    [SerializeField] private float worldSizeY=1000f;
    //Where 0 coordinate will be in the world
    [SerializeField] private float worldCenterAt=500f;
    //Flag if we want calculate z to y relation only once
    [SerializeField] private bool calculateOnce = true;


    private void CalculateZToYRelation()
    {
        //Calculate correct z position
        Vector3 position = transform.position;
        position.z = -1*(zRange-(position.y+worldCenterAt)*(zRange/worldSizeY));
        transform.position=position;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CalculateZToYRelation();

        if(calculateOnce)
            Destroy(this);
    }


    void Update()
    {
        CalculateZToYRelation();
    }
}
