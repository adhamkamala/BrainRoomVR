using OpenAI;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading.Tasks;

public class RecordSystem : MonoBehaviour
{
    public GameObject spawnLocationRecordPanel;
    public GameObject recordPanelPrefab;
    public GameObject boardsSystem;
    public GameObject spawnSystem;
    public GameObject aiSystem;
    public GameObject mainSystem;
    public GameObject leftController;
    public GameObject cardSystem;
    public GameObject vibrationSystem;
    public GameObject rightControllerRay;

    private GameObject tmp = null;
    private GameObject gameObjectTmp;
    private AudioClip clip;
    private OpenAIApi openai = new OpenAIApi(JObject.Parse(File.ReadAllText(Path.Combine(Application.dataPath, "Resources/config.json")))["openai"]["api"].ToString());
    private readonly string fileName = "output.wav";
    private string convertedAudio;
    private bool replaceOption = false;

    public void SpawnRecordPanel() {
        vibrationSystem.GetComponent<VibrationSystem>().HapticRight();
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
        if (mainSystem.GetComponent <MainSystem>().WhatMode() ==0 && !replaceOption) {
            spawnSystem.GetComponent<SpawnSystem>().SpawnBoard();
            boardsSystem.GetComponent<BoardsSystem>().GetSelectedBoard().GetComponent<BoardScript>().ChangeTopicTxt(convertedAudio);
            aiSystem.GetComponent<OpenAIController>().ModeWhiteBoardSendMessage(convertedAudio);
            DestroyRecordPanel();
        } else if (mainSystem.GetComponent<MainSystem>().WhatMode() == 1 && !replaceOption) {
            if (cardSystem.GetComponent<CardSystem>().IsRootNodeCreated())
            {
                leftController.GetComponent<LeftController>().AttachCard(cardSystem.GetComponent<CardSystem>().CreateMindMapCardObj(convertedAudio));
            } else
            {
                aiSystem.GetComponent<OpenAIController>().ModeMindMapSendMessage(convertedAudio);
            }
            DestroyRecordPanel();
        } else if (replaceOption)
        {
            cardSystem.GetComponent<CardSystem>().SetCardToReplace(gameObjectTmp);
            cardSystem.GetComponent<CardSystem>().ReplaceCard(convertedAudio);
            replaceOption = false;
            gameObjectTmp = null;
            DestroyRecordPanel();
        }
       rightControllerRay.GetComponent<RightControllerRay>().StopBlinking();
    }
    public void OnCancelClick()
    {
        rightControllerRay.GetComponent<RightControllerRay>().StopBlinking();
        DestroyRecordPanel();
    }
    public void EnableReplace()
    {
        replaceOption = true;
    }
    public void SetGameObjectTmp(GameObject gmo)
    {
        gameObjectTmp=gmo;
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
            Model = JObject.Parse(File.ReadAllText(Path.Combine(Application.dataPath, "Resources/config.json")))["openai"]["model_voice"].ToString(),
            Language = JObject.Parse(File.ReadAllText(Path.Combine(Application.dataPath, "Resources/config.json")))["openai"]["model_voice_language"].ToString()
    };
        var res = await openai.CreateAudioTranscription(req);
        tmp.GetComponent<RecordPanelScript>().ChangeMainTxt(res.Text);
        convertedAudio = res.Text;
    }

}
