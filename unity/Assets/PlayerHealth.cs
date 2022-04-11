using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int MaxHealth;
    public int CurrentHealth;
    public GameObject DeadUI;

    void Awake() { CurrentHealth = MaxHealth; }

    public void TakeDamage(int dam, Vector3 KnockBack)
    {
        CurrentHealth -= dam;
        if (CurrentHealth <= 0)
        {
            DeadUI.SetActive(true);
            GetComponent<Movement>().Speed = 0;
            GetComponent<Movement>().sensitivity = 0;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
            GetComponent<Rigidbody>().AddForce(KnockBack * 50);
    }
}
