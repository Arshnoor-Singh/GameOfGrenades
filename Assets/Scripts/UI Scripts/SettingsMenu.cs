using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class SettingsMenu : MonoBehaviour
{
    public AudioMixer mm;

    Resolution[] resolutions;
    public Dropdown resDrop;
    public Dropdown qualDrop;
    int currentRes = 0;

    private bool pause;

    void Start()
    {
        pause = false;
        resolutions = Screen.resolutions;
        resDrop.ClearOptions();

        List<string> options = new List<string>();
        for(int i = 0; i < resolutions.Length; i++)
        {
            string op = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(op);
            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentRes = i;
            }
        }
        resDrop.AddOptions(options);
        resDrop.value = currentRes;
        resDrop.RefreshShownValue();
    }

    //Audio
    public void setMusicVolume(float vol)
    {
        mm.SetFloat("MusicVolume", vol);
    }

    public void setEffectsVolume(float vol)
    {
        mm.SetFloat("SFXVolume", vol);
    }

    public void setVoiceVolume(float vol)
    {
        mm.SetFloat("VoiceVolume", vol);
    }

    //Graphics
    public void setQuality(int qIndex)
    {
        QualitySettings.SetQualityLevel(qIndex);
        qualDrop.GetComponent<AudioSource>().Play();
    }

    public void setScreenSize(int sIndex)
    {
        Screen.SetResolution(resolutions[sIndex].width, resolutions[sIndex].height, false);
        Debug.Log("bbbbbbb");
        qualDrop.GetComponent<AudioSource>().Play();
    }

    public void showSettingsMenu()
    {

    }
}
