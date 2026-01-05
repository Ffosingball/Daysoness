using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform ObjectToFollow;
    private Transform transform;


    private void Start()
    {
        transform = GetComponent<Transform>();
    }


    private void Update()
    {
        Vector3 positionToFollow = ObjectToFollow.position;
        positionToFollow.z = transform.position.z;
        transform.position=positionToFollow;
    }
}