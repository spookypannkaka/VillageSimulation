using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFightingState : IVillagerState
{
    private Selector rootNode;
    private VillagerController villager;

    public EnemyFightingState(VillagerController villager)
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

            }),
            new WanderAction() // Default action to wander
        });
    }

    public void EnterState(VillagerController villager)
    {
        Debug.Log("Entering Fighting State");
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
        //
    }
}
