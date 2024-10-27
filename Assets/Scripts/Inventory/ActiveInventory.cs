using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveInventory : MonoBehaviour
{
    private int activeSlotIndexNum = 0;

    private PlayerControls playerControls;

    private void Awake() {
        playerControls = new PlayerControls();
    }

    private void Start() {
        playerControls.Actions.Inventory.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());
        ToggleActiveHighlight(0);
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void ToggleActiveSlot(int numValue) {
        ToggleActiveHighlight(numValue - 1);
    }

    private void ToggleActiveHighlight(int indexNum) {
        activeSlotIndexNum = indexNum;

        foreach (Transform inventorySlot in this.transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }

        this.transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);

        ChangeActiveItem();
    }

    private void ChangeActiveItem() {
        if (ActiveInventoryItem.Instance.CurrentActiveItem != null) {
            Destroy(ActiveInventoryItem.Instance.CurrentActiveItem.gameObject);
        }

        if (transform.GetChild(activeSlotIndexNum).GetComponentInChildren<InventorySlot>().GetItemInfo() == null) {
            ActiveInventoryItem.Instance.ItemNull();
            return;
        }

        GameObject itemToSpawn = transform.GetChild(activeSlotIndexNum).
        GetComponentInChildren<InventorySlot>().GetItemInfo().itemPrefab;

        GameObject newitem = Instantiate(itemToSpawn, ActiveInventoryItem.Instance.transform.position, Quaternion.identity);

        newitem.transform.parent = ActiveInventoryItem.Instance.transform;

        ActiveInventoryItem.Instance.NewItem(newitem.GetComponent<MonoBehaviour>());

    }
}
