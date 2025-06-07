using UnityEngine;

public class ItembarScript : MonoBehaviour
{
    public GameObject Itembar_folded_in;
    public GameObject Itembar_folded_out;

    void Start()
    {
        
    }
    
    public void fold_itembar_in()
    {
        Debug.Log("Itembar einklappen");
        Itembar_folded_out.SetActive(false);
        Itembar_folded_in.SetActive(true);
    }

    public void fold_itembar_out()
    {
        Debug.Log("Itembar ausklappen");
        Itembar_folded_out.SetActive(true);
        Itembar_folded_in.SetActive(false);
    }
}
