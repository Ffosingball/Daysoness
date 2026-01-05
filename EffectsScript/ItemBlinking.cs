using System;
using System.Collections;
using UnityEngine;

public class ItemBlinking : MonoBehaviour
{
    [SerializeField] private Color blinkColor;
    [SerializeField] private float blinkTime = 1f;

    private SpriteRenderer spriteRenderer;
    private Material material;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;

        StartCoroutine(BlinkLoop());
    }

    private IEnumerator BlinkLoop()
    {
        material.SetColor("_BlinkColor", blinkColor);

        float elapsedTime=0f;
        float currentBlinkAmount=0f;
        bool increase=true;

        while(true)
        {
            elapsedTime+=Time.deltaTime;

            if(elapsedTime>blinkTime)
            {
                elapsedTime-=blinkTime;
                increase=!increase;
            }

            if(increase)
                currentBlinkAmount = Mathf.Lerp(0f,1f,elapsedTime/blinkTime);
            else
                currentBlinkAmount = Mathf.Lerp(1f,0f,elapsedTime/blinkTime);

            material.SetFloat("_BlinkAmount", currentBlinkAmount);

            yield return null;
        }
    }
}
