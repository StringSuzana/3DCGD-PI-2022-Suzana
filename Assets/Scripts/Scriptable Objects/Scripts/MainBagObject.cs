using UnityEngine;

[CreateAssetMenu(fileName = "New Bag object", menuName = "Inventory System/MainBag")]
public class MainBagObject : ItemObject
{
    public void Awake()
    {
        itemType = ItemType.MainVaccineBag;
    }
}