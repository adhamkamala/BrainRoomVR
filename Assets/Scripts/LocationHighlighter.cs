using UnityEngine;

public class LocationHighlighter : MonoBehaviour
{
   
    private GameObject cardHolding;
    private bool isAvailableBool = true;

    public bool isAvailable() { 
        return isAvailableBool;
    }
    public void changeAvailable(bool passBool)
    {
        isAvailableBool = passBool;
    }
    public void SetCardHolding(GameObject tmp)
    {
        cardHolding= tmp;
    }
    public GameObject GetCardHolding()
    {
        return cardHolding;
    }
}
