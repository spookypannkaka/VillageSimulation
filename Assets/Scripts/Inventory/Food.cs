using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour, IInventoryItem
{
    [SerializeField] private ItemInfo itemInfo;

    public void Use() {
        Debug.Log("I ate food");
    }

    public ItemInfo GetItemInfo()
    {
        return itemInfo;
    }

}
