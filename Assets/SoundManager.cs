using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioClip die, shoot, hack1, hack2, hack3, start, ambient;
    public AudioSource ambientSound;

    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        PersistentData.Load();
        Instance = this;
    }

    public void PlaySound(Sound s, float vol = -1)
    {
        AudioClip c = null;
        float volMod = 1;

        switch (s)
        {
            case Sound.Die:
                c = die;
                break;
            case Sound.Shoot:
                volMod = 0.4f;
                c = shoot;
                break;
            case Sound.Hack1:
                c = hack1;
                break;
            case Sound.Hack2:
                c = hack2;
                break;
            case Sound.Start:
                c = start;
                break;
            case Sound.Hack3:
                c = hack3;
                break;
        }

        if (c == null) return;
        AudioSource.PlayClipAtPoint(c, Camera.main.transform.position, (vol < 0 ? PersistentData.sfxVolume : vol) * volMod);
    }

    private void Update()
    {
        ambientSound.volume = PersistentData.ambVolume;
    }
}

public enum Sound
{
    Die, Shoot, Hack1, Hack2, Start, Hack3
}