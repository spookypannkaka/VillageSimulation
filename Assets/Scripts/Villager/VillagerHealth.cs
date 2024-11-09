using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VillagerHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private float knockBackThrust = 15f;
    [SerializeField] private float deathCd = 0.1f;
    private Knockback knockback;
    private Animator myAnimator;

    private int currentHealth;

    public UnityEvent OnDeath;

    private void Awake() {
        knockback = GetComponent<Knockback>();
        currentHealth = startingHealth;
    }

    private void Start() {
        myAnimator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage) {
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        myAnimator.SetTrigger("Hurt");
        knockback.GetKnockedBack(PlayerController.Instance.transform, knockBackThrust);

        if (currentHealth <= 0)
        {
            StartCoroutine(CheckDetectDeathRoutine());
        }
    }

    private IEnumerator CheckDetectDeathRoutine() {
        yield return new WaitForSeconds(deathCd);
        DetectDeath();
    }


    public void DetectDeath() {
        if (currentHealth <= 0) {
            myAnimator.SetTrigger("Death");
            OnDeath.Invoke();
        }
    }

    public bool IsDead => currentHealth <= 0;
    public int CurrentHealth => currentHealth;
    public int MaxHealth => startingHealth;
}
