using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int MaxHealth;
    public int CurrentHealth;

    void Awake()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(int dam)
    {
        CurrentHealth -= dam;
    }
}
