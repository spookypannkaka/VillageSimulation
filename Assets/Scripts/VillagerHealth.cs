using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private float knockBackThrust = 15f;
    [SerializeField] private float deathCd = 0.1f;
    private Knockback knockback;
    private Animator myAnimator;

    private int currentHealth;

    private void Awake() {
        knockback = GetComponent<Knockback>();
    }

    private void Start() {
        myAnimator = GetComponent<Animator>();
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        myAnimator.SetTrigger("Hurt");
        knockback.GetKnockedBack(PlayerController.Instance.transform, knockBackThrust);
        StartCoroutine(CheckDetectDeathRoutine());
    }

    private IEnumerator CheckDetectDeathRoutine() {
        yield return new WaitForSeconds(deathCd);
        DetectDeath();
    }


    public void DetectDeath() {
        if (currentHealth <= 0) {
            //Destroy(gameObject);
            myAnimator.SetTrigger("Death");
        }
    }
}
