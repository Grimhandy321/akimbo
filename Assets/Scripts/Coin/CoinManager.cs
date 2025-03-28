using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    private float storedDamage;
    private Team storedTeam;

    void OnTriggerEnter(Collider other)
    {
        // Handle Projectile
        if (other.TryGetComponent<Projectile>(out var projectile))
        {
            storedTeam = projectile.GetTeam();
            Debug.Log($"Hit by projectile! Teleporting. Team: {storedTeam}");

            // Teleport to the closest enemy player
            TeleportToClosestEnemy();
            Destroy(other.gameObject); // Destroy projectile
        }
    }

    // Called by HitScan weapons via Raycast
    public void HitByHitScan(float damage, Team team)
    {
        storedDamage = damage;
        storedTeam = team;
        Debug.Log($"Hit by HitScan! Damage stored: {storedDamage}, Team: {storedTeam}");
    }

    private void TeleportToClosestEnemy()
    {
        // Find all players
        PlayerController[] players = FindObjectsOfType<PlayerController>();

        // Get enemies only
        var enemyPlayers = players
            .Where(player => player.PlayerTeam != storedTeam) // Opposite team
            .OrderBy(player => Vector3.Distance(transform.position, player.transform.position))
            .ToList();

        // If an enemy exists, teleport to the closest
        if (enemyPlayers.Count > 0)
        {
            Transform closestEnemy = enemyPlayers[0].transform;
            transform.position = closestEnemy.position + Vector3.up * 2f; // Slightly above
            Debug.Log($"Teleported to {closestEnemy.name}");
        }
        else
        {
            Debug.Log("No enemy players found!");
        }
    }
}
