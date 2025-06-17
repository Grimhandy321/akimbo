using Alteruna;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour , ITargetable
{
    public void Heal(float amount)
    {

    }

    public void Damage(Team team, float dmg, ushort senderId)
    {
        Debug.Log("team:" + team + "dmg: " + dmg + "sender:" + senderId);
        ScoreBoard.Instance.AddDeaths(senderId, 1);
        ScoreBoard.Instance.AddKills(senderId, 1);
    }
}
