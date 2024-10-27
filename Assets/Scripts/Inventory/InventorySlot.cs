using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private ItemInfo itemInfo;

    public ItemInfo GetItemInfo() {
        return itemInfo;
    }
}
