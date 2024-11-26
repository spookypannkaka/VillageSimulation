using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class FleeingState : IVillagerState
{
    private Selector rootNode;
    private VillagerController villager;

    public FleeingState(VillagerController villager)
    {
        this.villager = villager;
        InitializeBehaviorTree();
    }

    private void InitializeBehaviorTree()
    {
        rootNode = new Selector(new List<BTNode>
        {
            new FleeAction()
        });
    }

    public void EnterState(VillagerController villager)
    {
        Debug.Log("Entering Fleeing State");

        villager.GetComponent<AIPath>().enabled = true;

        if (villager.GetComponent<VillagerWander>().enabled) {
            villager.GetComponent<VillagerWander>().enabled = false;
        }
    }

    public void UpdateState(VillagerController villager)
    {
        rootNode.Execute(villager);
        villager.ConsumeActionFlags();
    }

    public void ExitState(VillagerController villager)
    {
        villager.GetComponent<VillagerFleeing>().enabled = false;
    }
}
