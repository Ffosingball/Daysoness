using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public Movement movement;

    /*public void OnZoom(InputValue value)
    {
        events.CallOnCameraZoom(value.Get<Vector2>());
    }*/


    public void OnMove(InputValue value)
    {
        movement.setMovementDirection(value.Get<Vector2>());
    }
}

