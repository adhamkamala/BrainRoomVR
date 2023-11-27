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
        _ = SetupModel();
    }

    async Task SetupModel()
    {
        string filePath = Path.Combine(Application.dataPath, "Resources/questions_and_answers.json");
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

    public async Task SendMessage(string str)
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
        OpenAIToBoardTranslator(response);
    }

        public void StartRecording()
        {
        string micName ="";

        for (int i = 0; i < Microphone.devices.Length; i++) {
            if (Microphone.devices[i].Contains("Oculus")) { // Oculus
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
            spawnSystem.GetComponent<SpawnSystem>().SpawnBoard();
            boardsSystem.GetComponent<BoardsSystem>().GetSelectedBoard().GetComponent<BoardScript>().ChangeTopicTxt(res.Text);
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