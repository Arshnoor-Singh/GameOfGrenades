using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public AudioStorageScript AudioScript;
    public AudioSource Audio;

    private string Player_1_Deaths;
    private string Player_2_Deaths;
    private string Player_3_Deaths;
    private string Player_4_Deaths;

    [SerializeField] public int Team_A_Score;
    [SerializeField] public int Team_B_Score;

    PlayerSpawnner players;

    // Start is called before the first frame update
    void Start()
    {
        players = FindObjectOfType<PlayerSpawnner>();
        Audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(players.playerObjects[0] != null)
            Player_1_Deaths = players.playerObjects[0].GetComponentInChildren<FragPartyCharacter>().deathCount.ToString();

        if (players.playerObjects[1] != null)
            Player_2_Deaths = players.playerObjects[1].GetComponentInChildren<FragPartyCharacter>().deathCount.ToString();

        if (players.playerObjects[2] != null)
            Player_3_Deaths = players.playerObjects[2].GetComponentInChildren<FragPartyCharacter>().deathCount.ToString();

        if (players.playerObjects[3] != null)
            Player_4_Deaths = players.playerObjects[3].GetComponentInChildren<FragPartyCharacter>().deathCount.ToString();

        if (players.playerObjects[2] != null && players.playerObjects[3] != null)
            Team_A_Score = (players.playerObjects[2].GetComponentInChildren<FragPartyCharacter>().deathCount + players.playerObjects[3].GetComponentInChildren<FragPartyCharacter>().deathCount);

        if (players.playerObjects[0] != null && players.playerObjects[1] != null)
            Team_B_Score = (players.playerObjects[0].GetComponentInChildren<FragPartyCharacter>().deathCount + players.playerObjects[1].GetComponentInChildren<FragPartyCharacter>().deathCount);

        if(Team_A_Score == 1 || Team_B_Score == 1)
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
