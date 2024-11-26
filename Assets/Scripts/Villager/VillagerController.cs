using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VillagerController : MonoBehaviour
{
    // Managing states
    private IVillagerState currentState;
    public IVillagerState CurrentState { get => currentState; }
    public IVillagerState NeutralState { get; private set; }
    public IVillagerState AlertedState { get; private set; }
    public IVillagerState FriendState { get; private set; }
    public IVillagerState EnemyFightingState { get; private set; }
    public IVillagerState FriendFightingState { get; private set; }
    public IVillagerState FleeingState { get; private set; }
    public IVillagerState DeadState { get; private set; }

    // Villager status icon
    public SpriteRenderer statusIconRenderer;
    public Sprite neutralIcon, alertedIcon, friendIcon, fleeingIcon, fightingIcon, deadIcon;
    public GameObject highlightEffect;

    // Villager personality
    public Personality personality;

    // Health component
    private VillagerHealth health;

    // Villager vicinity detection
    public bool IsPlayerInFOV { get; private set; }
    public bool IsPlayerInRadius { get; private set; }
    public bool IsPlayerInVillagerArea { get; private set; }
    public bool IsNearCombatVillager { get; private set; }
    
    // Event that other villagers can listen to for nearby attacks
    public UnityEvent<Vector2> OnVillagerAttackedNearby = new UnityEvent<Vector2>();
    // Flags to communicate events to the behavior tree
    public bool IsPlayerStealing { get; private set; }
    public bool IsPlayerGivingGift { get; private set; }
    public bool IsUnderAttack { get; private set; }

    private void Start()
    {
        InitializeStates();
        AssignRandomPersonality();
        InitializeHealth();

        RegisterForNearbyAttackEvents();
        TransitionToState(NeutralState);
    }

    private void Update()
    {
        currentState.UpdateState(this); // Let the current state handle all state-specific logic
        //ResetFlags(); // Reset flags after each update cycle
    }

    private void OnEnable()
    {
        VillagerManager.Instance.RegisterVillager(this);
    }

    private void OnDisable()
    {
        VillagerManager.Instance.UnregisterVillager(this);
    }

    public void ConsumeActionFlags()
    {
        // Call this method explicitly after behavior tree processing
        ResetFlags();
    }

    private void InitializeStates()
    {
        // Each state gets its own behavior tree with its specific configuration
        NeutralState = new NeutralState(this);
        AlertedState = new AlertedState(this);
        FriendState = new FriendState(this);
        EnemyFightingState = new EnemyFightingState(this);
        FriendFightingState = new FriendFightingState(this);
        FleeingState = new FleeingState(this);
        DeadState = new DeadState(this);
    }

    private void AssignRandomPersonality()
    {
        personality = new Personality(
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f)
        );
    }

    private void InitializeHealth()
    {
        health = GetComponent<VillagerHealth>();
        health.OnDeath.AddListener(OnDeath); // Subscribe to death event
    }

    private void RegisterForNearbyAttackEvents()
    {
        foreach (var villager in FindObjectsOfType<VillagerController>())
        {
            if (villager != this)
            {
                villager.OnVillagerAttackedNearby.AddListener(OnNearbyVillagerAttacked);
            }
        }
    }

    // Event handlers for player actions
    public void OnPlayerStealsItem()
    {
        if (IsPlayerInFOV)
        {
            IsPlayerStealing = true;
        }
    }

    public void OnPlayerGiftsItem()
    {
        IsPlayerGivingGift = true;
    }

    public void OnPlayerAttacks()
    {
        IsUnderAttack = true;
    }

    private void ResetFlags()
    {
        IsPlayerStealing = false;
        IsPlayerGivingGift = false;
        IsUnderAttack = false;
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
            IsUnderAttack = true;
        }
    }

    private void OnDeath()
    {
        TransitionToState(DeadState);
    }

    public void TransitionToState(IVillagerState newState)
    {
        if (currentState != newState)
        {
            currentState?.ExitState(this);
            currentState = newState;
            newState.EnterState(this);
            UpdateStatusIconBasedOnState(newState);
        }
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

    public void SetPlayerInVillagerArea(bool inVillagerArea) {
        IsPlayerInVillagerArea = inVillagerArea;
    }

    public void SetIsPlayerStealing(bool isStealing) {
        IsPlayerStealing = isStealing;
    }

    public void SetIsNearCombatVillager(bool isNearCombatVillager) {
        IsNearCombatVillager = isNearCombatVillager;
    }

    public void UpdateStatusIcon(Sprite newIcon)
    {
        statusIconRenderer.sprite = newIcon;
    }

    private void UpdateStatusIconBasedOnState(IVillagerState newState)
    {
        Sprite iconToUse = newState switch
        {
            NeutralState _ => neutralIcon,
            AlertedState _ => alertedIcon,
            FriendState _ => friendIcon,
            EnemyFightingState _ => fightingIcon,
            FriendFightingState _ => friendIcon,
            FleeingState _ => fleeingIcon,
            DeadState _ => deadIcon,
            _ => null
        };

        UpdateStatusIcon(iconToUse);
    }

    public void SetHighlight(bool isActive)
    {
        if (highlightEffect != null)
        {
            highlightEffect.SetActive(isActive);
        }
    }

    public bool IsInCombatOrDead()
    {
        return CurrentState is DeadState || CurrentState is FleeingState ||
               CurrentState is EnemyFightingState || CurrentState is FriendFightingState;
    }
}