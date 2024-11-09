using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendState : IVillagerState
{
    public void EnterState(VillagerController villager)
    {
        Debug.Log("I am friendly");
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
        // Friendly villagers do not care if you steal
    }

    public void HandleGift(VillagerController villager)
    {
        // Friendly villagers cannot become more loyal
    }

    public void HandleAttack(VillagerController villager)
    {
        // We need to handle a situation where loyalty can change if you attack them
    }
}
