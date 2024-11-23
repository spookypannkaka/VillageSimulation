using TMPro;
using UnityEngine;

public class PlayerInventory : Singleton<PlayerInventory>
{
    public bool HasCake { get; private set; }
    public KeyCode pickUpKey = KeyCode.E;
    public GameObject equippedItem;

    protected override void Awake()
    {
        base.Awake();
    }

    public void PurchaseCake()
    {
        if (!HasCake)
        {
            HasCake = true;
            EconomyManager.Instance.PurchaseCake();
            UpdateInventoryUI();
            Debug.Log("Cake added to inventory!");
        }
        else
        {
            Debug.Log("You already have a cake.");
        }
    }

    public void StealCake()
    {
        if (!HasCake)
        {
            HasCake = true;
            UpdateInventoryUI();
            Debug.Log("Cake added to inventory!");
            // broadcast stealing in fov area
        }
        else
        {
            Debug.Log("You already have a cake.");
        }
    }

    public void GiftCake()
    {
        if (HasCake)
        {
            HasCake = false;
            UpdateInventoryUI();
            Debug.Log("You gifted the cake to a villager!");
        }
    }

    public void EatCake()
    {
        if (HasCake)
        {
            HasCake = false;
            UpdateInventoryUI();
            Debug.Log("You ate the cake!");
        }
    }

    private void UpdateInventoryUI()
    {
        if (equippedItem != null)
        {
            equippedItem.SetActive(HasCake);
        }
    }
}
