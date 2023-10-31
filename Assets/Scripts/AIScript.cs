using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AIScript : MonoBehaviour
{
    private string api = "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";
    private string gptApiUrl = "https://api.openai.com/v1/chat/completions"; // Check OpenAI API documentation for the correct URL

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetChatResponse("Hello, how can I help you?"));
    }

    IEnumerator GetChatResponse(string inputText)
    {
        string jsonInput = "{\"messages\": [{\"role\": \"system\", \"content\": \"You are a helpful assistant.\"}, {\"role\": \"user\", \"content\": \"" + inputText + "\"}]}";

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(gptApiUrl, jsonInput))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", "Bearer " + api);

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                // Parse the response and use it in your Unity application
                string jsonResponse = www.downloadHandler.text;
                Debug.Log("Response: " + jsonResponse);

                // TODO: Implement logic to handle and display the GPT response in your Unity project
            }
        }
    }
}
