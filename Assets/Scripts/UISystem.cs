using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystem : MonoBehaviour
{
    public GameObject uiGameObject;
    public GameObject uiSystem; 
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

    void ModeWhiteBoard()
    {
        uiSystem.GetComponent<OpenAIController>().ModeWhiteBoardSetupModel();
    }
}
