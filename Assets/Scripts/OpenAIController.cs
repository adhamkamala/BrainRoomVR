using OpenAI_API.Chat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using OpenAI;
using Newtonsoft.Json;
using Unity.VisualScripting;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Xml.Schema;

public class OpenAIController : MonoBehaviour
{
    public GameObject cardSystem;
    public GameObject boardsSystem;
    public GameObject spawnSystem;

    private readonly string fileName = "output.wav";
    private AudioClip clip;
    private OpenAI_API.OpenAIAPI api = new OpenAI_API.OpenAIAPI("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");
    private Conversation chat;
    private OpenAIApi openai = new OpenAIApi("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");


    // Start is called before the first frame update
    void Start()
    {
       chat = api.Chat.CreateConversation();
       chat.Model = "gpt-3.5-turbo-1106";
        //  _ = ModeWhiteBoardSetupModel();
        // ModeMindMapSetupModel();
    }
    private void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame) // for testing purposes
        {
            //ModeMindMapSendMessage("Wie kann ich schneller zu punkt b kommen?");
            cardSystem.GetComponent<CardSystem>().CreateRootNode("main");
            List<string> wurzelList = new List<string> { "a", "b", "c" };
            cardSystem.GetComponent<CardSystem>().CreateChildrenNodes(null, 3, wurzelList, 3);
            List<string> knotenlList = new List<string> { "1", "2", "3" };
            List<string> knotenlList2 = new List<string> { "4", "5", "6" };
            cardSystem.GetComponent<CardSystem>().CreateChildrenNodes(cardSystem.GetComponent<CardSystem>().GetNodeByName(null, "c"), knotenlList.Count, knotenlList, 6);
            cardSystem.GetComponent<CardSystem>().CreateChildrenNodes(cardSystem.GetComponent<CardSystem>().GetNodeByName(null, "b"), knotenlList2.Count, knotenlList2, 6);
            cardSystem.GetComponent<CardSystem>().ReplaceCard("ggez", null);
        }
    }
    public async Task ModeWhiteBoardSetupModel()
    {
        string filePath = Path.Combine(Application.dataPath, "Resources/questions_and_answers_whiteboard.json");
        string json = File.ReadAllText(filePath);
        QuestionsAndAnswers data = JsonConvert.DeserializeObject<QuestionsAndAnswers>(json);
        chat.AppendSystemMessage("Du bist ein Brainstorming-Assistent. Wenn Benutzer dir einen Begriff oder eine Frage geben, antworte bitte mit einem HeadLine und einem text. Der HeadLine sollte ein wichtiges Wort oder einen Begriff darstellen, und der Text sollte eine kurze Erklärung dazu bieten. Antworte ausschließlich mit Haupttitel & Untertitel, und füge keine weiteren Informationen hinzu. Du kannst auch mehrere Antworte geben als Array d.h. mehrere Haupt und Untertiteln. ich will auch dass du am ende sagst wie viele Antworte du mir gegeben hast. Antwort bitte wie ein JSON respond. Mindstends 1 Antworte und Max 3 Antworte. Deine Antworte müssen Volständig generiert sein sonst klappt die Antwort nicht.");
        foreach (var questionData in data.questions)
        {
            string question = questionData.question;
            chat.AppendUserInput(question);
            string answersJson = JsonConvert.SerializeObject(new { answers = questionData.answers });
            chat.AppendExampleChatbotOutput(answersJson);
        }
    }
    public async Task ModeWhiteBoardSendMessage(string str)
    {
        chat.AppendUserInput(str);
        string response = await chat.GetResponseFromChatbotAsync();
        Debug.Log(response);
        ModeWhiteBoardTranslator(response);
    }
    void ModeWhiteBoardTranslator(string str) { // translate answer from OPENAI to Cards on Board<<<<<<<
        OpenAIResopnse openAIResponse = JsonConvert.DeserializeObject<OpenAIResopnse>(str);
        List<Card> kartenList = openAIResponse.Answers;
        foreach (Card card in kartenList)
        {
            cardSystem.GetComponent<CardSystem>().initCardObj(card.HeadLine, card.Text);
        }
      
    }
    async public Task ModeWhiteBoardExtend(string strQues, string strAnswer) // 
    {
        // Question was... ur answer was... --> now give me more...
        string str = "Die Frage wurde dir gestellt: " + strQues + ". und du hast so beantwortert: " + strAnswer + ". kannst du noch zu der frage andere antworte geben?";
        chat.AppendUserInput(str);
        string response = await chat.GetResponseFromChatbotAsync();
        ModeWhiteBoardTranslator(response);
    }

    public async Task ModeMindMapSetupModel()
    {
        string filePath = Path.Combine(Application.dataPath, "Resources/questions_and_answers_mindmap.json");
        string json = File.ReadAllText(filePath);
        QuestionsAndAnswersMindMap data = JsonConvert.DeserializeObject<QuestionsAndAnswersMindMap>(json);
        chat.AppendSystemMessage("Du bist ein Brainstorming-Assistent für MindMaps. Hauptwurzel ist ein wort oder begriff aus der frage; Unterwurzeln sind die main begriffe und knoten beinhalten in weenigen worten mehr über unterwurzeln. Benutze so wenige worte wie möglich. Antwort bitte wie ein JSON respond.");
        foreach (var questionData in data.questions)
        {
            string question = questionData.question;
            chat.AppendUserInput(question);
            string answersJson = JsonConvert.SerializeObject(new { answers = questionData.answers });
            chat.AppendExampleChatbotOutput(answersJson);
        }
    }

    public async Task ModeMindMapSendMessage(string str)
    {
        chat.AppendUserInput(str);
        string response = await chat.GetResponseFromChatbotAsync();
        Debug.Log(response);
        ModeMindMapTranslator(response);

    }

    public async Task ModeMindMapTranslator(string str)
    {
        OpenAIResopnseMindMap openAIResponse = JsonConvert.DeserializeObject<OpenAIResopnseMindMap>(str);
        List<CardMindMap> kartenList = openAIResponse.Answers;
        List<string> wurzelList=new List<string>();
        List<string> knotenList = new List<string>();
        int count = 0;
        Dictionary<string, List<string>> knotenMap = new Dictionary<string, List<string>>();
        foreach (CardMindMap card in kartenList)
        {
            cardSystem.GetComponent<CardSystem>().CreateRootNode(card.Hauptwurzel);
            foreach (AntwortKnoten undercard in card.Unterwurzeln)
            {
                wurzelList.Add(undercard.Wurzel);
                foreach(string strn in undercard.Knoten)
                {
                   
                    knotenList.Add(strn);
                    count++;
                }
                knotenMap.Add(undercard.Wurzel, knotenList);
                knotenList = new List<string>();
            }
        }
        cardSystem.GetComponent<CardSystem>().CreateChildrenNodes(null, wurzelList.Count, wurzelList, wurzelList.Count);
    
        foreach (var key in knotenMap.Keys)
        {
            Debug.Log(key);
            knotenMap[key].ForEach(x => Debug.Log(x));
            cardSystem.GetComponent<CardSystem>().CreateChildrenNodes(cardSystem.GetComponent<CardSystem>().GetNodeByName(null,key), knotenMap[key].Count, knotenMap[key],count);
        }
   
        //map with main and under list


    }
    public async Task ModeMindMapExtend(string strQues, string strAnswer)
    {

    }

    public async Task ModeMindMapReplaceAI(string strQues, GameObject gmoRep)
    {
        // Question was... ur answer was... --> now give me more...
        string str = "Bei der vorherigen frage habe ich jetzt den punkt: " + strQues +". kannst du noch zu der frage 3 alternative punkte geben?";
        chat.AppendUserInput(str);
        string response = await chat.GetResponseFromChatbotAsync();
        Debug.Log(response);
        cardSystem.GetComponent<CardSystem>().ReplaceCard("ggez", gmoRep);
    }
}

class Card {
    public string HeadLine { get; set; }
    public string Text { get; set; }
}
class CardMindMap
{
    public string Hauptwurzel { get; set; }
    public List<AntwortKnoten> Unterwurzeln { get; set; }
}
class OpenAIResopnse
{
    public List<Card> Answers { get; set; }
}
class OpenAIResopnseMindMap
{
    public List<CardMindMap> Answers { get; set; }
}
public class QuestionAndAnswers
{
    public string question;
    public List<Answer> answers;
}

public class Answer
{
    public string HeadLine;
    public string Text;
}
public class QuestionsAndAnswers
{
    public List<QuestionAndAnswers> questions;
}

public class QuestionAndAnswersMindMap
{
    public string question;
    public List<AnswerMindMap> answers;
}
public class AnswerMindMap
{
    public string Hauptwurzel;
    public List<AntwortKnoten> Unterwurzeln;
}
public class AntwortKnoten
{
    public string Wurzel;
    public List<string> Knoten;
}
public class QuestionsAndAnswersMindMap
{
    public List<QuestionAndAnswersMindMap> questions;
}