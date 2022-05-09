using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string name;
    public int rounds;
    public bool isSelected;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        if(anim != null)
        {
            Debug.Log("YAY, it's working");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string darName()
    {
        return name;
    }

    public int darRounds()
    {
        return rounds;
    }

    public bool darIsSelected()
    {
        return isSelected;
    }

    public void changeIsSelected()
    {
        isSelected = !isSelected;
    }

    public void changeRounds(int r)
    {
        rounds = rounds + r;
    }

    public void Grow()
    {
        anim.Play("Grow");
    }

    public void Shrink()
    {
        anim.Play("Shrink");
    }
}
