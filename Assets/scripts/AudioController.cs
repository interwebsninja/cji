﻿using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour
{
    public AudioSource musicBus;
    public AudioSource[] effectBus;
    public AudioSource dynamicBus;

    public static AudioController Instance;

    private SoundBank bank;

    void Awake()
    {
        Instance = this;
        bank = GetComponent<SoundBank>();
    }

    void Start()
    {
        dynamicBus.clip = bank.Request(SoundBank.DynamicSounds.Comet);
        dynamicBus.loop = true;
        dynamicBus.Play();
    }
    public void StartMusic()
    {
        musicBus.clip = bank.Request(SoundBank.Music.Intro);
        musicBus.Play();
        musicBus.loop = false;

        StartCoroutine(CheckForEnd(musicBus, bank.Request(SoundBank.Music.Loop), true));
    }

    public void PlayAtEnd(AudioSource source, AudioClip next, bool loop)
    {
        StartCoroutine(CheckForEnd(source, next, loop));
    }

    IEnumerator CheckForEnd(AudioSource source, AudioClip next, bool loop)
    {
        yield return new WaitUntil(() => !source.isPlaying);
        source.clip = next;
        source.loop = loop;
        source.Play();
    }

    public void PlaySfx(SoundBank.SoundEffects clip)
    {
        AudioSource bus = effectBus[(int)clip];

        bus.clip = bank.Request(clip);
        bus.loop = false;
        bus.Play();
    }

    public void ChangeDynamicSound(float amount)
    {
        float total = amount / 5;
        total = Mathf.Clamp(total, 0, 1);
        dynamicBus.volume = 1 - total;
    }
}
