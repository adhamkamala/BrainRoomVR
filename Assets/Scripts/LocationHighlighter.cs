using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationHighlighter : MonoBehaviour
{
    private bool isAvailableBool = true;
    private GameObject cardHolding;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
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
