using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : IVillagerState
{
    private Selector rootNode;
    private VillagerController villager;

    public DeadState(VillagerController villager)
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
        });
    }

    public void EnterState(VillagerController villager)
    {
        Debug.Log("Entering Dead State");
    }

    public void UpdateState(VillagerController villager)
    {
        rootNode.Execute(villager);
    }

    public void ExitState(VillagerController villager)
    {
        //
    }
}
