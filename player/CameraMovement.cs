using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //Object which camera will follow
    public Transform ObjectToFollow;


    private void Update()
    {
        //Follow that object
        Vector3 positionToFollow = ObjectToFollow.position;
        positionToFollow.z = transform.position.z;
        transform.position=positionToFollow;
    }
}