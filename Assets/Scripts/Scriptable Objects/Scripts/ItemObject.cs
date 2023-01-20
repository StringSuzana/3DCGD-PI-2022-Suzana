using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ItemObject : ScriptableObject
{
    public GameObject itemPrefab;
    public ItemType itemType;
    public string description;

}
public enum ItemType
{
    VaccineBag = 1,
    FoodBag = 2,
    HealthBag = 3
}