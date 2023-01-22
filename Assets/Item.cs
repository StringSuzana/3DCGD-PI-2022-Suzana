using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
     public ItemObject itemObject;
    void Start()
    {
        Debug.Log(itemObject.description);
    }

}
