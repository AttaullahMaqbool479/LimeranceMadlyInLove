using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fillbar;
    public float health;
    public bool isPlayerAttcking;
    public void LoseHealth(int value)
    {
        //Do nothing if dead
        if(health <= 0 || FindObjectOfType<PlayerController>().IsBlocking() || FindObjectOfType<PlayerController>().IsRolling())
            return;

        //Reduce health
        health -= value;
        //Play animation
        FindObjectOfType<PlayerController>().Hurt();
        //Refresh UI bar
        fillbar.fillAmount = health / FindObjectOfType<PlayerController>().GetMaxHP();
        //Check if health is 0 or less -> dead
        if(health <= 0)
        {
            FindObjectOfType<PlayerController>().Die();
            Debug.Log("You Died");
        }
    }

    void Start()
    {
        health = FindObjectOfType<PlayerController>().GetMaxHP();
    }
}
