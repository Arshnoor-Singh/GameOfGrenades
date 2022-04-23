/*
 * Authors: José Javier Ortiz Vega
 * Date Created:4/18/2022
 * Date Edited: 4/23/2022
 * Description: Slow script 
 *              Can be modified in the window editor
 *              
 *              ******************************
 *              ****         DONE         ****
 *              ******************************
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowScript : MonoBehaviour
{
    FragPartyController playerController;
    FragPartyCharacter playerInfo;

    //give health
    public void healPlayer(int healthToGive)
    {
        playerInfo.Damage(-healthToGive);
    }

    //slow movement stuff---
    public void SlowDownPlayer(float SlowPercent, float timer)
    {
        StartCoroutine(DonutEvent(SlowPercent, timer));
    }

    //donutEvent---
    IEnumerator DonutEvent(float SlowDownPercent, float SlowDownTimer)
    {
        playerController.moveSpeed -= SlowDownPercent;

        yield return new WaitForSeconds(SlowDownTimer);

        playerController.moveSpeed += SlowDownPercent;
    }
}
