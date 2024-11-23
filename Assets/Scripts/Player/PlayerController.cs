using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerActions))]
public class PlayerController : Singleton<PlayerController>
{
    private PlayerControls playerControls;
    private PlayerMovement playerMovement;
    private PlayerActions playerActions;
    private Vector2 movement;
    private bool isRunning;

    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
        playerMovement = GetComponent<PlayerMovement>();
        playerActions = GetComponent<PlayerActions>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        playerMovement.Move(movement, isRunning);
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
        isRunning = playerControls.Movement.Run.ReadValue<float>() > 0;

        if (playerControls.Movement.Jump.triggered)
        {
            playerMovement.StartJump();
        }

        /*if (playerControls.Actions.StealItem.triggered)
        {
            if (actionManager.TryExecuteAction("Steal"))  // Check context before stealing
            {
                playerActions.StealItem(transform.position);
            }
        }*/

        if (playerControls.Actions.Attack.triggered)
        {
            playerActions.Attack(transform.position);
        }

        if (playerControls.Actions.UseItem.triggered)
        {
            playerActions.UseItem(new InputAction.CallbackContext());  // Passing a default context
        }

        if (playerControls.Actions.Steal.triggered)
        {
            playerActions.Steal(transform.position);
        }

        if (playerControls.Actions.Interact.triggered)
        {
            HandleInteract();
        }

    }

    private void HandleInteract()
    {
        // Check if there is an active villager and the player has a cake
        VillagerController activeVillager = VillagerManager.Instance.GetActiveVillager();
        if (activeVillager != null && PlayerInventory.Instance.HasCake)
        {
            playerActions.AttemptToGiveGift();
        }
        else
        {
            // Handle other interact actions or give feedback for why interaction isn't possible
            if (activeVillager == null) {
                Debug.Log("No active villager nearby to interact with.");
            } else if (!PlayerInventory.Instance.HasCake) {
                Debug.Log("You need a cake to gift it.");
            }
        }
    }

    public string GetStealKeyBinding()
    {
        return playerControls.Actions.Steal.GetBindingDisplayString();
    }

    // Function to fetch the key binding for a specific action
    public string GetKeyBinding(string actionName)
    {
        InputAction action = null;

        // Match action name with the InputAction in PlayerControls
        switch (actionName)
        {
            case "UseItem":
                action = playerControls.Actions.UseItem;
                break;
            case "Interact":
                action = playerControls.Actions.Interact;
                break;
            case "Steal":
                action = playerControls.Actions.Steal;
                break;
            case "Attack":
                action = playerControls.Actions.Attack;
                break;
            case "Jump":
                action = playerControls.Movement.Jump;
                break;
            // Add more cases as needed
        }

        if (action != null)
        {
            return action.GetBindingDisplayString(); // Get the mapped key
        }

        Debug.LogWarning($"No binding found for action: {actionName}");
        return "Unknown"; // Fallback if no matching action is found
    }

}