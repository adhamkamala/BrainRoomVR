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

    private GameObject tmp = null;
    private readonly string fileName = "output.wav";
    private AudioClip clip;
    private OpenAIApi openai = new OpenAIApi("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");
    private string convertedAudio;
    public void SpawnRecordPanel() {

        if (tmp != null) { 
        DestroyRecordPanel();
        }
       tmp = Instantiate(recordPanelPrefab);
       tmp.transform.parent = spawnLocationRecordPanel.transform;
        tmp.transform.localPosition = Vector3.zero;
        tmp.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

    }

    public void DestroyRecordPanel()
    {
        Destroy(tmp);
        tmp = null;
    }

    public void OnConfirmClick()
    {
        spawnSystem.GetComponent<SpawnSystem>().SpawnBoard();
        boardsSystem.GetComponent<BoardsSystem>().GetSelectedBoard().GetComponent<BoardScript>().ChangeTopicTxt(convertedAudio);
        aiSystem.GetComponent<OpenAIController>().SendMessage(convertedAudio);
        DestroyRecordPanel();
    }

    public void OnCancelClick()
    {
        DestroyRecordPanel();
    }

    public void StartRecording()
    {
        SpawnRecordPanel();
        string micName = "";
        tmp.GetComponent<RecordPanelScript>().ChangeHintTxt("Recording...");
        tmp.GetComponent<RecordPanelScript>().ChangeMainTxt("");
        for (int i = 0; i < Microphone.devices.Length; i++)
        {
            if (Microphone.devices[i].Contains("Oculus")) // Oculus
            { 
                micName = Microphone.devices[i];
            }

        }
        clip = Microphone.Start(micName, false, 10, 44100); // sekunden
    }

    public async void EndRecording()
    {
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
        tmp.GetComponent<RecordPanelScript>().ChangeMainTxt(res.Text);
        convertedAudio = res.Text;
    }



}