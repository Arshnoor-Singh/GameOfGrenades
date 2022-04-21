/*
 * Authors: José Javier Ortiz Vega
 * Date Created:4/20/2022
 * Date Edited: 4/20/2022
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

public class AudioStorage : EditorWindow
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
    public float postmortemRequirement = 5;
    public List<AudioClip> postMortemSound = new List<AudioClip>();

    [Header("Multi Killstreak Audio")]
    //killstreaks
    public string multiElimination1 = "Double Kill";
    [SerializeField][Range(0f, 100f)] private float multiStreakRequirement1 = 5;
    public List<AudioClip> multiEliminationSound1;
    public string multiElimination2 = "Triple Kill";
    [SerializeField][Range(0f, 100f)] private float multiStreakRequirement2 = 10;
    public List<AudioClip> multiEliminationSound2;
    public string multiElimination3 = "Cuadruple Kill";
    [SerializeField][Range(0f, 100f)] private float multiStreakRequirement3 = 15;
    public List<AudioClip> multiEliminationSound3;
    public string multiElimination4 = "Quientople Kill";
    [SerializeField][Range(0f, 100f)] private float multiStreakRequirement4 = 20;
    public List<AudioClip> multiEliminationSound4;

    [Header("Milestone Audio")]
    //Milestone variables
    [SerializeField][Range(0, 100)] private int Milestone1 = 5;
    public List<AudioClip> Milestone1Sounds;
    [SerializeField][Range(0, 100)] private int Milestone2 = 10;
    public List<AudioClip> Milestone2Sounds;
    [SerializeField][Range(0, 100)] private int Milestone3 = 15;
    public List<AudioClip> Milestone3Sounds;
    [SerializeField][Range(0, 100)] private int Milestone4 = 20;
    public List<AudioClip> Milestone4Sounds;
    [SerializeField][Range(0, 100)] private int Milestone5 = 25;
    public List<AudioClip> Milestone5Sounds;

    //editor private
    public Vector2 scrollPosition = Vector2.zero;
    private List<int> bigList;

    [MenuItem("Window/Audio Storage")]
    public static void showWindow()
    {
        GetWindow<AudioStorage>("Audio Storage Window");
    }
    private void OnGUI()
    {
        Rect rectPos = EditorGUILayout.GetControlRect(true);
        Rect rectBox = new Rect(rectPos.x, rectPos.y, Screen.width, Screen.height);
        Rect rectView = new Rect(rectPos.x, rectPos.y, Screen.width, 10000f);

        //editor window
        editorStuff();
        GUI.Box(rectBox, GUIContent.none);
        scrollPosition = GUI.BeginScrollView(rectBox, scrollPosition, rectView, false, true);
        setMatchSound(); //Match sounds   
        setPostMortem(); //Post mortem sound 
        setEliminations(); //Elimination streak   
        setMilestone(); //milestones
        setProgrammerInformation(); //programmer guide
        GUI.EndScrollView();
    }

    //gui methods**************************************************************
    private GUIStyle guiStyle = new GUIStyle(); //create a new variable

    public void editorStuff()
    {
        guiStyle.fontSize = 30;
        guiStyle.alignment = TextAnchor.MiddleCenter;
        guiStyle.normal.textColor = Color.white;
        guiStyle.font = EditorStyles.boldFont;

        GUILayout.Label("Audio Storage", guiStyle);
    }
    public void setMatchSound()
    {
        EditorGUILayout.Space(15);
        guiStyle.fontSize = 20;
        guiStyle.alignment = TextAnchor.MiddleLeft;
        guiStyle.normal.textColor = Color.white;
        guiStyle.font = EditorStyles.boldFont;
        GUILayout.Label("Match Sounds", guiStyle);
    }
    public void setPostMortem()
    {
        EditorGUILayout.Space(15);
        guiStyle.fontSize = 20;
        guiStyle.alignment = TextAnchor.MiddleLeft;
        guiStyle.normal.textColor = Color.white;
        guiStyle.font = EditorStyles.boldFont;
        GUILayout.Label("Postmortem Sounds", guiStyle);

        // requirement
        GUILayout.BeginHorizontal();
        GUILayout.Label("Postmortem requirements");
        postmortemRequirement = EditorGUILayout.Slider(postmortemRequirement, 0, 100);
        GUILayout.EndHorizontal();
    }
    public void setEliminations()
    {
        //Elimination streak 1
        EditorGUILayout.Space(15);
        guiStyle.fontSize = 20;
        guiStyle.alignment = TextAnchor.MiddleLeft;
        guiStyle.normal.textColor = Color.white;
        guiStyle.font = EditorStyles.boldFont;
        GUILayout.Label("Elimination Sounds", guiStyle);

        //elimination 1
        GUILayout.BeginHorizontal();
        GUILayout.Label("Multi-Elimination 1");
        multiElimination1 = EditorGUILayout.TextField(multiElimination1);
        GUILayout.EndHorizontal();

        //Elimination streak 1 requirement
        GUILayout.BeginHorizontal();
        GUILayout.Label("Multi-Elimination 1 requirements");
        multiStreakRequirement1 = EditorGUILayout.Slider(multiStreakRequirement1, 0, 100);
        GUILayout.EndHorizontal();

        /*
        AudioClip test = null;//------------------------delete later
        //Elimination streak 1 sound
        GUILayout.BeginHorizontal();
        GUILayout.Label("Multi-Elimination 1 sounds");
        test = EditorGUILayout.ObjectField(test, typeof(AudioClip), false) as AudioClip;
        GUILayout.EndHorizontal();
        */

        //Elimination streak 2
        GUILayout.BeginHorizontal();
        GUILayout.Label("Multi-Elimination 2 ");
        multiElimination2 = EditorGUILayout.TextField(multiElimination2);
        GUILayout.EndHorizontal();

        //Elimination streak 2 requirement
        GUILayout.BeginHorizontal();
        GUILayout.Label("Multi-Elimination 2 requirements");
        multiStreakRequirement2 = EditorGUILayout.Slider(multiStreakRequirement2, 0, 100);
        GUILayout.EndHorizontal();

        //Elimination streak 3
        GUILayout.BeginHorizontal();
        GUILayout.Label("Multi-Elimination 3");
        multiElimination3 = EditorGUILayout.TextField(multiElimination3);
        GUILayout.EndHorizontal();

        //Elimination streak 3 requirement
        GUILayout.BeginHorizontal();
        GUILayout.Label("Multi-Elimination 3 requirements");
        multiStreakRequirement3 = EditorGUILayout.Slider(multiStreakRequirement3, 0, 100);
        GUILayout.EndHorizontal();

        //Elimination streak 4
        GUILayout.BeginHorizontal();
        GUILayout.Label("Multi-Elimination 4");
        multiElimination4 = EditorGUILayout.TextField(multiElimination4);
        GUILayout.EndHorizontal();

        //Elimination streak 4 requirement
        GUILayout.BeginHorizontal();
        GUILayout.Label("Multi-Elimination 4 requirements");
        multiStreakRequirement4 = EditorGUILayout.Slider(multiStreakRequirement4, 0, 100);
        GUILayout.EndHorizontal();
    }
    public void setMilestone()
    {
        EditorGUILayout.Space(15);
        guiStyle.fontSize = 20;
        guiStyle.alignment = TextAnchor.MiddleLeft;
        guiStyle.normal.textColor = Color.white;
        guiStyle.font = EditorStyles.boldFont;
        GUILayout.Label("Milestone Sounds", guiStyle);

        //Milestone 1 requirement
        GUILayout.BeginHorizontal();
        GUILayout.Label("Milestone 1 Requirement");
        Milestone1 = (int)EditorGUILayout.Slider(Milestone1, 0, 100);
        GUILayout.EndHorizontal();

        //Milestone 2 requirement
        GUILayout.BeginHorizontal();
        GUILayout.Label("Milestone 2 Requirement");
        Milestone1 = (int)EditorGUILayout.Slider(Milestone1, 0, 100);
        GUILayout.EndHorizontal();

        //Milestone 3 requirement
        GUILayout.BeginHorizontal();
        GUILayout.Label("Milestone 3 Requirement");
        Milestone1 = (int)EditorGUILayout.Slider(Milestone1, 0, 100);
        GUILayout.EndHorizontal();

        //Milestone 4 requirement
        GUILayout.BeginHorizontal();
        GUILayout.Label("Milestone 4 Requirement");
        Milestone1 = (int)EditorGUILayout.Slider(Milestone1, 0, 100);
        GUILayout.EndHorizontal();

        //Milestone 5 requirement
        GUILayout.BeginHorizontal();
        GUILayout.Label("Milestone 5 Requirement");
        Milestone1 = (int)EditorGUILayout.Slider(Milestone1, 0, 100);
        GUILayout.EndHorizontal();
    }
    public void setProgrammerInformation()
    {
        EditorGUILayout.Space(15);
        EditorGUILayout.HelpBox("If you encounter a bug let José know\n" +
            "If he tries to escape lure him with some berries and then trow a Master ball, that should do the trick", MessageType.Warning);
        EditorGUILayout.HelpBox("For Programmer use:\n\n" +
            "-> getMilestone(int milestone) //returns the milestone requirement of either of the 5 milestones\n" +
            "", MessageType.Info);
    }
    //gui methods*************************************************************

    //methods to call-------------------------------------------------
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
            return multiStreakRequirement1;
        else if (goal == 2)
            return multiStreakRequirement2;
        else if (goal == 3)
            return multiStreakRequirement3;
        else if (goal == 4)
            return multiStreakRequirement4;
        else
            return 0;
    }

    public string getMultiElimination(int name)
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
}
