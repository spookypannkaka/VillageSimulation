using UnityEngine;

public class FleeAction : BTNode
{
    public override bool Execute(VillagerController villager)
    {
        var fleeingComponent = villager.GetComponent<VillagerFleeing>();
        
        if (fleeingComponent == null)
        {
            Debug.LogError("VillagerFleeing component not found on the same GameObject as VillagerController.");
            return false;
        }

        // Enable VillagerFleeing and confirm with a debug statement
        fleeingComponent.enabled = true;
        
        return true;
    }
}

public class FightBackAction : BTNode
{
    public override bool Execute(VillagerController villager)
    {
        villager.TransitionToState(villager.EnemyFightingState);
        return true;
    }
}

public class TransitionToAlertedAction : BTNode
{
    public override bool Execute(VillagerController villager)
    {
        villager.TransitionToState(villager.AlertedState);
        return true;
    }
}

public class SecondStealTransition : BTNode
{
    public override bool Execute(VillagerController villager)
    {
        if (villager.personality.Bravery >= 0.75f) {
            villager.TransitionToState(villager.EnemyFightingState);
        } else {
            // Notify other villagers of a thief?
        }
        
        return true;
    }
}

// Node for transitioning to the Friend state (after receiving a gift)
public class TransitionToFriendAction : BTNode
{
    public override bool Execute(VillagerController villager)
    {
        villager.TransitionToState(villager.FriendState);
        return true;
    }
}

public class TransitionToNeutralAction : BTNode
{
    public override bool Execute(VillagerController villager)
    {
        villager.TransitionToState(villager.NeutralState);
        return true;
    }
}

/*public class FollowPlayerAction : BTNode
{
    public override bool Execute(VillagerController villager)
    {
        villager.FollowPlayer();
        return true;
    }
}*/

public class WanderAction : BTNode
{
    public override bool Execute(VillagerController villager)
    {
        villager.GetComponent<VillagerWander>().enabled = true;
        return true;
    }
}

public class ReceiveGiftAction : BTNode
{
    public override bool Execute(VillagerController villager)
    {
        if (!PlayerInventory.Instance.HasCake) return false;
        
        if (villager.CurrentState is NeutralState) {
            PlayerInventory.Instance.GiftCake();
            villager.TransitionToState(villager.FriendState);
        } else if (villager.CurrentState is AlertedState) {
            PlayerInventory.Instance.GiftCake();
            villager.TransitionToState(villager.NeutralState);
        } else {
            return false;
        }

        return true;
    }
}

public class FightOrFleeAction : BTNode
{
    public override bool Execute(VillagerController villager)
    {
        if (villager.CurrentState is FriendState) {
                villager.TransitionToState(villager.FriendFightingState);
        } else {
            if (villager.personality.Bravery <= 0.75f) {
                villager.TransitionToState(villager.FleeingState);
            } else {
                villager.TransitionToState(villager.EnemyFightingState);
            }
        }
        return true;
    }
}