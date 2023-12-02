using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [SerializeField] Slider masterSlider;
    [SerializeField] Toggle toggleMusic;
    [SerializeField] Toggle toggleSfx;

    // Start is called before the first frame update
    void Start()
    {
        if (SesionManager.Instance != null)
        {
            OnMasterChange(SesionManager.Instance.MasterVolume);
            OnMusicToggle(SesionManager.Instance.MusicMute);
            OnSfxToggle(SesionManager.Instance.MusicMute);
        }
        else
        {
            OnMasterChange();
            OnMusicToggle();
            OnSfxToggle();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMasterChange()
    {
        AudioManager.Instance.SetMasterVolume(masterSlider.value/100);
        if (SesionManager.Instance != null)
        {
            SesionManager.Instance.MasterVolume = masterSlider.value / 100;
        }
    }

    public void OnMasterChange(float val)
    {
        AudioManager.Instance.SetMasterVolume(val);
        masterSlider.value = val * 100;
    }

    public void OnMusicToggle()
    {
        AudioManager.Instance.SetMuteMusicBus(!toggleMusic.isOn);
        if (SesionManager.Instance != null)
        {
            SesionManager.Instance.MusicMute = toggleMusic.isOn;
        }
    }

    public void OnMusicToggle(bool toggle)
    {
        AudioManager.Instance.SetMuteMusicBus(!toggle);
        toggleMusic.isOn = toggle;
    }

    public void OnSfxToggle()
    {
        AudioManager.Instance.SetMuteSfxBus(!toggleSfx.isOn);
        if (SesionManager.Instance != null)
        {
            SesionManager.Instance.SFXMute = toggleSfx.isOn;
        }
    }

    public void OnSfxToggle(bool toggle)
    {
        AudioManager.Instance.SetMuteSfxBus(!toggle);
        toggleSfx.isOn = toggle;
    }
}
