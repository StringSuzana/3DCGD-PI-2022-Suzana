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
    MainVaccineBag = 1,
    VaccineBag = 2,
    FoodBag = 3,
    HealthBag = 4
}