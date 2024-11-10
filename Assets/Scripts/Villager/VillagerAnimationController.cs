using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.U2D.Animation;

public class VillagerAnimationController : MonoBehaviour
{
    private AIPath aiPath;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private SpriteResolver spriteResolver;
    private VillagerAppearanceController appearanceController;

    void Awake()
    {
        aiPath = GetComponent<AIPath>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteResolver = GetComponent<SpriteResolver>();
        appearanceController = GetComponent<VillagerAppearanceController>();
    }

    void Update()
    {
        // Calculate the villager's current speed
        float speed = aiPath.velocity.magnitude;

        // Update the animator's Speed parameter to control transitions
        animator.SetFloat("Speed", speed);

        // Flip the sprite based on the movement direction
        if (aiPath.velocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (aiPath.velocity.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }
}
