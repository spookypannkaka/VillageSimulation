using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertedState : IVillagerState
{
    public void EnterState(VillagerController villager)
    {
        Debug.Log("I am alerted");
        villager.GetComponent<VillagerWander>().enabled = true;
    }

    public void UpdateState(VillagerController villager)
    {
        // 
    }

    public void ExitState(VillagerController villager)
    {
        villager.GetComponent<VillagerWander>().enabled = false;
    }

    public void HandleSteal(VillagerController villager)
    {
        if (villager.personality.Bravery > 0.5f)
        {
            villager.TransitionToState(villager.FightingState);
        }
    }

    public void HandleGift(VillagerController villager)
    {
        villager.TransitionToState(villager.NeutralState);
    }

    public void HandleAttack(VillagerController villager)
    {
        if (villager.IsPlayerInFOV || villager.IsPlayerInRadius)
        {
            if (villager.personality.Bravery > 0.5f)
            {
                villager.TransitionToState(villager.FightingState);
            }
            else
            {
                villager.TransitionToState(villager.FleeingState);
            }
        }
    }
}
