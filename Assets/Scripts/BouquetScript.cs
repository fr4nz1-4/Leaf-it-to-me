using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouquetScript : MonoBehaviour
{
    [SerializeField] private ItembarScript itembar;
    [SerializeField] private CloseUpScript closeUpScript;
    [SerializeField] private Sprite BouquetCloseUp;
    private bool found = false;

    // Update is called once per frame
    void Update()
    {
        if (found != false) return;
        if (itembar.FindItemSlotByName("flower 1 zoom in_0") && itembar.FindItemSlotByName("flower 2 zoom_0") &&
            itembar.FindItemSlotByName("flower 3 zoom_0"))
        {
            found = true;
            StartCoroutine(Bouquet());
        }
    }

    private IEnumerator Bouquet()
    {
        Debug.Log("inside Bouquet method");
        // yield return new WaitUntil(()=> !closeUpScript.closeUpPanel.activeSelf);
        yield return new WaitForSeconds(2);
        closeUpScript.ShowCloseUpPanel(BouquetCloseUp);
        itembar.RemoveItem("flower 1 zoom in_0");
        itembar.RemoveItem("flower 2 zoom_0");
        itembar.RemoveItem("flower 3 zoom_0");
        itembar.add_item(BouquetCloseUp);
    }
}
