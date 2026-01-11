using System;
using System.Collections;
using UnityEngine;

public class ItemBlinking : MonoBehaviour
{
    //Color with which item should blink
    [SerializeField] private Color blinkColor;
    //How long blink happens
    [SerializeField] private float blinkTime = 1f;

    private SpriteRenderer spriteRenderer;
    //Material which has a shader which can blink
    private Material material;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;

        StartCoroutine(BlinkLoop());
    }


    //Coroutine which indefinitely changes color of the item
    private IEnumerator BlinkLoop()
    {
        //Set color by its name in the shader
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

            //Set percentage of color in shader by its name
            material.SetFloat("_BlinkAmount", currentBlinkAmount);

            yield return null;
        }
    }
}
