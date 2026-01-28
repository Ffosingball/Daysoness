using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public struct ItemGenerationConfig
{
    public GameObject item;
    public bool appearInBulk;
    public Vector2Int amount;
    public float chance;
    public LevelsOfRareness rareness;
}


[Serializable]
public struct ItemGenerationSpot
{
    public Vector2 position;
    public float radius;
    public LevelsOfRareness rarenessOfTheItems;
}


public class ItemsGeneration : MonoBehaviour
{
    [SerializeField] private ItemGenerationConfig[] itemsForGeneration;
    [SerializeField] private ItemGenerationSpot[] allPlacesForItemsToAppear;



    private void Start()
    {
        GenerateItems();
    }



    private void GenerateItems()
    {
        foreach(ItemGenerationSpot spot in allPlacesForItemsToAppear)
        {
            foreach(ItemGenerationConfig item in itemsForGeneration)
            {
                if(spot.rarenessOfTheItems==item.rareness)
                {
                    if(item.appearInBulk)
                    {
                        int numOfItemsToCreate = UnityEngine.Random.Range(item.amount.x,item.amount.y);

                        for(int i=0; i<numOfItemsToCreate; i++)
                        {
                            CreateItemInSpot(spot,item);
                        }
                    }
                    else
                    {
                        if(UnityEngine.Random.Range(0f,1f)<item.chance)
                            CreateItemInSpot(spot,item);
                    }
                }
            }
        }
    }



    public void CreateItemInSpot(ItemGenerationSpot spot, ItemGenerationConfig item)
    {
        Vector2 position = GenerateItemPosition(spot.position,spot.radius);

        Instantiate(item.item,position,Quaternion.identity);
    }



    private Vector2 GenerateItemPosition(Vector2 centerPosition, float maxDistance)
    {
        float angle = UnityEngine.Random.Range(-180f,180f);
        float distance = UnityEngine.Random.Range(0f,maxDistance);
        int xSign = 1;
        int ySign = 1;

        if(angle<0f)
        {
            ySign=-1;
            angle*=-1;
        }

        if(angle>90f)
        {
            xSign=-1;
            angle-=90;
        }

        float b = Mathf.Sqrt(Mathf.Pow(distance,2)/((1/Mathf.Pow(Mathf.Tan(angle * Mathf.Deg2Rad),2))+1));
        float a = b / Mathf.Tan(angle*Mathf.Deg2Rad);

        return new Vector2(centerPosition.x+(a*xSign),centerPosition.y+(b*ySign));
    }
}
