using OpenAI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using OpenAI;
using System.Security.Cryptography;

public class RecordSystem : MonoBehaviour
{
    public GameObject spawnLocationRecordPanel;
    public GameObject recordPanelPrefab;
    public GameObject boardsSystem;
    public GameObject spawnSystem;
    public GameObject aiSystem;
    private GameObject tmp;
    private readonly string fileName = "output.wav";
    private AudioClip clip;
    private OpenAIApi openai = new OpenAIApi("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");
    private string convertedAudio;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnRecordPanel() {
    
       tmp = Instantiate(recordPanelPrefab);
       recordPanelPrefab.transform.parent = spawnLocationRecordPanel.transform;
    }

    public void DestroyRecordPanel()
    {
        Destroy(tmp);
    }

    public void OnConfirmClick()
    {
        spawnSystem.GetComponent<SpawnSystem>().SpawnBoard();
        boardsSystem.GetComponent<BoardsSystem>().GetSelectedBoard().GetComponent<BoardScript>().ChangeTopicTxt(convertedAudio);
        aiSystem.GetComponent<OpenAIController>().SendMessage(convertedAudio);
    }

    public void OnCancelClick()
    {
        DestroyRecordPanel();
    }

    public void StartRecording()
    {
        SpawnRecordPanel();
        string micName = "";
       // ChangeHintTxt("Recording...");
        tmp.GetComponent<RecordPanelScript>().ChangeHintTxt("Recording...");
        tmp.GetComponent<RecordPanelScript>().ChangeMainTxt("");
        //ChangeMainTxt("");
        for (int i = 0; i < Microphone.devices.Length; i++)
        {
            Debug.Log("Available Microphone: " + Microphone.devices[i]);
            if (Microphone.devices[i].Contains("Realtek"))
            { // Oculus
                Debug.Log("Found VR: " + Microphone.devices[i]);
                micName = Microphone.devices[i];
            }

        }
        Debug.Log("Talk...");
        clip = Microphone.Start(micName, false, 10, 44100); // sekunden
    }

    public async void EndRecording()
    {
       // ChangeHintTxt("Recorded");
        tmp.GetComponent<RecordPanelScript>().ChangeHintTxt("Recorded");
        Microphone.End(null);
        byte[] data = SaveWav.Save(fileName, clip);
        var req = new CreateAudioTranscriptionsRequest
        {
            FileData = new FileData() { Data = data, Name = "audio.wav" },
            Model = "whisper-1",
            Language = "de"
        };
        var res = await openai.CreateAudioTranscription(req);
        Debug.Log(res.Text);
       // ChangeMainTxt(res.Text);
        tmp.GetComponent<RecordPanelScript>().ChangeMainTxt(res.Text);
        convertedAudio = res.Text;
    }



}
