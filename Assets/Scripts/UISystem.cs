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
        aiSystem.GetComponent<OpenAIController>().ModeWhiteBoardSetupModel().Wait();
        mainSystem.GetComponent<MainSystem>().ChangeMode(0);
        ClearStartUp();
    }
    public void ModeMindMap()
    {
        aiSystem.GetComponent<OpenAIController>().ModeMindMapSetupModel().Wait();
        mainSystem.GetComponent<MainSystem>().ChangeMode(1);
        ClearStartUp();
    }
    public void ToggleUI()
    {
        uiBool = !uiBool;
        uiGameObject.SetActive(uiBool);
    }

    private void ClearStartUp()
    {
        cardSystem.GetComponent<CardSystem>().DestroyAll();
        spawnSystem.GetComponent<SpawnSystem>().DestoryAll();
        recordSystem.GetComponent<RecordSystem>().StartRecording();
        HideUI();
    }
}
