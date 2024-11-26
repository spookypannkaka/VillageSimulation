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
                new FightOrFleeAction()
            }),

            // Player steals an item
            new Sequence(new List<BTNode>
            {
                new CheckIfPlayerIsStealing(),
                new SecondStealTransition()
            }),

            new Sequence(new List<BTNode>
            {
                new CheckIfPlayerIsGivingGift(),
                new ReceiveGiftAction(),
                new TransitionToNeutralAction()
            }),

            new Sequence(new List<BTNode>
            {
                new CheckCombat(),
                new FightOrFleeAction(),
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
        villager.ConsumeActionFlags();
    }

    public void ExitState(VillagerController villager)
    {
        //villager.GetComponent<VillagerWander>().enabled = false;
    }
}