using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public List<AudioClip> AxeSwing;
    public List<AudioClip> AxeChains;
    public List<AudioClip> Jump;
    public AudioClip ShotGunShot;
    public AudioClip death;

    private AudioSource Source;

    void Awake()
    {
        Source = GetComponent<AudioSource>();
    }

    public void Play_AxeSwing()
    {
        Source.clip = AxeSwing[Random.Range(0, AxeSwing.Count)];
        Source.Play();
    }

    public void Play_AxeChains()
    {
        Source.clip = AxeChains[Random.Range(0, AxeChains.Count)];
        Source.Play();
    }

    public void Play_Jump()
    {
        Source.clip = Jump[Random.Range(0, Jump.Count)];
        Source.Play();
    }

    public void Play_ShotGunShot()
    {
        Source.clip = ShotGunShot;
        Source.Play();
    }

    public void Play_death()
    {
        Source.clip = death;
        Source.Play();
    }
}
