/*
 * Author: José Javier Ortiz Vega
 * Date added: 4/20/2022
 * Date ended: 4/20/2022
 * Description: Storage for the sound that the players can do
 * 
 * functions that return stuff
 *  getMilestone(int NumberOfTheMilestone);
 *  getMultikillstreakGoal(float goal);
 *  getKillstreakName(int name)
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(AudioSource))]
[CreateAssetMenu(fileName = "Storage", menuName = "ScriptableObjects/AudioFiles", order = 1)]
public class TestingAudio : ScriptableObject
{
    //starting point for the code
    [Header("Audio Storage")]
    public AudioSource audioSource;

    [Header("Match sounds")]
    //MatchVictory sounds
    public List<AudioClip> victory = new List<AudioClip>();
    public List<AudioClip> defeat = new List<AudioClip>();

    [Header("Kill Types")]
    //killtypes
    public List<AudioClip> postMortem = new List<AudioClip>();

    [Header("Multi Killstreak Audio")]
    //killstreaks
    public string multiKillStreaks1 = "Double Kill";
    [SerializeField][Range(0f, 100f)] private float multiKillStreaksGoal1 = 5;
    public List<AudioClip> multiKillStreaksGoalAudio1;
    public string multiKillStreaks2 = "Triple Kill";
    [SerializeField][Range(0f, 100f)] private float multiKillStreaksGoal2 = 10;
    public List<AudioClip> multiKillStreaksGoalAudio2;
    public string multiKillStreaks3 = "Cuadruple Kill";
    [SerializeField][Range(0f, 100f)] private float multiKillStreaksGoal3 = 15;
    public List<AudioClip> multiKillStreaksGoalAudio3;
    public string multiKillStreaks4 = "Quientople Kill";
    [SerializeField][Range(0f, 100f)] private float multiKillStreaksGoal4 = 20;
    public List<AudioClip> multiKillStreaksGoalAudio4;

    [Header("Milestone Audio")]
    //Milestone variables
    [SerializeField][Range(0,100)] private int Milestone1 = 5;
    public List<AudioClip> Milestone1Sounds;
    [SerializeField][Range(0, 100)] private int Milestone2 = 10;
    public List<AudioClip> Milestone2Sounds;
    [SerializeField][Range(0, 100)] private int Milestone3 = 15;
    public List<AudioClip> Milestone3Sounds;
    [SerializeField][Range(0, 100)] private int Milestone4 = 20;
    public List<AudioClip> Milestone4Sounds;
    [SerializeField][Range(0, 100)] private int Milestone5 = 25;
    public List<AudioClip> Milestone5Sounds;

    //calling methods
    public int getMilestone(int milestone)
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

    public float getMultikillstreakGoal(int goal)
    {
        if (goal == 1)
            return multiKillStreaksGoal1;
        else if (goal == 2)
            return multiKillStreaksGoal2;
        else if (goal == 3)
            return multiKillStreaksGoal3;
        else if (goal == 4)
            return multiKillStreaksGoal4;
        else
            return 0;
    }

    public string getKillstreakName(int name)
    {
        if (name == 1)
            return multiKillStreaks1;
        else if (name == 2)
            return multiKillStreaks2;
        else if (name == 3)
            return multiKillStreaks3;
        else if (name == 4)
            return multiKillStreaks4;
        else
            return null;
    }
}
