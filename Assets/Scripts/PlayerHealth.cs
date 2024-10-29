using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : Singleton<PlayerHealth>
{
    public bool IsDead { get; private set; }

    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;
    private Slider healthSlider;
    private int currentHealth;
    private bool canTakeDamage = true;
    private Knockback knockback;
    private Animator myAnimator;

    protected override void Awake() {
        base.Awake();

        knockback = GetComponent<Knockback>();
    }

    private void Start() {
        IsDead = false;
        myAnimator = GetComponent<Animator>();
        currentHealth = maxHealth;
        UpdateHealthSlider();
    }

    private void OnCollisionStay2D(Collision2D other) {
        VillagerAI villager = other.gameObject.GetComponent<VillagerAI>();

        if (villager && canTakeDamage) {
            TakeDamage(1);
            knockback.GetKnockedBack(other.gameObject.transform, knockBackThrustAmount);
        }
    }

    public void HealPlayer() {
        if (currentHealth < maxHealth) {
            currentHealth += 1;
            UpdateHealthSlider();
        }
    }

    public void TakeDamage(int damageAmount) {
        if (!canTakeDamage) { return; }

        canTakeDamage = false;
        ScreenShakeManager.Instance.ShakeScreen();
        currentHealth -= damageAmount;
        StartCoroutine(DamageRecoveryRoutine());
        
        if (currentHealth <= 0) {
            CheckIfPlayerDeath();
        } else {
            myAnimator.SetTrigger("Hurt");  // Only play Hurt if player is not dead
            StartCoroutine(DamageRecoveryRoutine());
        }

        UpdateHealthSlider();
    }

    private void CheckIfPlayerDeath() {
        if (currentHealth <= 0 && !IsDead) {
            IsDead = true;
            currentHealth = 0;
            myAnimator.SetBool("isDead", true);
            myAnimator.SetTrigger("Death");
            StartCoroutine(DeathLoadSceneRoutine());
        }
    }

    private IEnumerator DeathLoadSceneRoutine() {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        SceneManager.LoadScene("Scene_1");
    }

    private IEnumerator DamageRecoveryRoutine() {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void UpdateHealthSlider() {
        if (healthSlider == null) {
            healthSlider = GameObject.Find("Health Bar").GetComponent<Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

}
