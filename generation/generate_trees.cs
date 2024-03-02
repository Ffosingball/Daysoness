using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generate_trees : MonoBehaviour
{

    public GameObject tree_1, tree_2, tree_3;
    public int amount_tree=10, x_min=-11, x_max=10, y_min=-9, y_max=8;

    void Start()
    {
        for(int i=0; i<amount_tree; i++)
        {
            GenerateTree();
        }
    }

    void GenerateTree()
    {
        int type = UnityEngine.Random.Range(1,4);

        switch(type)
        {
            case 1:
                Instantiate(tree_1, new Vector3(UnityEngine.Random.Range(x_max,x_min),UnityEngine.Random.Range(y_max,y_min),-1), Quaternion.Euler(0f,0f,0f));
                break;
            case 2:
                Instantiate(tree_2, new Vector3(UnityEngine.Random.Range(x_max,x_min),UnityEngine.Random.Range(y_max,y_min),-1), Quaternion.Euler(0f,0f,0f));
                break;
            case 3:
                Instantiate(tree_3, new Vector3(UnityEngine.Random.Range(x_max,x_min),UnityEngine.Random.Range(y_max,y_min),-1), Quaternion.Euler(0f,0f,0f));
                break;
        }
    }
}
