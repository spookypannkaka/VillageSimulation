using System.Collections.Generic;
using UnityEngine;

public class VillagerArea : MonoBehaviour
{
    public static bool IsPlayerNearVillager()
    {
        // Logic to check if the player is near a villager
        return true; // Replace with actual condition
    }

    public static void AddVillagerActions(Dictionary<string, ActionManager.ActionData> actions)
    {
        string giftKey = PlayerController.Instance.GetKeyBinding("Interact");
        string attackKey = PlayerController.Instance.GetKeyBinding("Attack");

        actions[attackKey] = new ActionManager.ActionData
        {
            DisplayName = "Attack",
            KeyBinding = attackKey,
            Execute = () =>
            {
                /*if (PlayerInventory.Instance.HasCake)
                {
                    PlayerInventory.Instance.GiftCake();
                }
                else
                {
                    Debug.Log("You need a cake to gift it.");
                }*/
            }
        };

        if (PlayerInventory.Instance.HasCake) {
            actions[giftKey] = new ActionManager.ActionData
            {
                DisplayName = "Gift Cake",
                KeyBinding = giftKey,
                Execute = () => {}
            };
        }
    }
}
