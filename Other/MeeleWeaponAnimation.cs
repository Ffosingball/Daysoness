using UnityEngine;

public class MeeleWeaponAnimation : MonoBehaviour
{
    [SerializeField] private Vector3 weaponUsualPosition;
    [SerializeField] private Vector3 weaponMoveDownPosition;
    [SerializeField] private Vector3 weaponLongUpPosition;
    [SerializeField] private Vector3 weaponLongDownPosition;
    [SerializeField] private Vector3 weaponLongRightPosition;
    [SerializeField] private Vector3 weaponLongLeftPosition;
    [SerializeField] private Vector3 weaponAttackDownPosition;
    [SerializeField] private float timeToCancelSwingAnimation=1f;
    [SerializeField] private PlayerAnimations playerAnimation;
    //[SerializeField] private float epsilon=0.001f;
    [SerializeField] private float swingAngle=90f;
    [SerializeField] private float forwardSwingDistance=0.3f;
    [SerializeField] private SpriteRenderer weaponPicture;
    [SerializeField] private Transform weaponLocalTransform;


    private float timePassedSinceSwing=0f;
    private float swiningPassed=0f;
    private bool needToSwing=false;
    private float lastSavedRotation;
    private float swingPeriod;
    private float lastSavedXCooord;



    void Start()
    {
        swingPeriod=GetComponent<MeeleWeapon>().getSwingPeriod();
        EventsManager.OnStopFire+=StopSwing;
        EventsManager.OnStartFire+=StartSwing;
    }


    void OnEnable()
    {
        timePassedSinceSwing+=timeToCancelSwingAnimation;
        EventsManager.OnNewWeaponAcquired+=TurnOnWeaponRenderer;
    }



    void OnDisable()
    {
        EventsManager.OnNewWeaponAcquired-=TurnOnWeaponRenderer;
    }



    private void TurnOnWeaponRenderer()
    {
        weaponPicture.enabled=true;
    }



    public void StartSwing()
    {
        if(!needToSwing)
        {
            swiningPassed=0f;
            needToSwing=true;
            lastSavedRotation=playerAnimation.getCurrentAngle();
        }
    }



    public void StopSwing()
    {
        timePassedSinceSwing=0f;
    }



    void Update()
    {
        timePassedSinceSwing+=Time.deltaTime;
        swiningPassed+=Time.deltaTime;

        if(playerAnimation.isAttacking() || timePassedSinceSwing<timeToCancelSwingAnimation)
            SwingingAnimation();
        else
        {
            if(playerAnimation.isLongWaitAnimation())
                IdleLongAnimation();
            else
                IdleAnimation();
        }
    }



    private void SwingingAnimation()
    {
        switch(playerAnimation.getDirection())
        {
             case Directions.Up:
             case Directions.BackwardsDown:
                weaponUsualPosition.z=0.1f;
                transform.localPosition = weaponUsualPosition;
                break;
            default:
                weaponUsualPosition.z=-0.1f;
                transform.localPosition = weaponUsualPosition;
                break;
        }

        if(needToSwing)
        {
            //Swing down
            if(swiningPassed<swingPeriod/2f)
            {
                float currentRotation=lastSavedRotation+Mathf.Lerp(0,swingAngle,swiningPassed/(swingPeriod/2f))-(swingAngle/2f);
                //Debug.Log("currentRotation "+currentRotation+"; passed: "+swiningPassed+"; swingPeriod: "+swingPeriod);
                transform.rotation=Quaternion.Euler(0,0,currentRotation);
                lastSavedXCooord = weaponLocalTransform.localPosition.x;
            }
            else if(swiningPassed<swingPeriod)
            {
                transform.rotation=Quaternion.Euler(0,0,lastSavedRotation);
                float currentXCoord;

                //Swing forward
                if(swiningPassed<swingPeriod*0.75f)
                {
                    currentXCoord = lastSavedXCooord+Mathf.Lerp(0,forwardSwingDistance,(swiningPassed-(swingPeriod/2f))/(swingPeriod/4f));
                }
                else//Swing backward
                {
                    currentXCoord = lastSavedXCooord+Mathf.Lerp(forwardSwingDistance,0,(swiningPassed-(swingPeriod*0.75f))/(swingPeriod/4f));
                }

                Vector3 pos = weaponLocalTransform.localPosition;
                pos.x = currentXCoord;
                weaponLocalTransform.localPosition = pos;
            }
            else
            {
                timePassedSinceSwing=0f;
                needToSwing=false;
            }
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f,0f,playerAnimation.getCurrentAngle());
        }
    }



    private void IdleAnimation()
    {
        switch(playerAnimation.getDirection())
        {
            case Directions.Up:
                weaponUsualPosition.z=0.1f;
                transform.localPosition = weaponUsualPosition;
                transform.rotation = Quaternion.Euler(0f,0f,90f);
                break;
            case Directions.Down:
                transform.localPosition = weaponMoveDownPosition;
                transform.rotation = Quaternion.Euler(0f,0f,90f);
                break;
            case Directions.Left:
                weaponUsualPosition.z=-0.1f;
                transform.localPosition = weaponUsualPosition;
                transform.rotation = Quaternion.Euler(0f,0f,135f);
                break;
            case Directions.Right:
                weaponUsualPosition.z=-0.1f;
                transform.localPosition = weaponUsualPosition;
                transform.rotation = Quaternion.Euler(0f,0f,45f);
                break;
        }
    }



    private void IdleLongAnimation()
    {

        switch(playerAnimation.getDirection())
        {
            case Directions.Up:
                transform.localPosition = weaponLongUpPosition;
                transform.rotation = Quaternion.Euler(0f,0f,-90f);
                break;
            case Directions.Down:
                transform.localPosition = weaponLongDownPosition;
                transform.rotation = Quaternion.Euler(0f,0f,-90f);
                break;
            case Directions.Left:
                transform.localPosition = weaponLongLeftPosition;
                transform.rotation = Quaternion.Euler(0f,0f,-90f);
                break;
            case Directions.Right:
                transform.localPosition = weaponLongRightPosition;
                transform.rotation = Quaternion.Euler(0f,0f,-90f);
                break;
        }
    }
}
