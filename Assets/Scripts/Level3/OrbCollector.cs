using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OrbCollector : MonoBehaviour
{
    public Level3Controller levelcontroller;
    public AudioClip itemCollect;
    public AudioSource audioSource;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            levelcontroller.itemsCollected++;
            audioSource.PlayOneShot(itemCollect);
            levelcontroller.fillBar.fillAmount += 0.1f;
            if(levelcontroller.itemsCollected>=5)
            {
                levelcontroller.LevelComplete();
            }
            gameObject.SetActive(false);
        }


    }
}
