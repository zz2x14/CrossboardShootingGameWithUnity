using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    [SerializeField] private int deathEnergyBonus;
    protected override void Die()
    {
        PlayerEnergy.Instance.ObtainEnergy(deathEnergyBonus);
        EnemyManager.Instance.RemoveFromEnemyList(gameObject);
        base.Die();
    }

}
