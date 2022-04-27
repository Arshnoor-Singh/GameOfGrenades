/*
 * Authors: José Javier Ortiz Vega
 * Date Created:4/20/2022
 * Date Edited: 4/21/2022
 * Description: This will allow to store audio files for later use 
 *              Can be modified in the window editor
 *              
 *              ******************************
 *              ****         DONE         ****
 *              ******************************
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "AudioStorage", menuName = "ScriptableObjects/AudioFiles", order = 1)]
public class AudioStorageScript : ScriptableObject
{
    //starting point for the code
    [Header("Audio Storage (NOT REQUIRED)")]
    public AudioSource audioSource;

    [Header("Match sounds")]
    //MatchVictory sounds
    public List<AudioClip> victory = new List<AudioClip>();
    public List<AudioClip> defeat = new List<AudioClip>();

    [Header("Kill Types")]
    //killtypes
    [SerializeField][Range(0f, 100f)] private float postmortemRequirement = 5;
    public List<AudioClip> postMortemSound = new List<AudioClip>();

    [Header("Multi Killstreak Audio")]
    //killstreaks
    public string multiElimination1 = "Double Kill";
    [SerializeField][Range(0f, 100f)] public float multiStreakRequirement1 = 5, multiStreakRequirementTime1 = 3f;
    public List<AudioClip> multiEliminationSound1;
    public string multiElimination2 = "Triple Kill";
    [SerializeField][Range(0f, 100f)] public float multiStreakRequirement2 = 10, multiStreakRequirementTime2 = 3f;
    public List<AudioClip> multiEliminationSound2;
    public string multiElimination3 = "Cuadruple Kill";
    [SerializeField][Range(0f, 100f)] public float multiStreakRequirement3 = 15, multiStreakRequirementTime3 = 3f;
    public List<AudioClip> multiEliminationSound3;
    public string multiElimination4 = "Quientople Kill";
    [SerializeField][Range(0f, 100f)] public float multiStreakRequirement4 = 20, multiStreakRequirementTime4 = 3f;
    public List<AudioClip> multiEliminationSound4;
    public string multiElimination5 = "Sextople Kill";
    [SerializeField][Range(0f, 100f)] public float multiStreakRequirement5 = 30, multiStreakRequirementTime5 = 3f;
    public List<AudioClip> multiEliminationSound5;

    string test = "hi";
    [Header("Milestone Audio")]
    //Milestone variables
    [SerializeField][Range(0, 100)] private int Milestone1 = 5;
    public string Milestone1Name = "";
    public List<AudioClip> Milestone1Sounds;
    [SerializeField][Range(0, 100)] private int Milestone2 = 10;
    public string Milestone2Name = "";
    public List<AudioClip> Milestone2Sounds;
    [SerializeField][Range(0, 100)] private int Milestone3 = 15;
    public string Milestone3Name = "";
    public List<AudioClip> Milestone3Sounds;
    [SerializeField][Range(0, 100)] private int Milestone4 = 20;
    public string Milestone4Name = "";
    public List<AudioClip> Milestone4Sounds;
    [SerializeField][Range(0, 100)] private int Milestone5 = 25;
    public string Milestone5Name = "";
    public List<AudioClip> Milestone5Sounds;

    //return methods-----------------------------------------------------------------------------------
    public AudioSource getAudioSource()
    {
        return audioSource;
    }

    //returns library size
    public float getStreakRequirementTime(int multiEliminationSoundTime)
    {
        if (multiEliminationSoundTime == 1)
            return multiStreakRequirementTime1;
        else if (multiEliminationSoundTime == 2)
            return multiStreakRequirementTime2;
        else if (multiEliminationSoundTime == 3)
            return multiStreakRequirementTime3;
        else if (multiEliminationSoundTime == 4)
            return multiStreakRequirementTime4;
        else if (multiEliminationSoundTime == 5)
            return multiStreakRequirementTime5;
        else
            return 0;
    }
    public int getVictorySize()
    {
        return victory.Count;
    }
    public int getDefeatSize()
    {
        return defeat.Count;
    }
    public int getPostMortemSize()
    {
        return postMortemSound.Count;
    }
    public int getMultiEliminationSize(int elimination)
    {
        if (elimination == 1)
            return multiEliminationSound1.Count;
        else if (elimination == 2)
            return multiEliminationSound2.Count;
        else if (elimination == 3)
            return multiEliminationSound3.Count;
        else if (elimination == 4)
            return multiEliminationSound4.Count;
        else if (elimination == 5)
            return multiEliminationSound5.Count;
        else
            return 0;
    }
    public int getMilestoneSize(int milestone)
    {
        if (milestone == 1)
            return Milestone1Sounds.Count;
        else if (milestone == 2)
            return Milestone2Sounds.Count;
        else if (milestone == 3)
            return Milestone3Sounds.Count;
        else if (milestone == 4)
            return Milestone4Sounds.Count;
        else if (milestone ==5)
            return Milestone5Sounds.Count;
        else
            return 0;
    }
    //end of library size

    //returns sounds
    //returns random victory sound
    public AudioClip randomVictoryClip()
    {
        return victory[Random.Range(0, victory.Count)];
    }
    //returns specific victory sound
    public AudioClip specificVictoryClip(int clip)
    {
        return victory[(clip - 1)];
    }
    //returns random defeat sound
    public AudioClip randomDefeatClip()
    {
        return defeat[Random.Range(0, defeat.Count)];
    }
    //returns specific defeat sound
    public AudioClip specificDefeatClip(int clip)
    {
        return defeat[(clip - 1)];
    }
    //returns random postmortem sound
    public AudioClip randomPostmortemClip()
    {
        return postMortemSound[Random.Range(0, postMortemSound.Count)];
    }
    //returns specific postmortem sound
    public AudioClip specificPostMortemClip(int clip)
    {
        return postMortemSound[(clip - 1)];
    }
    //returns random multi elimination sound
    public AudioClip randomMultiEliminationClip(int elimination)
    {
        if (elimination == 1)
            return multiEliminationSound1[Random.Range(0, multiEliminationSound1.Count)];
        else if (elimination == 2)
            return multiEliminationSound2[Random.Range(0, multiEliminationSound2.Count)];
        else if (elimination == 3)
            return multiEliminationSound3[Random.Range(0, multiEliminationSound3.Count)];
        else if (elimination == 4)
            return multiEliminationSound4[Random.Range(0, multiEliminationSound4.Count)];
        else if (elimination == 5)
            return multiEliminationSound5[Random.Range(0, multiEliminationSound5.Count)];
        else
            return null;
    }
    //returns specific multi elimination sound
    public AudioClip specificMultiEliminationClip(int elimination, int clip)
    {
        if (elimination == 1)
            return multiEliminationSound1[(clip - 1)];
        else if (elimination == 2)
            return multiEliminationSound2[(clip - 1)];
        else if (elimination == 3)
            return multiEliminationSound3[(clip - 1)];
        else if (elimination == 4)
            return multiEliminationSound4[(clip - 1)];
        else if(elimination == 5)
            return multiEliminationSound5[(clip - 1)];
        else
            return null;
    }
    //returns random milestone sound
    public AudioClip randomMilestoneClip(int milestone)
    {
        if (milestone == 1)
            return Milestone1Sounds[Random.Range(0, Milestone1Sounds.Count)];
        else if (milestone == 2)
            return Milestone2Sounds[Random.Range(0, Milestone2Sounds.Count)];
        else if (milestone == 3)
            return Milestone3Sounds[Random.Range(0, Milestone3Sounds.Count)];
        else if (milestone == 4)
            return Milestone4Sounds[Random.Range(0, Milestone4Sounds.Count)];
        else if (milestone == 5)
            return Milestone5Sounds[Random.Range(0, Milestone5Sounds.Count)];  
        else
            return null;
    }
    //returns specific milestone sound
    public AudioClip specificMilestoneClip(int milestone, int clip)
    {
        if (milestone == 1)
            return Milestone1Sounds[(clip - 1)];
        else if (milestone == 2)
            return Milestone2Sounds[(clip - 1)];
        else if (milestone == 3)
            return Milestone3Sounds[(clip - 1)];
        else if (milestone == 4)
            return Milestone4Sounds[(clip - 1)];
        else if (milestone == 5)
            return Milestone5Sounds[(clip - 1)];
        else
            return null;
    }
    //end of return sounds

    //returns the multi elimination requirement
    public float getMultiEliminationRequirement(int requirement)
    {
        if (requirement == 1)
            return multiStreakRequirement1;
        else if (requirement == 2)
            return multiStreakRequirement2;
        else if (requirement == 3)
            return multiStreakRequirement3;
        else if (requirement == 4)
            return multiStreakRequirement4;
        else
            return 0;
    }

    //returns the multi elimination name
    public string getMultiEliminationName(int name)
    {
        if (name == 1)
            return multiElimination1;
        else if (name == 2)
            return multiElimination2;
        else if (name == 3)
            return multiElimination3;
        else if (name == 4)
            return multiElimination4;
        else
            return null;
    }

    //returns the milestone requirement
    public int getMilestoneRequirement(int milestone)
    {
        if (milestone == 1)
            return Milestone1;
        else if (milestone == 2)
            return Milestone2;
        else if (milestone == 3)
            return Milestone3;
        else if (milestone == 4)
            return Milestone4;
        else if (milestone == 5)
            return Milestone5;
        else
            return 0;
    }
}
