using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BookstallTaskHandler : MonoBehaviour
{
    public GameObject bookStallQuestionPaneel;
    public PlayerController player;
    public GameObject book;
    public Image limeranceFillbar;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<BoxCollider2D>().enabled = false;
            bookStallQuestionPaneel.SetActive(true);
            SetPlayerToIdle();
            
            ///secondTaskCutscene.SetActive(true);
        }
    }

    public void SetPlayerToIdle()
    {
        player.enabled = false;
        player.GetComponent<Animator>().SetBool("Grounded", true);
        player.GetComponent<Animator>().SetInteger("AnimState", 0);
    }

    public void OnPlayerSaysYes()
    {
        
        bookStallQuestionPaneel.SetActive(false);
        Level2Controller.Instane.isTask2Completed = true;
        book.SetActive(false);
        Level2Controller.Instane.OnLevelCompleted();
    }

    public void OnPlayerSaysNo()
    {
        
        bookStallQuestionPaneel.SetActive(false);
        Level2Controller.Instane.isTask2Completed = false;
        limeranceFillbar.DOFillAmount(0.5f, 1f).SetEase(Ease.Linear);
        Level2Controller.Instane.OnLevelCompleted();
    }
}
