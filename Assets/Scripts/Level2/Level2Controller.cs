using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2Controller : MonoBehaviour
{

    public PlayerController player;
    public GameObject startCutScene;
    public GameObject girlWalkAnimation;
    public bool isTask1Completed;
    public bool isTask2Completed;
    public static Level2Controller Instane;
    public GameObject levelSuccessPanel;
    public GameObject levelFailPanel;
    public GameObject levelFailText;
    public GameObject levelSuccessText;

    private void Awake()
    {
        if (!Instane)
            Instane = this;
    }

    public void StartGame()
    {
        player.enabled = true;
        startCutScene.SetActive(false);
    }

    public bool IsLevelSuccess()
    {
        return isTask1Completed || isTask2Completed;
    }

    public void OnLevelCompleted()
    {
        if (IsLevelSuccess())
            levelSuccessText.SetActive(true);
        else
            levelFailText.SetActive(true);
    }

    public void LevelSuccess()
    {
        player.enabled = false;
        levelSuccessPanel.SetActive(true);
    }

    public void LevelFail()
    {
        player.enabled = false;
        levelFailPanel.SetActive(true);
    }


    public void RestartLevel()
    {
        SceneManager.LoadScene("Level2");
    }

    public void NextLeveel()
    {
        SceneManager.LoadScene("Level3");
    }
}
