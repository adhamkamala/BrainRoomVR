using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystem : MonoBehaviour
{
    public GameObject uiGameObject;
    public GameObject uiSystem;
    public GameObject mainSystem;
    public GameObject cardSystem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void HideUI()
    {
        uiGameObject.SetActive(false);
    }

    void ShowUI()
    {
        uiGameObject.SetActive(true);
    }

    public void ModeWhiteBoard()
    {
        uiSystem.GetComponent<OpenAIController>().ModeWhiteBoardSetupModel();
        mainSystem.GetComponent<MainSystem>().ChangeMode(0);
        HideUI();
    }
    public void ModeMindMap()
    {
        uiSystem.GetComponent<OpenAIController>().ModeMindMapSetupModel();
        mainSystem.GetComponent<MainSystem>().ChangeMode(1);
        HideUI();
        cardSystem.GetComponent<CardSystem>().DestroyAll();
    }
}
