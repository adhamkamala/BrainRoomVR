using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardSystem : MonoBehaviour
{
    public GameObject cardsPrf;
    public GameObject boardsSystem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.oKey.wasPressedThisFrame) {

            initCardObj();
        }
    }
   public void initCardObj(string mainTxt ="Main", string subTxt = "Some Text written in small size font for description") {
        if (boardsSystem.GetComponent<BoardsSystem>().GetSelectedBoard().GetComponentInChildren<BoardSystem>().CanCreate())
        {
            GameObject cardTmp = Instantiate(cardsPrf); // , new Vector3(0f, 0f, 0f), Quaternion.identity)
            cardTmp.layer = LayerMask.NameToLayer("CardsLayer");
            cardTmp.GetComponent<CardScript>().ChangeMainTxt(mainTxt);
            cardTmp.GetComponent<CardScript>().ChangeSubTxt(subTxt);
            boardsSystem.GetComponent<BoardsSystem>().GetSelectedBoard().GetComponentInChildren<BoardSystem>().CardLocator(cardTmp);
        }
    }
}
