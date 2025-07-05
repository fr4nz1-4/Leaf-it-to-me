using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ItembarScript : MonoBehaviour
{
    public GameObject itembar_folded_in;
    public GameObject itembar_folded_out;
    public List<Image> itemSlots;
    public bool _isItembarfoldedOut;
    
    private void Awake() // not working
    {
        // DontDestroyOnLoad(this.gameObject);   // ItembarManager bleibt erhalten
        // //DontDestroyOnLoad(itembar_folded_out);      // ItembarPanel bleibt ebenfalls erhalten
    }
    
    private void Start()
    {
        foreach (var slot in itemSlots)
        {
            slot.color = new Color(1, 1, 1, 0); // komplett transparent
            slot.sprite = null;
            slot.enabled = true; // Komponente bleibt aktiv
        }
    }

    // private void OnMouseOver()
    // {
    //     if (_isItembarfoldedOut)
    //     {
    //         fold_itembar_in();
    //     }
    //     else
    //     {
    //         fold_itembar_out();
    //     }
    // }

    public void fold_itembar_in()
    {
        Debug.Log("Itembar einklappen");
        itembar_folded_out.SetActive(false);
        itembar_folded_in.SetActive(true);
    }

    public void fold_itembar_out()
    {
        Debug.Log("Itembar ausklappen");
        itembar_folded_out.SetActive(true);
        itembar_folded_in.SetActive(false);
    }

    public void add_item(Sprite itemSprite)
    {
        Debug.Log("AddItem aufgerufen!");
        foreach (var slot in itemSlots)
        {
            Debug.Log("in schleife drin");
            if (slot.sprite == null)
            {
                slot.sprite = itemSprite;
                slot.color = new Color(1, 1, 1, 1); // sichtbar machen (volle Deckkraft)
                Debug.Log("Item (" + slot.sprite.name + ") zu Inventar hinzugefügt");
                // DontDestroyOnLoad(slot.gameObject);
                return;
            }
        }
    }
    
    public Image FindItemSlotByName(string itemName)
    {
        foreach (var slot in itemSlots)
        {
            if (slot.sprite != null && slot.sprite.name == itemName)
            {
                Debug.Log("Item gefunden: " + itemName);
                return slot; // Gibt das passende Slot-Image zurück
            }
        }

        Debug.Log("Item nicht gefunden: " + itemName);
        return null; // Falls nichts gefunden wurde
    }
    
    public void ReplaceItemSprite(Image slot, Sprite newSprite)
    {
        if (slot != null)
        {
            slot.sprite = newSprite;
            slot.color = new Color(1, 1, 1, 1); // sicherstellen, dass es sichtbar ist
            Debug.Log("Sprite erfolgreich ausgetauscht mit: " + newSprite.name);
        }
    }

}
