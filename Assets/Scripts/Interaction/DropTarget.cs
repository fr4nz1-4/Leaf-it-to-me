using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DropTarget : MonoBehaviour, IDropHandler
{
    public string expectedItemName; // z. B. "Apfel" oder "Schlüssel"
    public GameObject finishedObject;
    private ItembarScript _itembarScript;

    private void Start()
    {
        _itembarScript = GameObject.Find("ItembarManager").GetComponent<ItembarScript>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;

        if (dropped == null) return;

        Image droppedImage = dropped.GetComponent<Image>();

        if (droppedImage != null && droppedImage.sprite != null)
        {
            Debug.Log("Dropped: " + droppedImage.sprite.name);

            if (droppedImage.sprite.name == expectedItemName)
            {
                Debug.Log("Richtiges Item benutzt!");

                // Item als Kind dieses DropTargets setzen
                dropped.transform.SetParent(this.transform);

                // Position des Items im DropTarget zurücksetzen (damit es „anliegt“)
                dropped.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                dropped.SetActive(false);
                gameObject.SetActive(false); // Outline deaktivieren
                finishedObject.SetActive(true); // Holz aktivieren
                Debug.Log("finishedObject platziert: " + finishedObject.activeSelf);
            }
            else
            {
                Debug.Log("Falsches Item!");
                // Optional: Item zurücksetzen oder anderweitig behandeln
            }
        }
    }

}