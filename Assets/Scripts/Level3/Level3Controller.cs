using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level3Controller : MonoBehaviour
{
    public PlayerController player;
    public GameObject enemyTimeline;
    public bool isGameStarted;
    public GameObject enemyShadow;
    public float failDistance = 1f;  
    public float survivalTime = 15f; 

    private float timer = 0f;
    private bool isTaskFailed = false;
    public Image fillBar;
    public int itemsCollected;
    public GameObject task1PassedText;
    public GameObject levelWinText;
    public GameObject levelFailText;

    public void StartLevel()
    {
        StartCoroutine(startLevelSoroutine()); 
    }

    IEnumerator startLevelSoroutine()
    {
        player.enabled = true;
        yield return new WaitForSeconds(5f);
        enemyTimeline.SetActive(true);
        isGameStarted = true;
    }




    void Update()
    {
        if (isTaskFailed || !isGameStarted) return; // Stop checking if level already failed

        float distance = Vector3.Distance(player.transform.position, enemyShadow.transform.position);

        if (distance < failDistance)
        {
            FailTask();
        }
        else
        {
            timer += Time.deltaTime;
            UpdateFillBar();

            if (timer >= survivalTime)
            {
                PassTask();
            }
        }
    }

    void UpdateFillBar()
    {
        if (fillBar != null)
        {
            fillBar.fillAmount = (timer / survivalTime)/2;
        }
    }
    void FailTask()
    {
        SetPlayerToIdle();
        isTaskFailed = true;
        levelFailText.SetActive(true);
        enemyShadow.SetActive(false);
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Restart Level
    }

    void PassTask()
    {
        SetPlayerToIdle();
        isTaskFailed = true;
        task1PassedText.SetActive(true);
        Debug.Log("Level Passed! You survived the chase.");
        // Load the next level or show a victory screen
    }

    public void LevelComplete()
    {
        SetPlayerToIdle();
        levelWinText.SetActive(true);
    }
    
    
    public void SetPlayerToIdle()
    {
        player.enabled = false;
        player.GetComponent<Animator>().SetBool("Grounded", true);
        player.GetComponent<Animator>().SetInteger("AnimState", 0);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene("Level4");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Level3");
    }
    
}
    