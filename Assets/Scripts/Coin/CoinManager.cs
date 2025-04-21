using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public float healAmount = 10f;

    public void HitByHitScan(float damage, Team shooterTeam)
    {
        PlayerController target = FindClosestEnemy(shooterTeam);
        if (target != null)
        {
            target.TakeDamage(damage, shooterTeam, true);
            HealShooter(shooterTeam, healAmount);
        }
    }

    public void HitByProjectile(Projectile projectile, Team shooterTeam)
    {
        PlayerController target = FindClosestEnemy(shooterTeam);
        if (target != null)
        {
            // Teleport projectile to target and simulate a hit
            projectile.transform.position = target.transform.position + Vector3.up; // optional offset
            target.TakeDamage(projectile.Damage, shooterTeam, true);
            HealShooter(shooterTeam, healAmount);
        }

        Destroy(projectile.gameObject); // simulate instant hit
    }

    private PlayerController FindClosestEnemy(Team team)
    {
        PlayerController[] allPlayers = FindObjectsOfType<PlayerController>();
        PlayerController closest = null;
        float minDist = float.MaxValue;

        foreach (var player in allPlayers)
        {
            if (player.playerTeam == team) continue;

            float dist = Vector3.Distance(transform.position, player.transform.position);
            if (dist < minDist)
            {
                closest = player;
                minDist = dist;
            }
        }

        return closest;
    }

    private void HealShooter(Team shooterTeam, float amount)
    {
        PlayerController[] allPlayers = FindObjectsOfType<PlayerController>();
        foreach (var player in allPlayers)
        {
            if (player.playerTeam == shooterTeam)
            {
                player.Heal(amount);
                Debug.Log($"Healed player on {shooterTeam} for {amount} HP");
                break;
            }
        }
    }
}