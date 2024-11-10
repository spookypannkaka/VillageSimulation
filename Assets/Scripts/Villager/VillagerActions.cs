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
        villager.TransitionToState(villager.FightingState);
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

// Node for transitioning to the Friend state (after receiving a gift)
public class TransitionToFriendAction : BTNode
{
    public override bool Execute(VillagerController villager)
    {
        villager.TransitionToState(villager.FriendState);
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
        villager.ReceiveGift();
        return true;
    }
}

public class FightOrFleeAction : BTNode
{
    public override bool Execute(VillagerController villager)
    {
        if (villager.personality.Bravery > 0.5f)
        {
            villager.TransitionToState(villager.FightingState);
        }
        else
        {
            villager.TransitionToState(villager.FleeingState);
        }
        return true;
    }
}