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
using UnityEditor.Experimental.GraphView;

public class OpenAIController : MonoBehaviour
{
    public GameObject cardSystem;
    public GameObject boardsSystem;
    public GameObject spawnSystem;
    public GameObject audioSystem;
    public GameObject vibrationSystem;

    private OpenAI_API.OpenAIAPI api = new OpenAI_API.OpenAIAPI(JObject.Parse(File.ReadAllText(Path.Combine(Application.dataPath, "Resources/config.json")))["openai"]["api"].ToString());
    private Conversation chat;
    private string mindMapRespondTmp;

    void Start()
    {
        chat = api.Chat.CreateConversation();
        chat.Model = JObject.Parse(File.ReadAllText(Path.Combine(Application.dataPath, "Resources/config.json")))["openai"]["model_chat"].ToString();
    }

    public Task ModeWhiteBoardSetupModel()
    {
        string filePath = Path.Combine(Application.dataPath, "Resources/questions_and_answers_whiteboard.json");
        string json = File.ReadAllText(filePath);
        QuestionsAndAnswers data = JsonConvert.DeserializeObject<QuestionsAndAnswers>(json);
        chat.AppendSystemMessage("Du bist ein Brainstorming-Assistent. Wenn Benutzer dir einen Begriff oder eine Frage geben, antworte bitte mit einem HeadLine und einem text. Der HeadLine sollte ein wichtiges Wort oder einen Begriff darstellen, und der Text sollte eine kurze Erklärung dazu bieten. Antworte ausschließlich mit Haupttitel & Untertitel, und füge keine weiteren Informationen hinzu. Du kannst auch mehrere Antworte geben als Array d.h. mehrere Haupt und Untertiteln. ich will auch dass du am ende sagst wie viele Antworte du mir gegeben hast. Antwort bitte wie ein JSON respond. Mindstends 1 Antworte und Max 3 Antworte. Deine Antworte müssen Volständig generiert sein sonst klappt die Antwort nicht.");
        foreach (var questionData in data.questions)
        {
            string question = questionData.question;
            chat.AppendUserInput(question);
            string answersJson = JsonConvert.SerializeObject(new { questionData.answers });
            chat.AppendExampleChatbotOutput(answersJson);
        }

        return Task.CompletedTask;
    }

    public async Task ModeWhiteBoardSendMessage(string str)
    {
        chat.AppendUserInput(str);
        string response = await chat.GetResponseFromChatbotAsync();
        audioSystem.GetComponent<AudioSystem>().PlaySecondaryClickAudio();
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

        vibrationSystem.GetComponent<VibrationSystem>().HapticLeft();
        vibrationSystem.GetComponent<VibrationSystem>().HapticRight();

    }
    async public Task ModeWhiteBoardExtend(string strQues, string strAnswer)  
    {
        // Question was... ur answer was... --> now give me more...
        string str = "Die Frage wurde dir gestellt: " + strQues + ". und du hast so beantwortert: " + strAnswer + ". kannst du noch zu der frage andere antworte geben?";
        chat.AppendUserInput(str);
        string response = await chat.GetResponseFromChatbotAsync();
        audioSystem.GetComponent<AudioSystem>().PlaySecondaryClickAudio();
        ModeWhiteBoardTranslator(response);
    }

    public Task ModeMindMapSetupModel()
    {
        string filePath = Path.Combine(Application.dataPath, "Resources/questions_and_answers_mindmap.json");
        string json = File.ReadAllText(filePath);
        QuestionsContainer questionsContainer = JsonUtility.FromJson<QuestionsContainer>(json);
        chat.AppendSystemMessage("Du bist ein Brainstorming-Assistent für MindMaps. Erste Wurzel soll ein wort oder begriff aus der frage beinhalten; Knoten sind die main begriffe und knoten beinhalten in wenigen worten mehr über Würzeln. Bei Knoten gibts wiederrum wurzeln die knoten haben usw usw. Benutze so wenige worte wie möglich. Antwort bitte wie ein JSON respond.");
        foreach (var questionData in questionsContainer.questions)
        {
            string question = questionData.question;
 
            chat.AppendUserInput(question);
            string answersJson = JsonConvert.SerializeObject(new { questionData.answers });
          
            chat.AppendExampleChatbotOutput(answersJson);
        }
        Debug.Log("Done Training");
        return Task.CompletedTask;
    }

    public async Task ModeMindMapSendMessage(string str)
    {
        chat.AppendUserInput(str);
        string response = await chat.GetResponseFromChatbotAsync();
        audioSystem.GetComponent<AudioSystem>().PlaySecondaryClickAudio();
        Debug.Log(response);
        mindMapRespondTmp = response;
        ModeMindMapTranslator(response);

    }

    public void SetCurrentJson(NodeStorage rootNode)
    {
        AnswersContainer reversedContainer = ConvertNodeToAnswersContainer(rootNode);
        mindMapRespondTmp = JsonUtility.ToJson(reversedContainer, true);
    }
    public async Task ModeMindMapTranslator(string str)
    {  
        cardSystem.GetComponent<CardSystem>().DestroyAll();
        AnswersContainer answersContainer = JsonUtility.FromJson<AnswersContainer>(str);
        foreach (var answer in answersContainer.answers)
        {
            NodeStorage rootNode = CreateNodeFromAnswerNode(answer, null);
            cardSystem.GetComponent<CardSystem>().InitMindMap(rootNode);
        }
        vibrationSystem.GetComponent<VibrationSystem>().HapticLeft();
        vibrationSystem.GetComponent<VibrationSystem>().HapticRight();
    }
    public async Task ModeMindMapReplaceTranslator(string str)
    {
        MyData myData = JsonUtility.FromJson<MyData>(str);
        List<string> list = new List<string>();
        foreach (string point in myData.alternativePunkte)
        {
            Debug.Log(point);
            list.Add(point);
        }
        cardSystem.GetComponent<CardSystem>().CreateReplaceAICards(list);
        vibrationSystem.GetComponent<VibrationSystem>().HapticLeft();
    }
    public async Task ModeMindMapExtend(string strQues)
    {
        // Question was... ur answer was... --> now give me more...
        string str = "Kannst du bei der Vorherigen antwort von dir: " + mindMapRespondTmp + " ein paar punkte unter dem Punkt " + strQues + " sameln. also 2 oder 3 punkte dadrunter tun nix anderes? Ergänze deine antwort also";
        chat.AppendUserInput(str);
        string response = await chat.GetResponseFromChatbotAsync();
        audioSystem.GetComponent<AudioSystem>().PlaySecondaryClickAudio();
        Debug.Log(response);
        ModeMindMapTranslator(response);
    }

    public async Task ModeMindMapReplaceAI(string strQues)
    {
        // Question was... ur answer was... --> now give me more...
        string str = "Bei der vorherigen frage: "+ mindMapRespondTmp + ". habe ich jetzt den punkt: " + strQues + ". kannst du noch zu der frage exakt nur 3 alternative punkte geben nur und nix anderes? Die antwort soll folgendes aussehen: JSON format mit array namens alternativePunkte und dadrunter die 3 punkte";
        chat.AppendUserInput(str);
        string response = await chat.GetResponseFromChatbotAsync();
        audioSystem.GetComponent<AudioSystem>().PlaySecondaryClickAudio();
        Debug.Log(response);
        ModeMindMapReplaceTranslator(response);
    }
    public async Task ModeMindMapAutoSort(string strQues)
    {
        // Question was... ur answer was... --> now give me more...
        string str = "Bei der vorherigen frage: " + mindMapRespondTmp + ". habe ich jetzt den punkt: " + strQues + ". kannst du diesen punkt zuordnen? Die antwort soll folgendes aussehen: JSON format genau wie vorherige frage aber mit dem punkt drin zugeordnet";
        chat.AppendUserInput(str);
        string response = await chat.GetResponseFromChatbotAsync();
        audioSystem.GetComponent<AudioSystem>().PlaySecondaryClickAudio();
        Debug.Log(response);
        ModeMindMapTranslator(response);
    }
    private NodeStorage CreateNodeFromAnswerNode(AnswerNode answerNode, NodeStorage parentNode)
    {
        NodeStorage newNode = new NodeStorage
        {
            name = answerNode.Wurzel,
            parentNode = parentNode
        };

        if (answerNode.Knoten != null)
        {
            foreach (var subNode in answerNode.Knoten)
            {
                NodeStorage childNode = CreateNodeFromAnswerNode(subNode, newNode);
                newNode.children.Add(childNode);
            }
        }

        return newNode;
    }
    private AnswersContainer ConvertNodeToAnswersContainer(NodeStorage rootNode)
    {
        AnswersContainer reversedContainer = new AnswersContainer
        {
            answers = new List<AnswerNode>
            {
                new AnswerNode
                {
                    Wurzel = rootNode.name,
                    Knoten = ConvertNodeToAnswerList(rootNode.children)
                }
            }
        };

        return reversedContainer;
    }

    private List<AnswerNode> ConvertNodeToAnswerList(List<NodeStorage> nodes)
    {
        List<AnswerNode> answerNodes = new List<AnswerNode>();

        foreach (var node in nodes)
        {
            AnswerNode answerNode = new AnswerNode
            {
                Wurzel = node.name,
                Knoten = ConvertNodeToAnswerList(node.children)
            };

            answerNodes.Add(answerNode);
        }

        return answerNodes;
    }
}

class Card {
    public string HeadLine { get; set; }
    public string Text { get; set; }
}

class OpenAIResopnse
{
    public List<Card> Answers { get; set; }
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

public class MyData
{
    public string[] alternativePunkte;
}



///NEU
[System.Serializable]
public class AnswerNode
{
    public string Wurzel;
    public List<AnswerNode> Knoten;
}

[System.Serializable]
public class AnswerClass
{
    public string question;
    public List<AnswerNode> answers;
}

[System.Serializable]
public class QuestionsContainer
{
    public List<AnswerClass> questions;
}

[System.Serializable]
public class AnswersContainer
{
    public List<AnswerNode> answers;
}
