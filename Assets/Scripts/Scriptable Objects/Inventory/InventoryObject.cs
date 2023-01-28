using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> Container = new List<InventorySlot>();

    public void AddItem(ItemObject item, int amount)
    {
        switch (item.itemType)
        {
            case ItemType.VaccineBag:
                Debug.Log("AddVaccineBag");
                AddVaccineBag(item, amount);
                break;
            case ItemType.MainVaccineBag:
                Debug.Log("AddMainBag");
                AddMainBag(item);
                break;
        }
    }

    private void AddMainBag(ItemObject item)
    {
        Container.Add(new InventorySlot(item, 1));
    }

    private void AddVaccineBag(ItemObject item, int amount)
    {
        if (Container.Count > 0)
        {
            Container.First().AddAmount(amount);
        }
        else
        {
            Container.Add(new InventorySlot(item, amount));
        }

    }

    public void AddVaccineBagsAmmo(int amount)
    {
        var bag = ScriptableObject.CreateInstance<BagObject>();
        Container.Add(new InventorySlot(bag, amount));
    }
    public void RemoveVaccineBagAmmo()
    {
        if (Container.Count > 0)
        {
            Container.First().RemoveOne();
        }
    }
}

[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int amount;

    public InventorySlot(ItemObject item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }

    public void RemoveOne()
    {
        if (amount > 0)
        {
            amount -= 1;
        }
    }
}