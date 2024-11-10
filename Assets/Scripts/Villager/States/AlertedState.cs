using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertedState : IVillagerState
{
    private Selector rootNode;
    private VillagerController villager;

    public AlertedState(VillagerController villager)
    {
        this.villager = villager;
        InitializeBehaviorTree();
    }

    private void InitializeBehaviorTree()
    {
        rootNode = new Selector(new List<BTNode>
        {
            new Sequence(new List<BTNode>
            {
                new CheckIfUnderAttack(),
                new FightOrFleeAction() // Either fight or flee based on bravery
            }),
            new WanderAction() // Fallback to wandering if no threat detected
        });
    }

    public void EnterState(VillagerController villager)
    {
        Debug.Log("Entering Alerted State");
    }

    public void UpdateState(VillagerController villager)
    {
        rootNode.Execute(villager);
    }

    public void ExitState(VillagerController villager)
    {
        //villager.GetComponent<VillagerWander>().enabled = false;
    }
}