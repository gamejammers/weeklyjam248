using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int MaxHealth;
    public int CurrentHealth;
    public GameObject DeadUI;

    private PlayerSounds Sounds;

    void Awake()
    {
        CurrentHealth = MaxHealth;
        Sounds = GetComponent<PlayerSounds>();
    }

    public void TakeDamage(int dam, Vector3 KnockBack)
    {
        CurrentHealth -= dam;
        if (CurrentHealth <= 0)
        {
            GetComponent<Movement>().Speed = 0;
            GetComponent<Movement>().sensitivity = 0;
            Sounds.Play_death();
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
            GetComponent<Rigidbody>().AddForce(KnockBack * 50);
    }
}
