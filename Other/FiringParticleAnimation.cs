using UnityEngine;

public class FiringParticleAnimation : MonoBehaviour
{
    //How long spark should exist
    [SerializeField] private float firingSparkDuration=0.1f;
    //Spark offsets
    [SerializeField] private Vector3 leftOffset;
    [SerializeField] private Vector3 rightOffset;
    [SerializeField] private Vector3 upOffset;
    [SerializeField] private Vector3 downOffset;
    //Should spark be rotated when it will be shown again
    [SerializeField] private bool rotateParticle;
    //Variations of the spark
    [SerializeField] private Sprite[] particlesVariations;
    [SerializeField] private PlayerAnimations playerAnimation;

    private SpriteRenderer firingSpark;
    //Time counter
    private float sparkTime=0f;



    private void Start()
    {
        EventsManager.OnWeaponSwitched+=HideFireSpark;
        EventsManager.OnFireBullet+=ShowFireSpark;

        firingSpark = GetComponent<SpriteRenderer>();
        Color color = firingSpark.color;
        color.a=0f;
        firingSpark.color = color;
    }



    private void Update()
    {
        //Increase counter
        sparkTime+=Time.deltaTime;

        //Fade away spark, if sprark is still visible
        Color color = firingSpark.color;
        if(sparkTime<firingSparkDuration)
            color.a=Mathf.Lerp(1f,0f,sparkTime/firingSparkDuration);
        else
            color.a=0f;
        firingSpark.color = color;

        //Set correct position depending on the direction
        switch(playerAnimation.getDirection())
        {
            case Directions.Up:
            case Directions.BackwardsDown:
                transform.localPosition = upOffset;
                break;
            case Directions.Down:
            case Directions.BackwardsUp:
                transform.localPosition = downOffset;
                break;
            case Directions.Left:
            case Directions.BackwardsRight:
                transform.localPosition = leftOffset;
                break;
            case Directions.Right:
            case Directions.BackwardsLeft:
                transform.localPosition = rightOffset;
                break;
        }
    }



    //When it is active make spark invisible
    void OnEnable()
    {
        sparkTime+=firingSparkDuration;
    }



    //When this meythod is called it will show spark, set new sprite variation
    //to it and rotate if needed
    public void ShowFireSpark()
    {
        sparkTime=0f;
        
        if(rotateParticle)
            transform.rotation = Quaternion.Euler(0f,0f,UnityEngine.Random.Range(0,360));

        firingSpark.sprite = particlesVariations[UnityEngine.Random.Range(0,particlesVariations.Length)];
    }



    //Hides spark
    public void HideFireSpark()
    {
        sparkTime+=firingSparkDuration;
    }
}