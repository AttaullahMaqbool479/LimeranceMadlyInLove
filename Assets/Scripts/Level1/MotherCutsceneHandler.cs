
using UnityEngine;
using UnityEngine.Playables;

public class MotherCutsceneHandler : MonoBehaviour
{
    public GameObject motherCutscene;
    public PlayerController player;
    public GameObject playerDiscussionPanel;
    public GameObject motherCharacter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(player == null)
                player = collision.gameObject.GetComponent<PlayerController>();
            SetPlayerToIdle();
            player.enabled = false;
            HandleMotherPanel();
            
        }
    }

    public void HandleMotherPanel()
    {
        var playableDirector = UIManagerLevel1.Instance.playableDirectorGirl;
        if (playableDirector != null && playableDirector.playableGraph.IsValid())
        {
            playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(0.1f);
        }
        playerDiscussionPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnPlayerSaysYes()
    {
        playerDiscussionPanel.SetActive(false);
        motherCutscene.SetActive(true);
    }

    public void OnPlayerSaysNo()
    {
        motherCharacter.SetActive(false);
        playerDiscussionPanel.SetActive(false);
        motherCutscene.SetActive(false);
        player.enabled = true;
        var playableDirector = UIManagerLevel1.Instance.playableDirectorGirl;
        if (playableDirector != null && playableDirector.playableGraph.IsValid())
        {
            playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(1);
        }
    }

    public void SetPlayerToIdle()
    {
        player.GetComponent<Animator>().SetBool("Grounded", true);
        player.GetComponent<Animator>().SetInteger("AnimState", 0);
    }
}
