using UnityEngine;

public class CalculatingYtoZRelation : MonoBehaviour
{
    //private Transform transform;

    //Between camera and grid
    [SerializeField] private float zRange=100f;
    //World size in unity units
    [SerializeField] private float worldSizeY=1000f;
    //Where 0 coordinate will be in the world
    [SerializeField] private float worldCenterAt=500f;
    //Flag if we want calculate z to y relation only once
    [SerializeField] private bool calculateOnce = true;


    private void CalculateZToYRelation()
    {
        Vector3 position = transform.position;
        position.z = -1*(zRange-(position.y+worldCenterAt)*(zRange/worldSizeY));
        transform.position=position;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //transform = GetComponent<Transform>();

        CalculateZToYRelation();

        if(calculateOnce)
            Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateZToYRelation();
    }
}
