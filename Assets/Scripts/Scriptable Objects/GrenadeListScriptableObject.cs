/*
*  Author:             Ryan Jenkins
*  Creation Date:      4/19/2022
*  Last Updated:       4/20/2022
*  Description:        
*/


using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GrenadeListScriptableObject", order = 2)]
public class GrenadeListScriptableObject : ScriptableObject
{
    public GameObject[] list;
    
    public GameObject Get(int index)
    {
        return index >= 0 && index < list.Length ? list[index] : null;
    }
    
    public GameObject GetRandom()
    {
        return Get(Random.Range(0, list.Length));
    }
}
