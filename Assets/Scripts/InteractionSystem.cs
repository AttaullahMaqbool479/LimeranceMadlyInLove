
using UnityEngine;
using UnityEngine.UI;

public class InteractionSystem : MonoBehaviour
{
    [Header("Detection Fields")]
    public Transform detectionPoint;    // Detection point
    private const float detectionRadius = 0.5f; // Detection radius
    public LayerMask detectionLayer;    // Detection layer
    public GameObject detectedObject; // Cached trigger object
    public bool isExamining;    // Flag set if player is in examine window

    [Header("Examine Fields")]
    public GameObject examineWindow;
    public UnityEngine.UI.Image examineImage;
    public Text examineText;

    void Update()
    {
        if(DetectObject() && IntereactInput())
        {
            detectedObject.GetComponent<Item>().Intereact();
        }
    }

    bool IntereactInput()
    {
        return ControlFreak2.CF2Input.GetKeyDown(KeyCode.E);
    }

    bool DetectObject()
    {
        Collider2D obj = Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, detectionLayer);
        if(obj==null)
        {
            detectedObject = null;
            return false;
        }
        else
        {
            detectedObject = obj.gameObject;
            return true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(detectionPoint.position, detectionRadius);
    }

    public void ExamineItem(Item item)
    {
        if(isExamining)
        {
            examineWindow.SetActive(false);
            isExamining = false;
        }
        else
        {
            examineImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
            examineText.text = item.descriptionText;
            examineWindow.SetActive(true);
            isExamining = true;
        }
    }
}