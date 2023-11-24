using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecordPanelScript : MonoBehaviour
{
    public TextMeshPro hintTxt;
    public TextMeshPro mainTxt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeHintTxt(string txt)
    {
        hintTxt.text = txt;
    }

    public void ChangeMainTxt(string txt)
    {
        mainTxt.text = txt;
    }
}
