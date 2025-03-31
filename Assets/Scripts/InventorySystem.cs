
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    [Header("General Fields")]
    public List<GameObject> itemList;   // List of items picked up
    public bool isOpen;    // Inventory open flag

    [Header("UI Items Section")]
    public GameObject uiWindow;    // Inventory system window
    public Image[] itemImages;
    
    [Header("UI Items Section")]
    public GameObject uiDescriptionWindow;
    public Image descriptionImage;
    public Text descriptionTitle;

    void Update()
    {
        if(ControlFreak2.CF2Input.GetKeyDown(KeyCode.I))
        {
            ToogleInventory();
        }
    }

    void ToogleInventory()
    {
        isOpen = !isOpen;
        uiWindow.SetActive(isOpen);
    }

    public void PickUp(GameObject item) // Add the item to the items list
    {
        itemList.Add(item);
        UpdateInventory();
    }

    void UpdateInventory() // refresh UI elements in the inventory window
    {
        HideAll();

        for(int i=0; i<itemList.Count; i++)
        {
            itemImages[i].sprite = itemList[i].GetComponent<SpriteRenderer>().sprite;
            itemImages[i].gameObject.SetActive(true);
        }
    }

    void HideAll()  // Hide all item UI images
    {
        foreach(var item in itemImages)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void ShowDescription(int ID)
    {
        descriptionImage.sprite = itemImages[ID].sprite;
        descriptionTitle.text = itemList[ID].name;
        descriptionImage.gameObject.SetActive(true);
        descriptionTitle.gameObject.SetActive(true);
    }

    public void HideDescription()
    {
        descriptionImage.gameObject.SetActive(false);
        descriptionTitle.gameObject.SetActive(false);
    }
}
