using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class statistica : MonoBehaviour
{
    public GameObject StatScreen;
    public Text spiderT, beatleT, RREWT, zenSmallT, zenMediumT, zenBigT, itemT, bulletT, swordT, timeT;
    public static float playedTime=0f;
    public static int spidersKilled=0, beatleKilled=0, zenSmallKilled=0, zenMediumKilled=0, zenBigKilled=0, RREWKilled=0, itemPicked=0, firedBullets=0, swungSword=0;

    void Start()
    {
        StartCoroutine(counter());
    }

    private IEnumerator counter()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);
            playedTime+=0.1f;
        }
    }

    public void showStatistic()
    {
        spiderT.text=""+spidersKilled;
        beatleT.text=""+beatleKilled;
        RREWT.text=""+RREWKilled;
        zenSmallT.text=""+zenSmallKilled;
        zenMediumT.text=""+zenMediumKilled;
        zenBigT.text=""+zenBigKilled;
        itemT.text=""+itemPicked;
        bulletT.text=""+firedBullets;
        swordT.text=""+swungSword;
        timeT.text=""+Math.Round(playedTime,1);
        StatScreen.SetActive(true);
    }

    public void hideStatistic()
    {
        StatScreen.SetActive(false);
    }
}
