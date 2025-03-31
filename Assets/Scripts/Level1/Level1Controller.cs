using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Level1Controller : MonoBehaviour
{
    public Transform player; // Reference to the player
    public Transform girl; // Reference to the girl
    public Image fillImage; // UI Image to adjust
    public float endCutSceneSpeed = 0.5f;
    public float maxDistance = 10f; // Maximum distance for zero fill
    public float minDistance = 1f; // Minimum distance for full fill
    public GameObject Ui;
    public bool isGameStarted;
    bool isGameRunning = true;
    bool isGirlCaught;
    void Update()
    {
        if (player == null || girl == null || fillImage == null || !isGameRunning)
            return;

        float distance = Vector3.Distance(player.position, girl.position);
        Debug.Log("distance is :" + distance);
        // Normalize the value between 0 and 1
        float fillAmount = Mathf.Clamp01(1 - ((distance - minDistance) / (maxDistance - minDistance)));

        // Update UI image fill
        fillImage.fillAmount = fillAmount;

        if(isGameStarted && distance <= 5 && !isGirlCaught)
        {
            isGirlCaught = true;
            UIManagerLevel1.Instance.endCutscene.SetActive(true);
            var playableDirector = UIManagerLevel1.Instance.playableDirectorGirl;
            if (playableDirector != null && playableDirector.playableGraph.IsValid())
            {
                playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(endCutSceneSpeed);
            }
            Debug.Log("He has successfully chased her");
        }

        if(distance >= 100 && !isGirlCaught)
        {
            LevelFail();
        }

        
    }
    public void StartCoroutineForGame()
    {
        StartCoroutine(StartLevel());
    }

    IEnumerator StartLevel()
    {
        yield return new WaitForSecondsRealtime(2.5f);
        player.GetComponent<PlayerController>().enabled = true;
        Ui.SetActive(true);
    }

    public void LevelComplete()
    {
        UIManagerLevel1.Instance.levelCompletePanel.SetActive(true);
        UIManagerLevel1.Instance.endCutscene.SetActive(false);
        girl.gameObject.GetComponent<SpriteRenderer>().flipX = false;
        player.GetComponent<PlayerController>().enabled = false;
        UIManagerLevel1.Instance.playableDirectorGirl.gameObject.SetActive(false);

    }
    public void LevelFail()
    {
        isGameRunning = false;
        UIManagerLevel1.Instance.levelFailPanel.SetActive(true);
    }

    public void NextLevel()
    {

        SceneManager.LoadScene("Level2");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Level1");
    }
}
