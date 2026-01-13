using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //Speed with which camera will change its size
    [SerializeField] private float zoomSpeed;
    //x minimal zoom value, y maximum zoom value
    [SerializeField] private Vector2 zoomConfines;
    //This is needed so player will exactly at the cnter of the screen
    [SerializeField] private float yOffset;

    //Object which camera will follow
    public Transform ObjectToFollow;
    //Last frame camera zoom change
    private Vector2 deltaCameraZoom;
    private Camera _camera;


    //Setters
    public void setDeltaCameraZoom(Vector2 _deltaCameraZoom)
    {
        deltaCameraZoom=_deltaCameraZoom;
    }



    private void Start()
    {
        _camera = GetComponent<Camera>();
    }



    private void Update()
    {
        //Follow that object
        Vector3 positionToFollow = ObjectToFollow.position;
        positionToFollow.z = transform.position.z;
        positionToFollow.y = positionToFollow.y+yOffset;
        transform.position=positionToFollow;
        
        //Resize camera size
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize + (-deltaCameraZoom.y*zoomSpeed*Time.deltaTime),zoomConfines.x,zoomConfines.y);
    }
}