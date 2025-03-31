using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Task1Collider : MonoBehaviour
{
    public GameObject enemy;
    public GameObject enemyCutscene;
    public PlayerController player;
    public Level4Controller level4Controller;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SetPlayerToIdle();
            if (level4Controller.playableDirectorGirl != null && level4Controller.playableDirectorGirl.playableGraph.IsValid())
            {
                level4Controller.playableDirectorGirl.playableGraph.GetRootPlayable(0).SetSpeed(0.05f);
            }
            enemy.GetComponent<EnemyAILevel4>().collider1.SetActive(true);
            enemy.GetComponent<EnemyAILevel4>().collider2.SetActive(true);
            enemyCutscene.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void StartFight()
    {
        player.enabled = true;
        enemy.GetComponent<EnemyAILevel4>().isTaskStarted = true;
    }
    
    public void SetPlayerToIdle()
    {
        player.enabled = false;
        player.GetComponent<Animator>().SetBool("Grounded", true);
        player.GetComponent<Animator>().SetInteger("AnimState", 0);
    }
}
