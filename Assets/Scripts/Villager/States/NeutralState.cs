using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralState : IVillagerState
{
    private Selector rootNode;
    private VillagerController villager;

    public NeutralState(VillagerController villager)
    {
        this.villager = villager;
        InitializeBehaviorTree();
    }

    private void InitializeBehaviorTree()
    {
        rootNode = new Selector(new List<BTNode>
        {
            // Villager under attack
            new Sequence(new List<BTNode>
            {
                new CheckIfUnderAttack(),
                new FightOrFleeAction()
            }),

            // Player steals an item
            new Sequence(new List<BTNode>
            {
                new CheckIfPlayerIsStealing(),
                new TransitionToAlertedAction() // Transition to AlertedState if player is stealing
            }),

            // Villager receives a gift
            new Sequence(new List<BTNode>
            {
                new CheckIfPlayerIsGivingGift(),
                new ReceiveGiftAction(),
                new TransitionToFriendAction() // Transition to FriendState after receiving a gift
            }),

            // Check for dead villagers
            new Sequence(new List<BTNode>
            {
                new CheckCombat(),
                new FightOrFleeAction(),
            }),

            new WanderAction() // Default action to wander
        });
    }

    public void EnterState(VillagerController villager)
    {
        Debug.Log("Entering Neutral State");
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