using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystem : MonoBehaviour
{
    public GameObject uiGameObject;
    public GameObject aiSystem;
    public GameObject mainSystem;
    public GameObject cardSystem;
    public GameObject spawnSystem;
    public GameObject recordSystem;

    private bool uiBool = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void HideUI()
    {
        uiGameObject.SetActive(false);
    }

    private void ShowUI()
    {
        uiGameObject.SetActive(true);
    }

    public void ModeWhiteBoard()
    {
        aiSystem.GetComponent<OpenAIController>().ModeWhiteBoardSetupModel();
        mainSystem.GetComponent<MainSystem>().ChangeMode(0);
        cardSystem.GetComponent<CardSystem>().DestroyAll();
        spawnSystem.GetComponent<SpawnSystem>().DestoryAll();
        recordSystem.GetComponent<RecordSystem>().StartRecording();
        HideUI();
    }
    public void ModeMindMap()
    {
        aiSystem.GetComponent<OpenAIController>().ModeMindMapSetupModel();
        mainSystem.GetComponent<MainSystem>().ChangeMode(1);
        cardSystem.GetComponent<CardSystem>().DestroyAll();
        spawnSystem.GetComponent<SpawnSystem>().DestoryAll();
        recordSystem.GetComponent<RecordSystem>().StartRecording();
        HideUI();
    }
    public void ToggleUI()
    {
        uiBool = !uiBool;
        uiGameObject.SetActive(uiBool);
    }
}
