using AlterunaFPS;
using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public partial class PlayerController : ITargetable
{
    [Header("Health")]
    public float maxHealth = 100f;
    [SynchronizableField]
    public float currentHealth;


    private int _lastSpawnIndex = 0;
    private ushort senderID = 0;

    private void InitializeHealth()
    {
        currentHealth = maxHealth;
        ShowHp.Instance.UpdateHealthUI(currentHealth);
    }

    public void Damage(Team attackerTeam, float damage, ushort senderId)
    {
        currentHealth -= damage;
        ShowHp.Instance.UpdateHealthUI(currentHealth);

        if (currentHealth <= 0f)
        {
            senderID = senderId;
            Die();
        }
    }

    void Die()
    {
        if (_possessed)
        {
            ScoreBoard.Instance.AddDeaths(multiplayer.Me.Index, 1);
            ScoreBoard.Instance.AddKills(senderID, 1);
        }

        currentHealth = maxHealth;
        ShowHp.Instance.UpdateHealthUI(currentHealth);

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

        Transform spawn = Multiplayer.AvatarSpawnLocations.Count > 0 ?Multiplayer.AvatarSpawnLocations[spawnIndex] :Multiplayer.AvatarSpawnLocation;

        gameObject.SetActive(false);
        transform.position = spawn.position;
        transform.rotation = spawn.rotation;
        gameObject.SetActive(false);

        RespawnController.Respawn(gameObject);
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        ShowHp.Instance.UpdateHealthUI(currentHealth);
    }

}
