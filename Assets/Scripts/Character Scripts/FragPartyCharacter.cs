/*
*  Author:         Ryan Jenkins
*  Date Created:   4/13/2022
*  Last Updated:   4/13/2022
*  Description:    The central script for characters in the game that contains the characters stat values and can send
*                  instruction to the Character Controller.
*/

using UnityEngine;
using System.Collections;
using Cinemachine;
using UnityEngine.UI;

public class FragPartyCharacter : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;   // The max health for the character, defaults to 100 unless set in inspector
    [SerializeField] public int _currentHealth;     // The current health value of the character

    public int EnemiesKilled = 0;
    public int deathCount = 0;
    public Text playerId;
    public bool isAlive = true;

    public AudioStorageScript AudioScript;
    public AudioSource Audio;

    PlayerSpawnner players;
    PlayerRespawn SpawnPlayer;
    CinemachineBrain Cinebrain;
    CharacterController CharacterControl;
    GameUI Health;
    ScoreBoard PlayerManager; 

    // Start is called before the first frame update
    void Start()
    {
        Cinebrain = transform.parent.GetComponentInChildren<CinemachineBrain>();
        players = FindObjectOfType<PlayerSpawnner>();
        CharacterControl = GetComponent<CharacterController>();
        SpawnPlayer = GetComponent<PlayerRespawn>();
        _currentHealth = _maxHealth; // Set the characters current health to the max health at the start of play
        Health = transform.parent.GetComponentInChildren<GameUI>();
        Health.changeLifeBar(_currentHealth);
        playerId.text = "P" + GetComponent<FragPartyController>().PlayerID;
        //PlayerManager = transform.parent.GetComponent<Player>();
        Audio = players.GetComponent<AudioSource>();
    }

    public void HealPlayer(int HealAmount)
    {
        _currentHealth += HealAmount;

        if (_currentHealth > 100)
        {
            _currentHealth = 100;
        }
    }

    // Apply damage to the characters health and check to see if they have been killed
    public void Damage(int amount, int playerID)
    {
        if (players.playerObjects[playerID].GetComponentInChildren<FragPartyController>().Team != transform.GetComponent<FragPartyController>().Team)
        {


            Debug.Log("Damage dealt " + amount + " by " + playerID);

            _currentHealth -= amount; // Reduce the characters health by the amount of damage dealt

            Health.currentLife = _currentHealth;

            StartCoroutine(DamageImpact()); // Visual cue for damage taken

            // Kill the character if their health falls below 0
            if (_currentHealth <= 0 && isAlive)
            {
                isAlive = false;
                players.playerObjects[playerID].GetComponentInChildren<FragPartyCharacter>().KillPoint();
                Kill();
            }


        }
        else
        {
            StartCoroutine(DamageImpact());
        }

    }

    public void KillPoint()
    {
        EnemiesKilled += 1;
        
        PlayKillAudio(deathCount, lifeKills);
    }

    // Kill the character and trigger their respawning
    public void Kill()
    {
<<<<<<< Updated upstream
        deathCount += 1;      
=======
        deathCount += 1;
        lifeKills = 0;      
>>>>>>> Stashed changes
        StartCoroutine(ActivateRagdollAndRespawn()); //This fucntion activates the ragdoll death and then respawns the player
        //PlayKillAudio(deathCount, lifeKills);
    }

    void PlayKillAudio()
    {

        Audio.clip = AudioScript.randomMilestoneClip(Random.Range(1, 5));

        Audio.Play();
    }

    private void ResetStats()
    {
        _currentHealth = 100;
        Health.currentLife = _currentHealth;
        isAlive = true;
        // Reset the characters health
        // Reset the characters grenade inventory
    }

    IEnumerator ActivateRagdollAndRespawn()
    {
        GetComponent<Animator>().enabled = false;
        Cinebrain.enabled = false;
        GetComponent<FragPartyController>().enabled = false;
        SetRagdollKinematicsAndColliders(true, false);

        yield return new WaitForSeconds(1.5f);

        SetRagdollKinematicsAndColliders(false, true);
        GetComponent<Animator>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<FragPartyController>().enabled = true;
        CharacterControl.enabled = true;
        Cinebrain.enabled = true;
        SpawnPlayer.Respawn(); //Respawns player
        ResetStats(); //Resets Stats
    }

    IEnumerator DamageImpact()
    {
        yield return new WaitForSeconds(0.1f);

        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    void SetRagdollKinematicsAndColliders(bool ColliderStatus, bool KinematicStatus)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();

        foreach (Collider collider in colliders)
        {
            collider.enabled = ColliderStatus;
        }

        foreach (Rigidbody rb in bodies)
        {
            rb.isKinematic = KinematicStatus;
        }
    }

}
