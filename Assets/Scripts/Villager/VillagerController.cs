using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VillagerController : MonoBehaviour
{
    // Managing states
    private IVillagerState currentState;
    public IVillagerState CurrentState { get; private set; }
    public IVillagerState NeutralState { get; private set; }
    public IVillagerState AlertedState { get; private set; }
    public IVillagerState FriendState { get; private set; }
    public IVillagerState FightingState { get; private set; }
    public IVillagerState FleeingState { get; private set; }
    public IVillagerState DeadState { get; private set; }

    // Villager personality
    public Personality personality;

    // Health component
    private VillagerHealth health;

    // Villager vicinity detection
    public bool IsPlayerInFOV { get; private set; }
    public bool IsPlayerInRadius { get; private set; }
    // Event that other villagers can listen to for nearby attacks
    public UnityEvent<Vector2> OnVillagerAttackedNearby = new UnityEvent<Vector2>();

    private void Start()
    {
        NeutralState = new NeutralState();
        AlertedState = new AlertedState();
        FriendState = new FriendState();
        FightingState = new FightingState();
        FleeingState = new FleeingState();
        DeadState = new DeadState();
        TransitionToState(NeutralState);

        // Assign a random personality to the villager
        personality = new Personality(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1));

        // Initialize health and subscribe to death event
        health = GetComponent<VillagerHealth>();
        health.OnDeath.AddListener(OnDeath); // Subscribe to death event

        RegisterForNearbyAttackEvents();
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    // Method to register for other villagers' attack events
    private void RegisterForNearbyAttackEvents()
    {
        VillagerController[] allVillagers = FindObjectsOfType<VillagerController>();

        foreach (VillagerController villager in allVillagers)
        {
            if (villager != this) // Skip self to avoid double-listening
            {
                villager.OnVillagerAttackedNearby.AddListener(OnNearbyVillagerAttacked);
            }
        }
    }

    // Event handling methods
    public void OnPlayerStealsItem()
    {
        currentState.HandleSteal(this);
    }

    public void OnPlayerGiftsItem()
    {
        currentState.HandleGift(this);
    }

    public void OnPlayerAttacks()
    {
        currentState.HandleAttack(this);
    }

    // Call this method when this villager is attacked
    public void NotifyOfAttack()
    {
        // Broadcast to all listeners (other villagers) that an attack happened here
        OnVillagerAttackedNearby.Invoke(transform.position);
    }

    // Handle reaction to a nearby attack
    public void OnNearbyVillagerAttacked(Vector2 attackPosition)
    {
        // React if the player is in FOV or detection radius
        if (IsPlayerInFOV || IsPlayerInRadius)
        {
            currentState.HandleAttack(this);
        }
    }

    private void OnDeath()
    {
        TransitionToState(DeadState);
    }

    public void TransitionToState(IVillagerState newState)
    {
        if (currentState != null) currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    // Method to update FOV status
    public void SetPlayerInFOV(bool inFOV)
    {
        IsPlayerInFOV = inFOV;
    }

    // Method to update Detection Radius status
    public void SetPlayerInRadius(bool inRadius)
    {
        IsPlayerInRadius = inRadius;
    }
}
