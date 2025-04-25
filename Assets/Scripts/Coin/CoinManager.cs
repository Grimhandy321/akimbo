using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoinManager : MonoBehaviour, ITargetable
{
    public float healAmount = 10f;

    public void HitByHitScan(float damage, Team shooterTeam)
    {
        PlayerController target = FindClosestEnemy(shooterTeam);
        if (target != null)
        {
            target.Damage(shooterTeam, damage);
            HealShooter(shooterTeam, healAmount);
        }
    }

    public void Damage(Team attackerTeam, float damage)
    {
        HitByHitScan(damage, attackerTeam);
    }

    void Die()
    {
        Debug.Log("Player Died");
    }

    public void Heal(float amount)
    {

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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}