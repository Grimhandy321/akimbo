using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public float healAmount = 10f;

    public void HitByHitScan(float damage, Team shooterTeam,ushort senderId)
    {

        PlayerController target = FindClosestEnemy(shooterTeam);
        Target testTarget = FindClosestTarget();
        if (testTarget != null) 
        {
            transform.position = testTarget.transform.position;
            testTarget.Damage(shooterTeam, damage, senderId);
            HealShooter(shooterTeam, healAmount);
            Debug.Log("Target Hit");
        }
        if (target != null)
        {
            transform.position = target.transform.position;
            target.Damage(shooterTeam, damage, senderId);
            HealShooter(shooterTeam, healAmount);
            Destroy(gameObject);
        }
    }
    public void HitByProjectile(Team shooterTeam, GameObject projectile)
    {
        PlayerController target = FindClosestEnemy(shooterTeam);
        if (target != null)
        {
            projectile.transform.position = target.transform.position;

            ProjectileBase pb = projectile.GetComponent<ProjectileBase>();
            if (pb != null)
            {
                pb.Detonate();
            }

            Destroy(gameObject);
        }
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

    private Target FindClosestTarget() 
    {
        Target[] allPlayers = FindObjectsOfType<Target>();
        Target closest = null;
        float minDist = float.MaxValue;

        foreach (var player in allPlayers)
        {

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