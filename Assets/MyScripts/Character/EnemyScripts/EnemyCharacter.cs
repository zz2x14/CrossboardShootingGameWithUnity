using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    [SerializeField] private int deathEnergyBonus;
    [SerializeField] private int deathScoreBonus;
    protected override void Die()
    {
        ScoreManager.Instance.AddScore(deathScoreBonus);
        PlayerEnergy.Instance.ObtainEnergy(deathEnergyBonus);
        EnemyManager.Instance.RemoveFromEnemyList(gameObject);

        base.Die();
    }

}
