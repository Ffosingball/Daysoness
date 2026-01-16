using UnityEngine;

public class FiringParticleAnimation : MonoBehaviour
{
    [SerializeField] private float firingSparkDuration=0.1f;
    [SerializeField] private Vector3 leftOffset;
    [SerializeField] private Vector3 rightOffset;
    [SerializeField] private Vector3 upOffset;
    [SerializeField] private Vector3 downOffset;
    [SerializeField] private bool rotateParticle;
    [SerializeField] private Sprite[] particlesVariations;
    [SerializeField] private PlayerAnimations playerAnimation;

    private SpriteRenderer firingSpark;
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
        sparkTime+=Time.deltaTime;

        Color color = firingSpark.color;
        if(sparkTime<firingSparkDuration)
            color.a=Mathf.Lerp(1f,0f,sparkTime/firingSparkDuration);
        else
            color.a=0f;
        firingSpark.color = color;

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



    void OnEnable()
    {
        sparkTime+=firingSparkDuration;
    }



    public void ShowFireSpark()
    {
        sparkTime=0f;
        
        if(rotateParticle)
            transform.rotation = Quaternion.Euler(0f,0f,UnityEngine.Random.Range(0,360));

        firingSpark.sprite = particlesVariations[UnityEngine.Random.Range(0,particlesVariations.Length)];
    }



    public void HideFireSpark()
    {
        sparkTime+=firingSparkDuration;
    }
}