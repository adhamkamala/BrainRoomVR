using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSystem : MonoBehaviour
{
    public GameObject[] boardLocations;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CardLocator(GameObject card) {

        foreach (GameObject g in boardLocations)
        {
            LocationHighlighter locHigh = g.GetComponent<LocationHighlighter>();
            if (locHigh.isAvailable())
            {
                card.transform.position = new Vector3(g.transform.position.x+0.005f, g.transform.position.y, g.transform.position.z);
                locHigh.changeAvailable(false);
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
