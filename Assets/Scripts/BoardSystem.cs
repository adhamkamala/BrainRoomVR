using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSystem : MonoBehaviour
{
    public GameObject[] boardLocations;
    public void CardLocator(GameObject card) {

        foreach (GameObject g in boardLocations)
        {
            LocationHighlighter locHigh = g.GetComponent<LocationHighlighter>();
            if (locHigh.isAvailable())
            {
                card.transform.position = new Vector3(0.1f,0f,0f);
                Vector3 localPos = card.transform.localPosition;
                Quaternion localRot = card.transform.localRotation;
                locHigh.changeAvailable(false);
                locHigh.SetCardHolding(card);
                card.GetComponent<CardScript>().SetLocationHolding(g);
                card.transform.position += new Vector3(0.1f, 0f, 0f);
                card.transform.parent = g.transform;
                card.transform.localPosition = localPos;
                card.transform.localRotation = localRot;
                card.transform.localScale = new Vector3(1f, 1f, 1f);
                break;
            }
        }
    }

    public bool CanCreate()
    {
        foreach (GameObject g in boardLocations)
        {
            LocationHighlighter locHigh = g.GetComponent<LocationHighlighter>();
            if (locHigh.isAvailable())
            {
                return true;
            }
        }
        return false;
    }

}
