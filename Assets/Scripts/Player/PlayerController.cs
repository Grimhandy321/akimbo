using System;
using UnityEngine;

public class PlayerController :  MonoBehaviour , ITargetable
{

    public float maxHealth = 100f;
    public Team playerTeam;
    public Transform weaponHolder;
    private float currentHealth;
    public static Action shootInput;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        HandleShooting();
    }

    void HandleShooting()
    {
        if (Input.GetMouseButton(0))
        {
            shootInput?.Invoke();
        }
    }
    
    public void Damage(Team attackerTeam, float damage)
    {
        if (attackerTeam == playerTeam)
        {
            return;
        }

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
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }
}
