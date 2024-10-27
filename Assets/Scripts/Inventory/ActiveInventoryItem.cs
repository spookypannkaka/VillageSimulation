using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveInventoryItem : Singleton<ActiveInventoryItem>
{
    public MonoBehaviour CurrentActiveItem { get; private set; }
    private PlayerControls playerControls;
    private bool isUsing = false;

    protected override void Awake() {
        base.Awake();

        playerControls = new PlayerControls();
    }

    private void OnEnable() {
        playerControls.Enable();
        playerControls.Actions.UseItem.performed += _ => OnUseItem();
    }

    private void OnDisable() {
        playerControls.Disable();
        playerControls.Actions.UseItem.performed -= _ => OnUseItem();
    }

    private void OnUseItem() {
        Use();
    }

    public void NewItem(MonoBehaviour newItem) {
        CurrentActiveItem = newItem;
    }

    public void ItemNull() {
        CurrentActiveItem = null;
    }

    public void ToggleIsUsing(bool value) {
        isUsing = value;
    }

    public void Use() {
        if (!isUsing && CurrentActiveItem) {
            isUsing = true;
            (CurrentActiveItem as IInventoryItem).Use();
            isUsing = false;
        }

    }
}
