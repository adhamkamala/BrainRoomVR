using UnityEngine;
using System;

public class BoardSystem : MonoBehaviour
{
    public GameObject[] boardLocations;
    public void CardLocator(GameObject card) {
        try {
            foreach (GameObject gameObject in boardLocations)
            {
                LocationHighlighter locHighlighter = gameObject.GetComponent<LocationHighlighter>();
                if (locHighlighter.isAvailable())
                {
                    card.transform.position = new Vector3(0.1f, 0f, 0f);
                    Vector3 localPosition = card.transform.localPosition;
                    Quaternion localRotation = card.transform.localRotation;
                    locHighlighter.changeAvailable(false);
                    locHighlighter.SetCardHolding(card);
                    card.GetComponent<CardScript>().SetLocationHolding(gameObject);
                    card.transform.parent = gameObject.transform;
                    card.transform.localPosition = localPosition;
                    card.transform.localRotation = localRotation;
                    card.transform.localScale = new Vector3(1f, 1f, 1f);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error in CardLocator: {ex.Message}");
        }

    }
    public bool CanCreateBoard()
    {
        foreach (GameObject gameObject in boardLocations)
        {
            LocationHighlighter locHighlighter = gameObject.GetComponent<LocationHighlighter>();
            if (locHighlighter.isAvailable())
            {
                return true;
            }
        }
        return false;
    }

}
