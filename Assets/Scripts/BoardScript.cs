using TMPro;
using UnityEngine;

public class BoardScript : MonoBehaviour
{
    public TextMeshPro topicTxt;
    private string boardAnswer;
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
