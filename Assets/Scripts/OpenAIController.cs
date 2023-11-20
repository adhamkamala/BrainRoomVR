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
    private readonly string fileName = "output.wav";
    private AudioClip clip;
    private OpenAI_API.OpenAIAPI api = new OpenAI_API.OpenAIAPI("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");
    private Conversation chat;
    private OpenAIApi openai = new OpenAIApi("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");
    public GameObject cardSystem;
    public GameObject board;

    // Start is called before the first frame update
    void Start()
    {
       chat = api.Chat.CreateConversation();
       SetupModel();
    }

    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            Debug.Log("pressed");
            tsk1Async();
        }
    }

    public void testTrans()
    {
        string serverResponse = "{ \"answers\": [ { \"MainTitel\": \"Datum\", \"Untertitel\": \"22. November 1963\" },{ \"MainTitel\": \"Ort\", \"Untertitel\": \"Dealey Plaza, Dallas, Texas, USA\" },{ \"MainTitel\": \"Opfer\", \"Untertitel\": \"Pr�sident John F. Kennedy\" }] }";
        Debug.Log(serverResponse);

    }

    async Task SetupModel()
    {
        string filePath = Path.Combine(Application.dataPath, "Resources/questions_and_answers.json");
        string json = File.ReadAllText(filePath);
        QuestionsAndAnswers data = JsonConvert.DeserializeObject<QuestionsAndAnswers>(json);
        chat.AppendSystemMessage("Du bist ein Brainstorming-Assistent. Wenn Benutzer dir einen Begriff oder eine Frage geben, antworte bitte mit einem HeadLine und einem text. Der HeadLine sollte ein wichtiges Wort oder einen Begriff darstellen, und der Text sollte eine kurze Erkl�rung dazu bieten. Antworte ausschlie�lich mit Haupttitel & Untertitel, und f�ge keine weiteren Informationen hinzu. Du kannst auch mehrere Antworte geben als Array d.h. mehrere Haupt und Untertiteln. ich will auch dass du am ende sagst wie viele Antworte du mir gegeben hast. Antwort bitte wie ein JSON respond. Mindstends 1 Antworte und Max 3 Antworte. Deine Antworte m�ssen Volst�ndig generiert sein sonst klappt die Antwort nicht.");
        foreach (var questionData in data.questions)
        {
            string question = questionData.question;
            chat.AppendUserInput(question);
            string answersJson = JsonConvert.SerializeObject(new { answers = questionData.answers });
            chat.AppendExampleChatbotOutput(answersJson);
        }
        // chat.AppendUserInput("Was ist ICE?");
        //chat.AppendExampleChatbotOutput("[{MainTitel: Transport ; Untertitel: Ist ein Zug in Deutschland}] Anzahl Antworte: 1");
        // chat.AppendExampleChatbotOutput("{ \"answers\": [ { \"MainTitel\": \"Transport\", \"Untertitel\": \"Ist ein Zug in Deutschland\" }] }");
        // chat.AppendUserInput("Was wei�t du �ber den Attentat auf JFK?");
        // chat.AppendExampleChatbotOutput("{ \"answers\": [ { \"MainTitel\": \"Datum\", \"Untertitel\": \"22. November 1963\" },{ \"MainTitel\": \"Ort\", \"Untertitel\": \"Dealey Plaza, Dallas, Texas, USA\" },{ \"MainTitel\": \"Opfer\", \"Untertitel\": \"Pr�sident John F. Kennedy\" }] }");
    }

    async Task SendMessage(string str)
    {
        chat.AppendUserInput(str);
        string response = await chat.GetResponseFromChatbotAsync();
        Debug.Log(response);
        OpenAIToBoardTranslator(response);
    }

    void OpenAIToBoardTranslator(string str) { // translate answer from OPENAI to Cards on Board<<<<<<<
        //Debug.Log(str);
        OpenAIResopnse openAIResponse = JsonConvert.DeserializeObject<OpenAIResopnse>(str);
        List<Card> kartenList = openAIResponse.Answers;
        foreach (Card card in kartenList)
        {
            cardSystem.GetComponent<CardSystem>().initCardObj(card.HeadLine, card.Text);
        }
      
    }

    async public Task SendMessageMore(string strQues, string strAnswer) // 
    {
        // Question was... ur answer was... --> now give me more...
        string str = "Die Frage wurde dir gestellt: " + strQues + ". und du hast so beantwortert: " + strAnswer + ". kannst du noch zu der frage andere antworte geben?";
        chat.AppendUserInput(str);
        string response = await chat.GetResponseFromChatbotAsync();
        Debug.Log(response);
        OpenAIToBoardTranslator(response);
    }


    async Task tsk1Async()
    {
        chat.AppendUserInput("as wei�t du �ber Eyes Wide Shut?");
        // and get the response
        string response = await chat.GetResponseFromChatbotAsync();
        Console.WriteLine(response); // "Yes"
        //debug.log(response);
        //debug.log("3");
        //// and continue the conversation by asking another
        //chat.appenduserinput("is this an animal? chair");
        //// and get another response
        //response = await chat.getresponsefromchatbotasync();
        //console.writeline(response); // "no"
        //debug.log(response);
        //debug.log("4");
        //// the entire chat history is available in chat.messages
        foreach (OpenAI_API.Chat.ChatMessage msg in chat.Messages)
        {
            Console.WriteLine($"{msg.Role}: {msg.Content}");
            Debug.Log($"{msg.Role}: {msg.Content}");
        }
    }

        public void StartRecording()
        {
        string micName ="";

        for (int i = 0; i < Microphone.devices.Length; i++) {
            Debug.Log("Available Microphone: " + Microphone.devices[i]);
            if (Microphone.devices[i].Contains("Realtek")) { // Oculus
                Debug.Log("Found VR: " + Microphone.devices[i]);
                micName = Microphone.devices[i];
            }

        }
        Debug.Log("Talk...");
        clip = Microphone.Start(micName, false, 5, 44100); // sekunden
        }

            public async void EndRecording()
        {
            
            Microphone.End(null);
            byte[] data = SaveWav.Save(fileName, clip);
            var req = new CreateAudioTranscriptionsRequest
            {
                FileData = new FileData() {Data = data, Name = "audio.wav"},
                Model = "whisper-1",
                Language = "de"
            };
            var res = await openai.CreateAudioTranscription(req);
            Debug.Log(res.Text);
            board.GetComponent<BoardScript>().ChangeTopicTxt(res.Text);
            await SendMessage(res.Text);
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