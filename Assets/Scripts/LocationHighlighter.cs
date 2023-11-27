using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationHighlighter : MonoBehaviour
{
    private bool isAvailableBool = true;
    private GameObject cardHolding;
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
