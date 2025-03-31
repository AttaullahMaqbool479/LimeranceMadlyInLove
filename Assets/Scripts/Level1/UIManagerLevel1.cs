using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class UIManagerLevel1 : MonoBehaviour
{
    public GameObject limeranceMeter;
    public GameObject girlCutscene;
    public GameObject endCutscene;
    public PlayableDirector playableDirectorGirl;
    public GameObject levelCompletePanel;
    public GameObject levelFailPanel;
    public static UIManagerLevel1 Instance;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
    }

}
