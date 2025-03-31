using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class MangoSecondTaskHandler : MonoBehaviour
{
    public PlayerController player;
    public GameObject secondTaskCutscene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GetComponent<BoxCollider2D>().enabled = false;
            SetPlayerToIdle();
            secondTaskCutscene.SetActive(true);
        }
    }

    public void SetPlayerToIdle()
    {
        player.enabled = false;
        player.GetComponent<Animator>().SetBool("Grounded", true);
        player.GetComponent<Animator>().SetInteger("AnimState", 0);
    }

    public void ReadyForTask2()
    {
        secondTaskCutscene.SetActive(false);
        player.enabled = true;
        secondTaskCutscene.SetActive(false);
    }
}
