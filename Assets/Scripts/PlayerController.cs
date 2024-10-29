using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public bool FacingLeft { get { return facingLeft; } }

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float runSpeedMultiplier = 2f;
    [SerializeField] private float jumpHeight = 0.5f;
    [SerializeField] private float jumpCooldown = 0.5f;
    [SerializeField] private float jumpDuration = 0.9f;
    [SerializeField] private GameObject walkAnimPrefab;
    [SerializeField] private Transform walkAnimSpawnPoint;
    private bool facingLeft = false;


    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;
    private Knockback knockback;
    private bool isRunning;
    private bool isJumping;
    private float jumpTimer;
    private float jumpYOffset;
    private GameObject walkAnim;

    protected override void Awake() {
        base.Awake();
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    private void Update() {
        PlayerInput();
    }
    private void FixedUpdate() {
        if (!isJumping) {
            AdjustPlayerFacingDirection();
        }
        Move();
    }

    private void PlayerInput() {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        isRunning = playerControls.Movement.Run.ReadValue<float>() > 0;

        if (playerControls.Movement.Jump.triggered && !isJumping && Time.time > jumpTimer) {
            StartJump();
        }
    }

    private void Move() {
        if (knockback.GettingKnockedBack || PlayerHealth.Instance.IsDead) {
            myAnimator.SetFloat("moveX", 0);
            myAnimator.SetFloat("moveY", 0);
            myAnimator.SetBool("isRunning", false);
            myAnimator.SetBool("isJumping", false);
            return;
        }

        float currentSpeed = isRunning ? moveSpeed * runSpeedMultiplier : moveSpeed;

        Vector2 newPosition = rb.position + movement * (currentSpeed * Time.fixedDeltaTime);
        newPosition.y += jumpYOffset;

        rb.MovePosition(newPosition);

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
        myAnimator.SetBool("isRunning", isRunning);
        myAnimator.SetBool("isJumping", isJumping);

    if (movement.x != 0) {
        if (walkAnim == null) {
            walkAnim = Instantiate(walkAnimPrefab, walkAnimSpawnPoint.position, Quaternion.identity);
            walkAnim.transform.parent = this.transform.parent;
        }

        // Flip the walk animation based on the player's facing direction
        Vector3 animScale = walkAnim.transform.localScale;
        animScale.x = Mathf.Sign(transform.localScale.x);  // Match the player's direction
        walkAnim.transform.localScale = animScale;
    }

    }

    private void StartJump() {
        if (knockback.GettingKnockedBack || PlayerHealth.Instance.IsDead) { return; }

        myAnimator.SetTrigger("isJumping");
        isJumping = true;

        // Set the timer to prevent jumping too frequently
        jumpTimer = Time.time + jumpCooldown;

        StartCoroutine(JumpRoutine());
    }

    private IEnumerator JumpRoutine() {
        float elapsedTime = 0f;
        float halfDuration = jumpDuration / 2f;

        while (elapsedTime < halfDuration) {
            jumpYOffset = Mathf.Lerp(0, jumpHeight, elapsedTime / halfDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < halfDuration) {
            jumpYOffset = Mathf.Lerp(jumpHeight, 0, elapsedTime / halfDuration) * -1;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        jumpYOffset = 0f;
        isJumping = false;
    }

    private void AdjustPlayerFacingDirection() {
        if (movement.x < 0 && transform.localScale.x > 0) {
            Flip();
        } else if (movement.x > 0 && transform.localScale.x < 0) {
            Flip();
        }
    }

    private void Flip() {
        if (knockback.GettingKnockedBack || PlayerHealth.Instance.IsDead) { return; }

        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }

}
