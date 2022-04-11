using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int MaxHealth;
    public int CurrentHealth;
    public EnemyVisualController enemyVisualController;
    public EnemyAI enemyAI;

    void Awake()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(int dam)
    {
        CurrentHealth -= dam;
        enemyAI.AttackCool = 1;
        if (CurrentHealth >= 0)
            enemyVisualController.TakeDamage();
        else
        {
            enemyAI.AttackCool = 3;
            enemyVisualController.SetDead();
            enemyAI.Die();
        }
    }
}
