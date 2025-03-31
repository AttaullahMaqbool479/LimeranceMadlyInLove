using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level4Controller : MonoBehaviour
{
    public PlayerController player;
    public bool isTask1Completed;
    public GameObject mangoCutscene;
    public GameObject task1SuccessText;
    public GameObject levelFailText;
    public GameObject levelSuccessText;
    public Transform mango;
    public bool isGameStarted = false; // Ensure this is set to true when the game starts
    public float failDistance = 0f;
    public Image distanceBar;
    public float maxDistance = 100f;
    public GameObject enemy1;
    public GameObject enemy2;
    
    public PlayableDirector playableDirectorGirl;
    
 
    
    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, mango.position);
        distanceBar.fillAmount = (maxDistance - distance)/100;
        Debug.Log("distance value is "+ distance + "fillAmount :"+ (maxDistance - distance)/100);
        if (isGameStarted && player != null && mango != null)
        {
            
             // Increase the bar as distance decreases
            Debug.Log("Game is startted: ");
            if (distance <= failDistance)
            {
                Debug.Log("Level Failed! Player reached Mango.");
                LevelFail();
            }
        }
    }

    
    public void StartLevel()
    {
        isGameStarted = true;
        player.enabled = true;
        mangoCutscene.SetActive(true);
    }

    public void Task1Success()
    {
        if(!isTask1Completed)
            task1SuccessText.SetActive(true);
        isTask1Completed = true;
        if (playableDirectorGirl != null && playableDirectorGirl.playableGraph.IsValid())
        {
            playableDirectorGirl.playableGraph.GetRootPlayable(0).SetSpeed(0.1f);
        }
    }
    
    public void LevelComplete()
    {
        player.enabled = false;
        levelSuccessText.SetActive(true);
    }

    public void LevelFail()
    {
        task1SuccessText.SetActive(false);
        if(enemy1!=null)
            enemy1.SetActive(false);
        if(enemy2 !=null)
            enemy2.SetActive(false);
        isGameStarted = false;
        player.enabled = false;
        mangoCutscene.SetActive(false);
        mango.gameObject.SetActive(false);
        levelFailText.SetActive(true);  
    }

    public void NextLevel()
    {
        SceneManager.LoadScene("Level1");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Level4");
    }

}
