using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Movement movement;
    private InputAction fireAction;
    [SerializeField] private  ItemManager itemManager;
    [SerializeField] private  UIManager uiManager;


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



    public void OnSwitchForward(InputValue value)
    {
        itemManager.SwitchWeaponForward();
    }



    public void OnSwitchBackward(InputValue value)
    {
        itemManager.SwitchWeaponBackward();
    }



    public void OnRecharge(InputValue value)
    {
        itemManager.RechargeWeapon();
    }



    public void OnHeal(InputValue value)
    {
        itemManager.UseFirstAid();
    }



    public void OnPause_Resume(InputValue value)
    {
        if(Time.deltaTime==0f)
            uiManager.Continue();
        else
            uiManager.Pause();
    }
}

