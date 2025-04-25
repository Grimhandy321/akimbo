using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour , ITargetable
{
    float currentHealth = 100;
    public void Damage(Team attackerTeam, float damage)
    {
        Debug.Log("team:" + attackerTeam + "dmg: " + damage);
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Died");
    }

    public void Heal(float amount)
    {

    }
}
