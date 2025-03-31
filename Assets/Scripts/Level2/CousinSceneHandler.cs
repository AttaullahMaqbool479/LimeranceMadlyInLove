using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CousinSceneHandler : MonoBehaviour
{
    public GameObject cousinCutscene;
    public PlayerController player;
    public GameObject boyQuestionPanel;
    public GameObject cousin;
    public Image bg1;
    public Image bg2;
    public Color failureColor;
    public Color successColor;
    public Image limeranceFillbar;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GetComponent<BoxCollider2D>().enabled = false;
            SetPlayerToIdle();
            boyQuestionPanel.SetActive(true);
        }

    }

    public void OnBoySaysYes()
    {
        boyQuestionPanel.SetActive(false);
        cousinCutscene.SetActive(true);
        
    }

    public void OnBoySaysNo()
    {
        boyQuestionPanel.SetActive(false);
        cousin.SetActive(false);
        cousinCutscene.SetActive(false);
        bg1.DOColor(failureColor, 1f).OnComplete(() =>
        {
            bg1.gameObject.SetActive(false);

        });
        limeranceFillbar.DOFillAmount(0.5f, 1f).SetEase(Ease.Linear);
        bg2.gameObject.SetActive(true);
        bg2.DOColor(successColor, 1f);
        player.enabled = true;
        Level2Controller.Instane.isTask1Completed = false;
    }

    public void SetPlayerToIdle()
    {
        player.enabled = false;
        player.GetComponent<Animator>().SetBool("Grounded", true);
        player.GetComponent<Animator>().SetInteger("AnimState", 0);
    }


    public void OnTask1Completed()
    {
        Level2Controller.Instane.isTask1Completed = true;
        cousin.SetActive(false);
        player.enabled = true;
        cousinCutscene.SetActive(false);
    }
}
