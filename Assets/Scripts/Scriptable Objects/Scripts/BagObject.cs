using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bag object", menuName = "Inventory System/Bag")]
public class BagObject : ItemObject
{
    public void Awake()
    {
        itemType = ItemType.VaccineBag;
    }
}