using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{
    public enum InteractionType {NONE, PickUp, Examine, Coin}
    public InteractionType type;

    [Header("Examine")]
    public string descriptionText;
    public Sprite image;

    [Header("CustomEvents")]
    public UnityEvent customEvent;

    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
        gameObject.layer = 6;
    }

    public void Intereact()
    {
        switch(type)
        {
            case InteractionType.PickUp:
                FindObjectOfType<InventorySystem>().PickUp(gameObject);
                gameObject.SetActive(false);
                Debug.Log("PickUp");
                break;
            case InteractionType.Examine:
                FindObjectOfType<InteractionSystem>().ExamineItem(this);
                Debug.Log("Examine");
                break; 
            case InteractionType.Coin:
                FindObjectOfType<CoinSystem>().AddCoin();
                Debug.Log("Coin added");
                break;
            default:
                Debug.Log("NULL");
                break;
        }

        customEvent.Invoke();
    }
}
