using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Movement movement;
    [SerializeField] private  ItemManager itemManager;
    [SerializeField] private  UIManager uiManager;
    [SerializeField] private float epsilon=0.001f;



    public void OnMove(InputAction.CallbackContext ctx)
    {
        if(Time.timeScale>epsilon)
            movement.setMovementDirection(ctx.ReadValue<Vector2>());
    }



    public void OnFire(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
            OnFirePressed(ctx);

        if(ctx.canceled)
            OnFireReleased(ctx);
    }



    public void OnFirePressed(InputAction.CallbackContext ctx)
    {
        if(Time.timeScale>epsilon)
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
    }



    public void OnFireReleased(InputAction.CallbackContext ctx)
    {
        if(Time.timeScale>epsilon)
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



    public void OnSwitchForward(InputAction.CallbackContext ctx)
    {
        if(Time.timeScale>epsilon)
        {
            //Debug.Log("Pressed forward");
            if(ctx.started)
                itemManager.SwitchWeaponForward();
        }
    }



    public void OnSwitchBackward(InputAction.CallbackContext ctx)
    {
        if(Time.timeScale>epsilon)
        {
            if(ctx.started)
                itemManager.SwitchWeaponBackward();
        }
    }



    public void OnRecharge(InputAction.CallbackContext ctx)
    {
        if(Time.timeScale>epsilon)
        {
            if(ctx.started)
                itemManager.RechargeWeapon();
        }
    }



    public void OnHeal(InputAction.CallbackContext ctx)
    {
        if(Time.timeScale>epsilon)
        {
            if(ctx.started)
                itemManager.StartUsingFirstAid();
        }
    }



    public void OnPause_Resume(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            if(Time.timeScale<epsilon)
                uiManager.Continue();
            else
                uiManager.Pause();
        }
    }
}

