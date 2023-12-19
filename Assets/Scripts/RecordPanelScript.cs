using TMPro;
using UnityEngine;

public class RecordPanelScript : MonoBehaviour
{
    public TextMeshPro hintTxt;
    public TextMeshPro mainTxt;
    public void ChangeHintTxt(string txt)
    {
        hintTxt.text = txt;
    }
    public void ChangeMainTxt(string txt)
    {
        mainTxt.text = txt;
    }
}
