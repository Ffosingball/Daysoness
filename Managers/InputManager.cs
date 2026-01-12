using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //References to other components
    [SerializeField] private Movement movement;
    [SerializeField] private  ItemManager itemManager;
    [SerializeField] private  UIManager uiManager;
    //Epsilon for float equality comparisons
    [SerializeField] private float epsilon=0.001f;



    public void OnMove(InputAction.CallbackContext ctx)
    {
        //Always set movement direaction
        if(Time.timeScale>epsilon)
            movement.setMovementDirection(ctx.ReadValue<Vector2>());
    }



    public void OnFire(InputAction.CallbackContext ctx)
    {
        //Check when button is pressed and when is released
        //Important for automatic firearms and meele weapons
        if(ctx.started)
            OnFirePressed();

        if(ctx.canceled)
            OnFireReleased();
    }



    public void OnFirePressed()
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



    public void OnFireReleased()
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
        //Switch weapon for the next in the list
        if(Time.timeScale>epsilon)
        {
            if(ctx.started)
                itemManager.SwitchWeaponForward();
        }
    }



    public void OnSwitchBackward(InputAction.CallbackContext ctx)
    {
        //Switch weapon for the previous in the list
        if(Time.timeScale>epsilon)
        {
            if(ctx.started)
                itemManager.SwitchWeaponBackward();
        }
    }



    public void OnRecharge(InputAction.CallbackContext ctx)
    {
        //Reload weapon
        if(Time.timeScale>epsilon)
        {
            if(ctx.started)
                itemManager.RechargeWeapon();
        }
    }



    public void OnHeal(InputAction.CallbackContext ctx)
    {
        //Start using first aid to increase health
        if(Time.timeScale>epsilon)
        {
            if(ctx.started)
                itemManager.StartUsingFirstAid();
        }
    }


    //Checks whether game is paused or not and then 
    public void PauseResume()
    {
        if(Time.timeScale<epsilon)
        {
            //Continue game and stops movement or fire if still works
            uiManager.Continue();
            movement.setMovementDirection(Vector2.zero);
            OnFireReleased();
        }
        else
            uiManager.Pause();
    }



    public void OnPause_Resume(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            PauseResume();
        }
    }
}

