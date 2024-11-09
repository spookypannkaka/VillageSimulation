using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeingState : IVillagerState
{
    public void EnterState(VillagerController villager)
    {
        Debug.Log("I am fleeing");
        villager.GetComponent<VillagerFleeing>().enabled = true;
    }

    public void UpdateState(VillagerController villager)
    {
        // 
    }

    public void ExitState(VillagerController villager)
    {
        villager.GetComponent<VillagerFleeing>().enabled = false;
    }

    public void HandleSteal(VillagerController villager) { /* Fleeing villagers ignore theft */ }
    public void HandleGift(VillagerController villager) { /* Fleeing villagers ignore gifts */ }
    public void HandleAttack(VillagerController villager) { /* Fleeing villagers continue fleeing if attacked */ }
}
