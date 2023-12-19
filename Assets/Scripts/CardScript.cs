using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    public TextMeshPro mainTitleTxt;
    public TextMeshPro subTitleTxt;

    private GameObject locationHolding;
    private GameObject card;
    public void ChangeMainTxt(string txt)
    {
        mainTitleTxt.text = txt;
    }
    public void ChangeSubTxt(string txt)
    {
        subTitleTxt.text = txt;
    }
    public void SetLocationHolding(GameObject tmp)
    {
        locationHolding = tmp;
    }
    public GameObject GetLocationHolding()
    {
        return locationHolding;
    }
    public void SetCard(GameObject cardObj)
    {
        card = cardObj;
    }
    private void DrawLineToCard(GameObject card)
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.widthMultiplier = 0.01f;
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
        }
        Vector3[] positions = { card.transform.position, this.transform.position };
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }
    private void Update()
    {
        if (card != null)
        {
            DrawLineToCard(card);
        }

    }
}
