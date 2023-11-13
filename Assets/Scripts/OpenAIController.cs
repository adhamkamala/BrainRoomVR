using OpenAI_API.Chat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using OpenAI;
public class OpenAIController : MonoBehaviour
{
    private readonly string fileName = "output.wav";
    private AudioClip clip;
    private OpenAI_API.OpenAIAPI api = new OpenAI_API.OpenAIAPI("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");
    private Conversation chat;
    private OpenAIApi openai = new OpenAIApi("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");
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

    async Task SetupModel()
    {
        chat.AppendSystemMessage("Du bist ein Brainstorming-Assistent. Wenn Benutzer dir einen Begriff oder eine Frage geben, antworte bitte mit einem Haupttitel und einem Subtitel. Der Haupttitel sollte ein wichtiges Wort oder einen Begriff darstellen, und der Subtitel sollte eine kurze Erklärung dazu bieten. Antworte ausschließlich mit Haupttitel & Subtitel, und füge keine weiteren Informationen hinzu.");
        chat.AppendUserInput("Was ist ICE");
        chat.AppendExampleChatbotOutput("MainTitel: Transport Subtitel: Ist ein Zug in Deutschland");
    }

    async Task SendMessage(string str)
    {
        chat.AppendUserInput(str);
        string response = await chat.GetResponseFromChatbotAsync();
        Debug.Log(response);

    }

    async Task tsk1Async()
    {
        chat.AppendUserInput("as weißt du Über Eyes Wide Shut?");
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
           // Debug.Log("Available Microphone: " + Microphone.devices[i]);
            if (Microphone.devices[i].Contains("Oculus")) {
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
            await SendMessage(res.Text);
        }
}
