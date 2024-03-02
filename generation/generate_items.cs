using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generate_items : MonoBehaviour
{
    public GameObject medic, shield, sword1, sword2, weapon1, weapon2, weapon3, weapon4, magazine1, magazine2, magazine3, magazine4;
    public int amount_weapon=4, amount_sword=2, amount_shield=10, amount_medic=14, amount_magazine=20, min_x=-29, max_x=49, min_y=-21, max_y=21;
    private int num;

    void Start()
    {
        generateItems(amount_magazine,magazine1);
        generateItems(amount_magazine,magazine2);
        generateItems(amount_magazine,magazine3);
        generateItems(amount_magazine,magazine4);
        generateItems(amount_weapon,weapon1);
        generateItems(amount_weapon,weapon2);
        generateItems(amount_weapon,weapon3);
        generateItems(amount_weapon,weapon4);
        generateItems(amount_sword,sword1);
        generateItems(amount_sword,sword2);
        generateItems(amount_medic,medic);
        generateItems(amount_shield,shield);
    }

    private void generateItems(int amount, GameObject obj)
    {
        num = UnityEngine.Random.Range((int)(amount/2),amount);

        for(int i=0; i<num; i++)
        {
            Instantiate(obj, new Vector3(UnityEngine.Random.Range(max_x,min_x),UnityEngine.Random.Range(max_y,min_y),-0.3f), Quaternion.Euler(0f,0f,0f));
        }
    }
}
