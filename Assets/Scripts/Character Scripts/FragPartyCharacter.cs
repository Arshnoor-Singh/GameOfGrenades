/*
*  Author:         Ryan Jenkins
*  Date Created:   4/13/2022
*  Last Updated:   4/13/2022
*  Description:    The central script for characters in the game that contains the characters stat values and can send
*                  instruction to the Character Controller.
*/

using UnityEngine;

public class FragPartyCharacter : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;   // The max health for the character, defaults to 100 unless set in inspector
    private int _currentHealth;     // The current health value of the character


    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = _maxHealth; // Set the characters current health to the max health at the start of play
    }

    // Apply damage to the characters health and check to see if they have been killed
    public void Damage(int amount)
    {
        _currentHealth -= amount; // Reduce the characters health by the amount of damage dealt

        // Kill the character if their health falls below 0
        if (_currentHealth < 0)
        {
            Kill();
        }
    
    }

    // Kill the character and trigger their respawning
    private void Kill()
    {
        // Kill the character
        // Reset the characters stats
        // Spawn the character at the level spawn location
    }

    private void ResetStats()
    {
        // Reset the characters health
        // Reset the characters grenade inventory
    }
}
