using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private float attackCD = .5f;

    private PlayerControls playerControls;
    private Animator myAnimator;
    private bool attackButtonDown, isAttacking = false;

    
    private void Awake() {
        myAnimator = GetComponent<Animator>();
        playerControls = new PlayerControls();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    void Start()
    {
        playerControls.Actions.Attack.started += _ => StartAttacking();
        playerControls.Actions.Attack.canceled += _ => StopAttacking();

    }

    private void Update() {
        Attack();
    }

    private void StartAttacking() {
        attackButtonDown = true;
    }

    private void StopAttacking() {
        attackButtonDown = false;
    }

    private void Attack() {
        if (attackButtonDown && !isAttacking) {
            isAttacking = true;
            myAnimator.SetTrigger("Attack");
            weaponCollider.gameObject.SetActive(true);
            StartCoroutine(AttackCDRoutine());
        }

    }

    private IEnumerator AttackCDRoutine() {
        yield return new WaitForSeconds(attackCD);
        isAttacking = false;
    }

    public void DoneAttackingAnimEvent() {
        weaponCollider.gameObject.SetActive(false);
    }
    
}
