using UnityEngine;

public partial class PlayerController: ITargetable
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;
    private void InitializeHealth() 
    {
        currentHealth = maxHealth;
    }

    public void Damage(Team attackerTeam, float damage)
    {
        if (attackerTeam == playerTeam)
            return;

        currentHealth -= damage;
        if (currentHealth <= 0f)
            Die();
    }

    void Die() => Debug.Log("Player Died");

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }
}
