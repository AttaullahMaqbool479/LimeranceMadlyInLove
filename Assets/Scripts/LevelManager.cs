using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    Vector2 playerInitPosition;

    void Start()
    {
        playerInitPosition = FindObjectOfType<PlayerController>().transform.position;
    }

    public void Restart(bool totalReset)
    {
        SceneManager.LoadScene(1);
    }
}
