using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;

    private void OnTriggerEnter2D(Collider2D other) {
        // Check if the object has VillagerHealth to apply damage
        VillagerHealth villagerHealth = other.GetComponent<VillagerHealth>();
        if (villagerHealth != null)
        {
            villagerHealth.TakeDamage(damageAmount);

            // Check if the object also has a VillagerController for state management
            VillagerController villagerController = other.GetComponent<VillagerController>();
            if (villagerController != null)
            {
                villagerController.NotifyOfAttack();
                //villagerController.OnPlayerAttacks();

                if (other.tag == "Player") {
                    // The player attacked this villager
                    villagerController.OnPlayerAttacks();
                } else if (other.tag == "Villager") {
                    // Another villager attacked this villager, decide if to defend or not
                    villagerController.OnNearbyVillagerAttacked(transform.position);
                }
            }
        }
    }
}
