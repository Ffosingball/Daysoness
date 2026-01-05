using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Movement movement;
    [SerializeField] private InputAction fireAction;
    [SerializeField] private  ItemManager itemManager;

    private void Start()
    {
        fireAction.started += OnFirePressed;
        fireAction.canceled += OnFireReleased;
    }

    public void OnMove(InputValue value)
    {
        movement.setMovementDirection(value.Get<Vector2>());
    }

    public void OnFirePressed(InputAction.CallbackContext ctx)
    {
        GameObject currentWeapon = itemManager.getCurrentWeapon();

        if(currentWeapon.TryGetComponent<FirearmWeapon>(out FirearmWeapon firearmWeapon))
        {
            firearmWeapon.StartFire();
        }
        else
        {
            currentWeapon.GetComponent<MeeleWeapon>().StartSwing();
        }
    }

    public void OnFireReleased(InputAction.CallbackContext ctx)
    {
        GameObject currentWeapon = itemManager.getCurrentWeapon();

        if(currentWeapon.TryGetComponent<FirearmWeapon>(out FirearmWeapon firearmWeapon))
        {
            firearmWeapon.StopFire();
        }
        else
        {
            currentWeapon.GetComponent<MeeleWeapon>().StopSwing();
        }
    }
}

