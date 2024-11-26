using System.Collections.Generic;
using UnityEngine;

public class MarketArea : MonoBehaviour
{
    // A static counter to keep track of how many markets the player is inside
    private static int playerInMarketCount = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInMarketCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInMarketCount--;
        }
    }

    // A static method to check if the player is in any market area
    public static bool IsPlayerInAnyMarket()
    {
        return playerInMarketCount > 0;
    }

public static void AddMarketActions(Dictionary<string, ActionManager.ActionData> actions)
    {
        string purchaseKey = PlayerController.Instance.GetKeyBinding("Interact");
        string stealKey = PlayerController.Instance.GetKeyBinding("Steal");

        if (!PlayerInventory.Instance.HasCake)
        {
            // Add purchase action
            actions[purchaseKey] = new ActionManager.ActionData
            {
                DisplayName = "Purchase Cake (3 coins)",
                KeyBinding = purchaseKey,
                Execute = () =>
                {
                    if (EconomyManager.Instance.CanAffordCake())
                    {
                        PlayerInventory.Instance.PurchaseCake();
                    }
                    else
                    {
                        Debug.Log("Not enough coins to purchase a cake.");
                    }
                }
            };

            // Add steal action
            actions[stealKey] = new ActionManager.ActionData
            {
                DisplayName = "Steal Cake",
                KeyBinding = stealKey,
                Execute = () =>
                {
                    if (!PlayerInventory.Instance.HasCake)
                    {
                        PlayerInventory.Instance.StealCake();
                    }
                }
            };
        }
    }
}
