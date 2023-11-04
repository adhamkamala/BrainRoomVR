using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    public TextMeshPro mainTitleTxt;
    public TextMeshPro subTitleTxt;
    private GameObject locationHolding;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
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
