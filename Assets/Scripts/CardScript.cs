using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    public TextMeshPro mainTitleTxt;
    public TextMeshPro subTitleTxt;
    private GameObject locationHolding;
    public void ChangeMainTxt(string txt)
    {
        mainTitleTxt.text = txt;
    }

    public void ChangeSubTxt(string txt)
    {
        subTitleTxt.text = txt;
    }

    public void SetLocationHolding(GameObject tmp)
    {
        locationHolding = tmp;
    }

    public GameObject GetLocationHolding()
    {
        return locationHolding;
    }
}
