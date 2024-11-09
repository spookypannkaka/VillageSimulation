using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(Knockback))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float runSpeedMultiplier = 2f;
    [SerializeField] private float jumpHeight = 0.5f;
    [SerializeField] private float jumpCooldown = 0.5f;
    [SerializeField] private float jumpDuration = 0.9f;
    [SerializeField] private GameObject walkAnimPrefab;
    [SerializeField] private Transform walkAnimSpawnPoint;

    private Rigidbody2D rb;
    private Animator animator;
    private Knockback knockback;
    private bool isRunning;
    private bool isJumping;
    private bool facingLeft = false;
    private float jumpTimer;
    private float jumpYOffset;
    private GameObject walkAnim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        knockback = GetComponent<Knockback>();
    }

    public void Move(Vector2 movement, bool running)
    {
        if (knockback.GettingKnockedBack || PlayerHealth.Instance.IsDead) 
        {
            animator.SetFloat("moveX", 0);
            animator.SetFloat("moveY", 0);
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
            return;
        }

        float currentSpeed = running ? moveSpeed * runSpeedMultiplier : moveSpeed;
        Vector2 newPosition = rb.position + movement * (currentSpeed * Time.fixedDeltaTime);
        newPosition.y += jumpYOffset;

        rb.MovePosition(newPosition);

        isRunning = running;
        UpdateAnimation(movement);
        HandleWalkEffect(movement);
        AdjustFacingDirection(movement);
    }

    private void UpdateAnimation(Vector2 movement)
    {
        animator.SetFloat("moveX", movement.x);
        animator.SetFloat("moveY", movement.y);
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumping", isJumping);
    }

    private void HandleWalkEffect(Vector2 movement)
    {
        if (movement.x != 0)
        {
            if (walkAnim == null)
            {
                walkAnim = Instantiate(walkAnimPrefab, walkAnimSpawnPoint.position, Quaternion.identity);
                walkAnim.transform.parent = transform.parent;
            }

            Vector3 animScale = walkAnim.transform.localScale;
            animScale.x = Mathf.Sign(transform.localScale.x);
            walkAnim.transform.localScale = animScale;
        }
    }

    private void AdjustFacingDirection(Vector2 movement)
    {
        if (movement.x < 0 && !facingLeft)
        {
            Flip();
        }
        else if (movement.x > 0 && facingLeft)
        {
            Flip();
        }
    }

    private void Flip()
    {
        if (knockback.GettingKnockedBack || PlayerHealth.Instance.IsDead) return;

        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
        facingLeft = !facingLeft;
    }

    public void StartJump()
    {
        if (isJumping || knockback.GettingKnockedBack || Time.time < jumpTimer || PlayerHealth.Instance.IsDead) return;

        isJumping = true;
        jumpTimer = Time.time + jumpCooldown;
        animator.SetTrigger("isJumping");

        StartCoroutine(JumpRoutine());
    }

    private IEnumerator JumpRoutine()
    {
        float halfDuration = jumpDuration / 2f;
        for (float t = 0; t < halfDuration; t += Time.deltaTime)
        {
            jumpYOffset = Mathf.Lerp(0, jumpHeight, t / halfDuration);
            yield return null;
        }

        for (float t = 0; t < halfDuration; t += Time.deltaTime)
        {
            jumpYOffset = Mathf.Lerp(jumpHeight, 0, t / halfDuration) * -1;
            yield return null;
        }

        jumpYOffset = 0;
        isJumping = false;
    }

    public bool IsRunning => isRunning;
    public bool IsJumping => isJumping;
    public bool FacingLeft => facingLeft;
}
