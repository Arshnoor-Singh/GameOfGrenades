using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class SettingsMenu : MonoBehaviour
{
    public AudioMixer mm;
    public AudioMixer em;
    public AudioMixer vm;

    Resolution[] resolutions;
    public Dropdown resDrop;
    int currentRes = 0;

    private bool pause;

    void Start()
    {
        Debug.Log(resDrop.name);
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
        mm.SetFloat("volume", vol);
    }

    public void setEffectsVolume(float vol)
    {
        em.SetFloat("volume", vol);
    }

    public void setVoiceVolume(float vol)
    {
        vm.SetFloat("volume", vol);
    }

    //Graphics
    public void setQuality(int qIndex)
    {
        QualitySettings.SetQualityLevel(qIndex);
    }

    public void setScreenSize(int sIndex)
    {
        Screen.SetResolution(resolutions[sIndex].width, resolutions[sIndex].height, false);
<<<<<<< HEAD
        //qualDrop.GetComponent<AudioSource>().Play();
=======
>>>>>>> parent of 104fd4f0 (UI 99%)
        Debug.Log("bbbbbbb");
    }

    public void showSettingsMenu()
    {
        //GameObject settingsPanel;
        //GameObject sss;

    }
}
