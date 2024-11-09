using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : IVillagerState
{
    public void EnterState(VillagerController villager)
    {
        Debug.Log("I am dead");
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
        // Dead people do not react to stealing
    }

    public void HandleGift(VillagerController villager)
    {
        // Dead people do not receive gifts
    }

    public void HandleAttack(VillagerController villager)
    {
        // Dead people can't be attacked
    }
}
