using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour {

    public Slider mVol;
    public Slider eVol;

    private void Start()
    {
        mVol.value = PersistentData.ambVolume;
        eVol.value = PersistentData.sfxVolume;
    }

    private void Update()
    {
        PersistentData.ambVolume = mVol.value;
        PersistentData.sfxVolume = eVol.value;
    }

    public void Hide()
    {
        PersistentData.FlushValues();
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
