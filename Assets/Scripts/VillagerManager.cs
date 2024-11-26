using System.Collections.Generic;
using UnityEngine;

public class VillagerManager : Singleton<VillagerManager>
{
    private List<VillagerController> villagers = new List<VillagerController>();
    private List<VillagerController> activeVillagers = new List<VillagerController>();
    private VillagerController currentActiveVillager;

    protected override void Awake()
    {
        base.Awake();
    }

    public void RegisterVillager(VillagerController villager)
    {
        if (!villagers.Contains(villager))
            villagers.Add(villager);
    }

    public void UnregisterVillager(VillagerController villager)
    {
        if (villagers.Contains(villager))
            villagers.Remove(villager);
    }

    public void EnterVillagerArea(VillagerController controller)
    {
        if (!activeVillagers.Contains(controller))
        {
            activeVillagers.Add(controller);
            UpdateActiveVillager();
        }
    }

    public void ExitVillagerArea(VillagerController controller)
    {
        activeVillagers.Remove(controller);
        UpdateActiveVillager();
    }

    public VillagerController GetActiveVillager()
    {
        return currentActiveVillager;
    }

    private void UpdateActiveVillager()
    {
        VillagerController newActive = null;

        if (activeVillagers.Count > 0)
        {
            float minDistance = Mathf.Infinity;
            Vector3 playerPosition = PlayerController.Instance.transform.position;

            foreach (var villager in activeVillagers)
            {
                float distance = Vector3.Distance(playerPosition, villager.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    newActive = villager;
                }
            }
        }

        if (newActive != currentActiveVillager)
        {
            if (currentActiveVillager != null)
            {
                currentActiveVillager.SetHighlight(false);
            }

            currentActiveVillager = newActive;

            if (currentActiveVillager != null)
            {
                currentActiveVillager.SetHighlight(true);
            }
        }
    }

    public void NotifyVillagersPlayerIsStealing()
    {
        foreach (var villager in villagers)
        {
            if (villager.IsPlayerInFOV)
                villager.SetIsPlayerStealing(true);
        }
    }

}
