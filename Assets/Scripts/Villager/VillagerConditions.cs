using UnityEngine;

public class CheckIfPlayerIsStealing : BTNode
{
    public override bool Execute(VillagerController villager)
    {
        return villager.IsPlayerStealing;
    }
}

public class CheckIfPlayerIsGivingGift : BTNode
{
    public override bool Execute(VillagerController villager)
    {
        return villager.IsPlayerGivingGift;
    }
}

public class CheckCombat : BTNode
{
    public override bool Execute(VillagerController villager)
    {
        return villager.IsNearCombatVillager;
    }
}

/*public class CheckNearbyAttack : BTNode
{
    public override bool Execute(VillagerController villager)
    {
        return villager.HasWitnessedAttack(); // Check for nearby attack
    }
}

public class CheckPlayerUnderAttack : BTNode
{
    public override bool Execute(VillagerController villager)
    {
        return villager.IsPlayerUnderAttack(); // Custom method to check if the player is under attack
    }
}*/

public class CheckIfUnderAttack : BTNode
{
    public override bool Execute(VillagerController villager)
    {
        if (villager.IsUnderAttack)
        {
            /*// Respond to attack, e.g., transition to FightingState or FleeingState
            if (villager.personality.Bravery > 0.5f)
            {
                villager.TransitionToState(villager.FightingState);
            }
            else
            {
                villager.TransitionToState(villager.FleeingState);
            }*/
            return true;
        }
        return false;
    }
}
