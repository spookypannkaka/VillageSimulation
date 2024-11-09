using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralState : IVillagerState
{
    public void EnterState(VillagerController villager)
    {
        Debug.Log("I am neutral");
        //villager.GetComponent<RandomWalker>().enabled = true;
        villager.GetComponent<VillagerWander>().enabled = true;
    }

    public void UpdateState(VillagerController villager)
    {
        // Code to handle neutral state behavior, e.g., wandering around
    }

    public void ExitState(VillagerController villager)
    {
        villager.GetComponent<VillagerWander>().enabled = false;
    }

    public void HandleSteal(VillagerController villager)
    {
        if (villager.IsPlayerInFOV)
        {
            villager.TransitionToState(villager.AlertedState);
        }
    }

    public void HandleGift(VillagerController villager)
    {
        villager.TransitionToState(villager.FriendState);
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
