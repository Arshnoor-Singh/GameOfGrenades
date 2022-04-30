using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public AudioStorageScript AudioScript;
    public AudioSource Audio;

    public string Player_1_Deaths;
    public string Player_2_Deaths;
    public string Player_3_Deaths;
    public string Player_4_Deaths;

    public int[] playerKills = new int[4];

    [SerializeField] public int Team_A_Score = 0;
    [SerializeField] public int Team_B_Score = 0;
    [SerializeField] public int VictoryTarget = 15;


    PlayerSpawnner players;

    // Start is called before the first frame update
    void Start()
    {
        players = GetComponent<PlayerSpawnner>();
        Audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        for(int i = 0; i < playerKills.Length; i++)
        {
            if(players.playerObjects[i] != null)
            {
                playerKills[i] = players.playerObjects[i].GetComponentInChildren<FragPartyCharacter>().EnemiesKilled;
            }
        }

        Team_A_Score = playerKills[0] + playerKills[1];
        Team_B_Score = playerKills[2] + playerKills[3];

        if (players.playerObjects[0] != null)
            Player_1_Deaths = players.playerObjects[0].GetComponentInChildren<FragPartyCharacter>().deathCount.ToString();
            

        if (players.playerObjects[1] != null)
            Player_2_Deaths = players.playerObjects[1].GetComponentInChildren<FragPartyCharacter>().deathCount.ToString();

        if (players.playerObjects[2] != null)
            Player_3_Deaths = players.playerObjects[2].GetComponentInChildren<FragPartyCharacter>().deathCount.ToString();

        if (players.playerObjects[3] != null)
            Player_4_Deaths = players.playerObjects[3].GetComponentInChildren<FragPartyCharacter>().deathCount.ToString();

        

        if(Team_A_Score == VictoryTarget || Team_B_Score == VictoryTarget)
        {
            PlayVictoryAudio();
        }

    }

    void PlayVictoryAudio()
    {

        Audio.clip = AudioScript.randomVictoryClip();

        Audio.Play();
    }

}
