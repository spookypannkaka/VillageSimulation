using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public TMP_Text controlsText;
    private Dictionary<string, ActionData> availableActions = new Dictionary<string, ActionData>();

    // Struct to represent an action
    public struct ActionData
    {
        public string DisplayName; // e.g., "Purchase Cake"
        public System.Action Execute; // The action to execute when triggered
        public string KeyBinding;
    }

    private void Update()
    {
        availableActions.Clear();
        
        // Gather actions from different contexts
        if (MarketArea.IsPlayerInAnyMarket())
        {
            MarketArea.AddMarketActions(availableActions);
        }
        
        VillagerController activeVillager = VillagerManager.Instance.GetActiveVillager();
        if (activeVillager != null && activeVillager.IsPlayerInVillagerArea)
        {
            VillagerArea.AddVillagerActions(availableActions);
        }

        UpdateControlsText();
    }

    private void UpdateControlsText()
    {
        if (availableActions.Count > 0)
        {
            controlsText.gameObject.SetActive(true);

            var displayText = new List<string>();
            foreach (var kvp in availableActions)
            {
                displayText.Add($"{kvp.Value.KeyBinding}: {kvp.Value.DisplayName}");
            }

            controlsText.text = string.Join("\n", displayText);
        }
        else
        {
            controlsText.gameObject.SetActive(false);
        }
    }

    public void TryExecuteAction(string actionKey)
    {
        if (availableActions.TryGetValue(actionKey, out var actionData))
        {
            actionData.Execute?.Invoke(); // Execute the action
        }
        else
        {
            Debug.Log($"No valid action mapped to key: {actionKey}");
        }
    }
}
