using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public partial class PlayerController : ITargetable
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthSlider;

    private int _lastSpawnIndex = 0;
    private ushort senderID = 0;

    private void InitializeHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void Damage(Team attackerTeam, float damage, ushort senderId)
    {
        if (attackerTeam == playerTeam)
            return;

        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 0f)
        {
            senderID = senderId;
            Die();
        }
    }

    void Die()
    {
        if (_possesed)
        {
            ScoreBoard.Instance.AddDeaths(Avatar.Possessor, 1);
            ScoreBoard.Instance.AddKills(senderID, 1);
        }

        currentHealth = maxHealth;
        UpdateHealthUI();

        int spawnIndex = 0;
        int spawnLocationsCount = Multiplayer.AvatarSpawnLocations.Count;

        if (spawnLocationsCount > 1)
        {
            do
            {
                spawnIndex = Random.Range(0, spawnLocationsCount);
            }
            while (_lastSpawnIndex == spawnIndex);

            _lastSpawnIndex = spawnIndex;
        }
        else if (spawnLocationsCount <= 0)
        {
            throw new IndexOutOfRangeException("No spawns");
        }

        transform.position = Multiplayer.AvatarSpawnLocations[spawnIndex].position;
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }
}
