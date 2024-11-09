using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        /*if (playerControls.Actions.Steal.triggered)
        {
            playerActions.StealItem(transform.position);
        }*/

        if (playerControls.Actions.Attack.triggered)
        {
            playerActions.Attack(transform.position);
        }
    }
}