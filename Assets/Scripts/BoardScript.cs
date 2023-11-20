using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoardScript : MonoBehaviour
{
    public TextMeshPro topicTxt;
    private string boardAnswer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeTopicTxt(string txt)
    {
        topicTxt.text = txt;
    }

    public void ChangeAnswerTxt(string txt)
    {
        boardAnswer = txt;
    }

    public string GetAnswerTxt()
    {
        return boardAnswer;
    }
    public string GetTopicTxt()
    {
        return topicTxt.text;
    }

}
