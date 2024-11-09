using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingState : IVillagerState
{
    public void EnterState(VillagerController villager)
    {
       Debug.Log("I am fighting");
    }

    public void UpdateState(VillagerController villager)
    {
        // 
    }

    public void ExitState(VillagerController villager)
    {
        // 
    }

    public void HandleSteal(VillagerController villager)
    {
        // Villagers that fight don't care if you steal
    }

    public void HandleGift(VillagerController villager)
    {
        // Fighting villagers do not accept gifts
    }

    public void HandleAttack(VillagerController villager)
    {
        // Maybe we should change loyalty if we attack a loyal villager
        // (But they don't care if you attack other villagers)
    }
}
