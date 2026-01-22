using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZenzeleyskBehaviour : MonoBehaviour
{
    [SerializeField] private Sprite[] zenzeleyskAppearenceBig;
    [SerializeField] private Sprite[] zenzeleyskAppearenceMedium;
    [SerializeField] private Sprite[] zenzeleyskAppearenceSmall;
    [SerializeField] private Sprite[] zenzeleyskBiteBig;
    [SerializeField] private Sprite[] zenzeleyskBiteMedium;
    [SerializeField] private Sprite[] zenzeleyskBiteSmall;
    [SerializeField] private Sprite zenzeleyskDead;
    [SerializeField] private GameObject zenzeleyskBody;
    [SerializeField] private float appearanceTime;
    [SerializeField] private float biteTime;
    [SerializeField] private float nextAttackWaitTime;
    [SerializeField] private float biteDamage;
    [SerializeField] private Vector2 timeToWaitToAttackPlayerRange;
    [SerializeField] private CommonEnemyBehaviour commonEnemyBehaviour;
    [SerializeField] private float appearenceYOffset;
    [SerializeField] private float stunPlayerForSeconds = 5f;
    [SerializeField] private Vector3 bigBodyPosition;
    [SerializeField] private Vector3 mediumBodyPosition;
    [SerializeField] private Vector3 smallBodyPosition;
    [SerializeField] private Vector2 bigBodySize;
    [SerializeField] private Vector2 bigBodyOffset;
    [SerializeField] private Vector2 mediumBodySize;
    [SerializeField] private Vector2 mediumBodyOffset;
    [SerializeField] private Vector2 smallBodySize;
    [SerializeField] private Vector2 smallBodyOffset;
    [SerializeField] private GameObject shadow;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] biteClips;
    [SerializeField] private AudioClip[] appearClips;
    [SerializeField] private AudioClip[] idleClips;
    [SerializeField] private Vector2 timeBetweenIdleClips;

    private SpriteRenderer spriteRenderer;
    private Transform bodyTransform;
    private BoxCollider2D bodyBoxCollider;
    private BoxCollider2D mainBoxCollider;
    private PlayerComponent playerComponent;
    private Movement movement;
    private Transform playerTransform;
    private Vector2 centralPosition;
    private float timeToWaitToAttackPlayer;
    private float timeUntilAttackCounter=0f;
    private Coroutine attackLoop;
    private float idleSoundCounter=0f;
    private float waitTimeForNextIdleSound;


    private void Start() 
    {
        playerTransform = GameObject.FindGameObjectWithTag("player").transform;
        playerComponent = GameObject.FindGameObjectWithTag("player").GetComponent<PlayerComponent>();
        movement = GameObject.FindGameObjectWithTag("player").GetComponent<Movement>();

        spriteRenderer = zenzeleyskBody.GetComponent<SpriteRenderer>();
        bodyTransform = zenzeleyskBody.transform;
        bodyBoxCollider = zenzeleyskBody.GetComponent<BoxCollider2D>();
        mainBoxCollider = GetComponent<BoxCollider2D>();

        spriteRenderer.enabled=false;
        bodyBoxCollider.enabled=false;
        mainBoxCollider.enabled=false;
        shadow.SetActive(false);
        
        waitTimeForNextIdleSound = Random.Range(timeBetweenIdleClips.x,timeBetweenIdleClips.y);
        centralPosition = transform.position;
        timeToWaitToAttackPlayer = Random.Range(timeToWaitToAttackPlayerRange.x,timeToWaitToAttackPlayerRange.y);
    }



    void Update()
    {
        if(!commonEnemyBehaviour.IsDead())
        {
            timeUntilAttackCounter+=Time.deltaTime;
            idleSoundCounter+=Time.deltaTime;

            float distanceToPlayer = Vector2.Distance(centralPosition, playerTransform.position);
            if(distanceToPlayer<=commonEnemyBehaviour.getDetectionRange())
            {
                if(movement.getIfCharacterMoves())
                    timeUntilAttackCounter=0f;
                else
                {
                    if(timeUntilAttackCounter>timeToWaitToAttackPlayer)
                    {
                        StartAttack();
                    }
                }
            }
            else
                StopAttack();

            if(idleSoundCounter>waitTimeForNextIdleSound)
            {
                waitTimeForNextIdleSound = Random.Range(timeBetweenIdleClips.x,timeBetweenIdleClips.y);
                idleSoundCounter=0f;
                audioSource.PlayOneShot(idleClips[Random.Range(0,idleClips.Length)]);
            }
        }
        else
        {
            if(attackLoop!=null)
            {
                StopCoroutine(attackLoop);
                attackLoop=null;
            }

            bodyTransform.localPosition = smallBodyPosition;
            spriteRenderer.enabled=true;
            mainBoxCollider.enabled=true;
            spriteRenderer.sprite = zenzeleyskDead;
            shadow.SetActive(false);
        }
    }



    private void StartAttack()
    {
        if(attackLoop==null)
            attackLoop = StartCoroutine(IndefiniteAttackLoop());
    }



    private void StopAttack()
    {
        if(attackLoop!=null)
        {
            StopCoroutine(attackLoop);
            attackLoop=null;

            spriteRenderer.enabled=false;
            bodyBoxCollider.enabled=false;
            mainBoxCollider.enabled=false;
            shadow.SetActive(false);

            idleSoundCounter=0f;
        }
    }



    private IEnumerator IndefiniteAttackLoop()
    {
        while(true)
        {
            audioSource.PlayOneShot(appearClips[Random.Range(0,appearClips.Length)]);
            movement.setCannotMoveFor(stunPlayerForSeconds);

            spriteRenderer.enabled=true;
            bodyBoxCollider.enabled=true;
            mainBoxCollider.enabled=true;
            shadow.SetActive(true);

            Vector3 pos = playerTransform.position;
            pos.y+=appearenceYOffset;
            transform.position = pos;

            if(commonEnemyBehaviour.getCurrentHP()>=commonEnemyBehaviour.getMaxHP()*0.66f)
            {
                bodyTransform.localPosition = bigBodyPosition;
                bodyBoxCollider.size = bigBodySize;
                bodyBoxCollider.offset = bigBodyOffset;

                StartCoroutine(appearanceAnimation(zenzeleyskAppearenceBig));
                yield return new WaitForSeconds(appearanceTime);

                StartCoroutine(biteAnimation(zenzeleyskBiteBig));
                yield return new WaitForSeconds(biteTime);

                StartCoroutine(disappearAnimation(zenzeleyskAppearenceBig));
                yield return new WaitForSeconds(appearanceTime);
            }
            else if(commonEnemyBehaviour.getCurrentHP()>=commonEnemyBehaviour.getMaxHP()*0.33f)
            {
                bodyTransform.localPosition = mediumBodyPosition;
                bodyBoxCollider.size = mediumBodySize;
                bodyBoxCollider.offset = mediumBodyOffset;

                StartCoroutine(appearanceAnimation(zenzeleyskAppearenceMedium));
                yield return new WaitForSeconds(appearanceTime);

                StartCoroutine(biteAnimation(zenzeleyskBiteMedium));
                yield return new WaitForSeconds(biteTime);

                StartCoroutine(disappearAnimation(zenzeleyskAppearenceMedium));
                yield return new WaitForSeconds(appearanceTime);
            }
            else
            {
                bodyTransform.localPosition = smallBodyPosition;
                bodyBoxCollider.size = smallBodySize;
                bodyBoxCollider.offset = smallBodyOffset;

                StartCoroutine(appearanceAnimation(zenzeleyskAppearenceSmall));
                yield return new WaitForSeconds(appearanceTime);

                StartCoroutine(biteAnimation(zenzeleyskBiteSmall));
                yield return new WaitForSeconds(biteTime);

                StartCoroutine(disappearAnimation(zenzeleyskAppearenceSmall));
                yield return new WaitForSeconds(appearanceTime);
            }

            spriteRenderer.enabled=false;
            bodyBoxCollider.enabled=false;
            mainBoxCollider.enabled=false;
            shadow.SetActive(false);

            yield return new WaitForSeconds(nextAttackWaitTime);
        }
    }



    private IEnumerator appearanceAnimation(Sprite[] sprites)
    {
        for(int i=0; i<sprites.Length; i++)
        {
            spriteRenderer.sprite = sprites[i];
            yield return new WaitForSeconds(appearanceTime/sprites.Length);
        }
    }



    private IEnumerator biteAnimation(Sprite[] sprites)
    {
        playerComponent.TakeDamage(biteDamage);
        audioSource.PlayOneShot(biteClips[Random.Range(0,biteClips.Length)]);

        for(int i=0; i<sprites.Length; i++)
        {
            spriteRenderer.sprite = sprites[i];
            yield return new WaitForSeconds(biteTime/sprites.Length);
        }
    }



    private IEnumerator disappearAnimation(Sprite[] sprites)
    {
        for(int i=0; i<sprites.Length; i++)
        {
            spriteRenderer.sprite = sprites[sprites.Length-i-1];
            yield return new WaitForSeconds(appearanceTime/sprites.Length);
        }
    }
}
