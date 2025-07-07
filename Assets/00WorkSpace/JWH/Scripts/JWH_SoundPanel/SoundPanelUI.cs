using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundPanel : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider seSlider;

    public Action<float> OnMasterVolumeChanged;
    public Action<float> OnBGMVolumeChanged;
    public Action<float> OnSEVolumeChanged;

    public Button helpButton;
    public Button homeButton;
    public Button staffButton;


    void Start()
    {
        masterSlider.onValueChanged.AddListener(value => OnMasterVolumeChanged?.Invoke(value));
        bgmSlider.onValueChanged.AddListener(value => OnBGMVolumeChanged?.Invoke(value));
        seSlider.onValueChanged.AddListener(value => OnSEVolumeChanged?.Invoke(value));

        masterSlider.value = SoundManager.Instance.masterVolume;
        bgmSlider.value = SoundManager.Instance.bgmVolume;
        seSlider.value = SoundManager.Instance.seVolume;

        OnMasterVolumeChanged += SoundManager.Instance.SetMasterVolume;
        OnBGMVolumeChanged += SoundManager.Instance.SetBGMVolume;
        OnSEVolumeChanged += SoundManager.Instance.SetSEVolume;

        helpButton.onClick.AddListener(OnClickHelp);
        homeButton.onClick.AddListener(OnClickHome);
        staffButton.onClick.AddListener(OnClickStaff);
    }

    void OnClickHelp()
    {
        
    }

    void OnClickHome()
    {
       
    }

    void OnClickStaff()
    {
        
    }
}
