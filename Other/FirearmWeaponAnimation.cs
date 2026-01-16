using UnityEngine;
using System;

public class FirearmWeaponAnimation : MonoBehaviour
{
    //Weapon position for different player states and directions
    [SerializeField] private Vector3 weaponUpPosition;
    [SerializeField] private Vector3 weaponDownPosition;
    [SerializeField] private Vector3 weaponRightPosition;
    [SerializeField] private Vector3 weaponLeftPosition;
    [SerializeField] private Vector3 weaponLongUpPosition;
    [SerializeField] private Vector3 weaponLongDownPosition;
    [SerializeField] private Vector3 weaponLongRightPosition;
    [SerializeField] private Vector3 weaponLongLeftPosition;
    [SerializeField] private Vector3 weaponFiringUpPosition;
    [SerializeField] private Vector3 weaponFiringDownPosition;
    //All weapon sprites
    [SerializeField] private Sprite horizontalWithMagazine;
    [SerializeField] private Sprite verticalWithMagazine;
    [SerializeField] private Sprite horizontalWithoutMagazine;
    [SerializeField] private Sprite verticalWithoutMagazine;
    //Reference to player animation
    [SerializeField] private PlayerAnimations playerAnimation;
    //When weapon is showed 
    [SerializeField] private float scaleDecreaseToVertical=0.7f;
    [SerializeField] private bool uprightWhenGoUpOrDown=false;
    [SerializeField] private float epsilon=0.001f;


    private SpriteRenderer spriteRenderer;
    //private FirearmWeapon playerAnimation;
    private bool withoutMagazines=true;
    private float initScale;



    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = horizontalWithoutMagazine;
        initScale = transform.localScale.x;
    }



    void OnEnable()
    {
        EventsManager.OnNewWeaponAcquired+=TurnOnWeaponRenderer;
    }



    void OnDisable()
    {
        EventsManager.OnNewWeaponAcquired-=TurnOnWeaponRenderer;
    }



    private void TurnOnWeaponRenderer()
    {
        if(spriteRenderer==null)
            spriteRenderer=GetComponent<SpriteRenderer>();
        spriteRenderer.enabled=true;
    }



    public void StartedRecharge()
    {
        withoutMagazines=true;
    }



    public void FinishedRecharge()
    {
        withoutMagazines=false;
    }



    void Update()
    {
        if(playerAnimation.isAttackingAnimation())
            FiringAnimation();
        else
        {
            if(playerAnimation.isLongWaitAnimation())
                IdleLongAnimation();
            else
                IdleAnimation();
        }
    }



    private void FiringAnimation()
    {
        transform.rotation = Quaternion.Euler(0f,0f,playerAnimation.getCurrentAngle());
        Vector3 currentScale = transform.localScale;

        switch(playerAnimation.getDirection())
        {
             case Directions.Up:
             case Directions.BackwardsDown:
                transform.localPosition = weaponFiringUpPosition;
                spriteRenderer.flipY=false;

                currentScale.x = initScale*scaleDecreaseToVertical;
                transform.localScale = currentScale;

                if(withoutMagazines)
                    spriteRenderer.sprite=verticalWithoutMagazine;
                else
                    spriteRenderer.sprite=verticalWithMagazine;

                break;
            case Directions.Down:
            case Directions.BackwardsUp:
                transform.localPosition = weaponFiringDownPosition;
                spriteRenderer.flipY=false;

                currentScale.x = initScale*scaleDecreaseToVertical;
                transform.localScale = currentScale;

                if(withoutMagazines)
                    spriteRenderer.sprite=verticalWithoutMagazine;
                else
                    spriteRenderer.sprite=verticalWithMagazine;

                break;
            case Directions.Left:
            case Directions.BackwardsRight:
                transform.localPosition = weaponLeftPosition;
                spriteRenderer.flipY=true;

                currentScale.x = initScale;
                transform.localScale = currentScale;

                if(withoutMagazines)
                    spriteRenderer.sprite=horizontalWithoutMagazine;
                else
                    spriteRenderer.sprite=horizontalWithMagazine;

                break;
            case Directions.Right:
            case Directions.BackwardsLeft:
                transform.localPosition = weaponRightPosition;
                spriteRenderer.flipY=false;

                currentScale.x = initScale;
                transform.localScale = currentScale;

                if(withoutMagazines)
                    spriteRenderer.sprite=horizontalWithoutMagazine;
                else
                       spriteRenderer.sprite=horizontalWithMagazine;

                break;
            }
    }



    private void IdleAnimation()
    {
        Vector3 currentScale = transform.localScale;
            
        switch(playerAnimation.getDirection())
        {
            case Directions.Up:
                spriteRenderer.flipY=true;

                if(!uprightWhenGoUpOrDown)
                    transform.rotation = Quaternion.Euler(0f,0f,180f);
                else
                    transform.rotation = Quaternion.Euler(0f,0f,90f);

                transform.localPosition = weaponUpPosition;
                break;
            case Directions.Down:
                spriteRenderer.flipY=false;

                if(!uprightWhenGoUpOrDown)
                    transform.rotation = Quaternion.Euler(0f,0f,0f);
                else
                    transform.rotation = Quaternion.Euler(0f,0f,-90f);

                transform.localPosition = weaponDownPosition;
                break;
            case Directions.Left:
                spriteRenderer.flipY=true;
                transform.rotation = Quaternion.Euler(0f,0f,180f);
                transform.localPosition = weaponLeftPosition;
                break;
            case Directions.Right:
                spriteRenderer.flipY=false;
                transform.rotation = Quaternion.Euler(0f,0f,0f);
                transform.localPosition = weaponRightPosition;
                break;
        }

        if(withoutMagazines)
        {
            //Debug.Log("Rot: "+transform.rotation.eulerAngles.z);
            if(Mathf.Abs(transform.rotation.eulerAngles.z-90f)<epsilon || Mathf.Abs(transform.rotation.eulerAngles.z-270f)<epsilon)
            {
                spriteRenderer.sprite=verticalWithoutMagazine;
                currentScale.x = initScale*scaleDecreaseToVertical;
                transform.localScale = currentScale;
            }
            else
            {
                spriteRenderer.sprite=horizontalWithoutMagazine;
                currentScale.x = initScale;
                transform.localScale = currentScale;
            }
        }
        else
        {
            //Debug.Log("Rot: "+transform.rotation.eulerAngles.z);
            if(Mathf.Abs(transform.rotation.eulerAngles.z-90f)<epsilon || Mathf.Abs(transform.rotation.eulerAngles.z-270f)<epsilon)
            {
                spriteRenderer.sprite=verticalWithMagazine;
                currentScale.x = initScale*scaleDecreaseToVertical;
                transform.localScale = currentScale;
            }
            else
            {
                spriteRenderer.sprite=horizontalWithMagazine;
                currentScale.x = initScale;
                transform.localScale = currentScale;
            }
        }
    }



    private void IdleLongAnimation()
    {
        Vector3 currentScale = transform.localScale;
        spriteRenderer.flipY=false;

            switch(playerAnimation.getDirection())
            {
                case Directions.Up:
                    transform.localPosition = weaponLongUpPosition;

                    currentScale.x = initScale;
                    transform.localScale = currentScale;

                    transform.rotation = Quaternion.Euler(0f,0f,90f);
                    if(withoutMagazines)
                        spriteRenderer.sprite=horizontalWithoutMagazine;
                    else
                        spriteRenderer.sprite=horizontalWithMagazine;

                    break;
                case Directions.Down:
                    transform.localPosition = weaponLongDownPosition;

                    currentScale.x = initScale;
                    transform.localScale = currentScale;

                    transform.rotation = Quaternion.Euler(0f,0f,90f);
                    if(withoutMagazines)
                        spriteRenderer.sprite=horizontalWithoutMagazine;
                    else
                        spriteRenderer.sprite=horizontalWithMagazine;

                    break;
                case Directions.Left:
                    transform.localPosition = weaponLongLeftPosition;

                    currentScale.x = initScale*scaleDecreaseToVertical;
                    transform.localScale = currentScale;

                    transform.rotation = Quaternion.Euler(0f,0f,90f);
                    if(withoutMagazines)
                        spriteRenderer.sprite=verticalWithoutMagazine;
                    else
                        spriteRenderer.sprite=verticalWithMagazine;

                    break;
                case Directions.Right:
                    transform.localPosition = weaponLongRightPosition;

                    currentScale.x = initScale*scaleDecreaseToVertical;
                    transform.localScale = currentScale;

                    transform.rotation = Quaternion.Euler(0f,0f,90f);
                    if(withoutMagazines)
                        spriteRenderer.sprite=verticalWithoutMagazine;
                    else
                        spriteRenderer.sprite=verticalWithMagazine;

                    break;
            }
    }
}
