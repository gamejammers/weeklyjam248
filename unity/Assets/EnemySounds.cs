using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySounds : MonoBehaviour
{
    public AudioClip death;

    private AudioSource Source;

    void Awake()
    {
        Source = GetComponent<AudioSource>();
    }

    public void Play_death()
    {
        Source.clip = death;
        Source.Play();
    }
}
