using UnityEngine;

public class FirearmWeaponAnimation : MonoBehaviour
{
    [SerializeField] private Vector3 weaponUpPosition;
    [SerializeField] private Vector3 weaponDownPosition;
    [SerializeField] private Vector3 weaponRightPosition;
    [SerializeField] private Vector3 weaponLeftPosition;
    [SerializeField] private Vector3 weaponLongUpPosition;
    [SerializeField] private Vector3 weaponLongDownPosition;
    [SerializeField] private Vector3 weaponLongRightPosition;
    [SerializeField] private Vector3 weaponLongLeftPosition;
    [SerializeField] private Sprite horizontalWithMagazine;
    [SerializeField] private Sprite verticalWithMagazine;
    [SerializeField] private Sprite horizontalWithoutMagazine;
    [SerializeField] private Sprite verticalWithoutMagazine;
    [SerializeField] private float timeToCancelFireSprites=1f;
    [SerializeField] private float firingSparkDuration=0.1f;
    [SerializeField] private PlayerAnimations playerAnimation;
    [SerializeField] private SpriteRenderer firingSpark;
    [SerializeField] private float scaleDecreaseToVertical=0.7f;


    private SpriteRenderer spriteRenderer;
    //private FirearmWeapon playerAnimation;
    private bool withoutMagazines=true;
    private float timePassedSinceFire=0f;
    private float initScale;



    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = horizontalWithoutMagazine;
        Color color = firingSpark.color;
        color.a=0f;
        firingSpark.color = color;
        initScale = transform.localScale.x;

        EventsManager.OnWeaponSwitched+=HideFireSpark;
    }



    public void StartedRecharge()
    {
        withoutMagazines=true;
    }



    public void FinishedRecharge()
    {
        withoutMagazines=false;
    }



    public void ShowFireSpark()
    {
        timePassedSinceFire=0f;
    }



    public void HideFireSpark()
    {
        timePassedSinceFire+=firingSparkDuration;
    }



    void Update()
    {
        timePassedSinceFire+=Time.deltaTime;
        if(timePassedSinceFire<firingSparkDuration)
        {
            Color color = firingSpark.color;
            color.a=Mathf.Lerp(1f,0f,timePassedSinceFire/firingSparkDuration);
            firingSpark.color = color;
        }

        Vector3 currentScale = transform.localScale;

        if(playerAnimation.isLongWaitAnimation())
        {
            //Debug.Log("Long animation");
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
        else
        {
            currentScale.x = initScale;
            transform.localScale = currentScale;

            if(withoutMagazines)
                spriteRenderer.sprite=horizontalWithoutMagazine;
            else
                spriteRenderer.sprite=horizontalWithMagazine;
            
            switch(playerAnimation.getDirection())
            {
                case Directions.Up:
                    spriteRenderer.flipY=true;
                    transform.rotation = Quaternion.Euler(0f,0f,180f);
                    transform.localPosition = weaponUpPosition;
                    break;
                case Directions.Down:
                    spriteRenderer.flipY=false;
                    transform.rotation = Quaternion.Euler(0f,0f,0f);
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
        }
    }
}
