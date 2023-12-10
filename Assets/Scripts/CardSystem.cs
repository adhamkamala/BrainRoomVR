using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

public class CardSystem : MonoBehaviour
{
    public GameObject cardsPrf;
    public GameObject cardsMindMapPrf;
    public GameObject boardsSystem;
    public Transform spawnPoint;

    private Node lastNode;
    private Node rootNode;
    private float horzSpacing = 2f;
    private GameObject parentCard;
    private GameObject childCard;

    private void Start()
    {
 
    }
    public void initCardObj(string mainTxt ="Main", string subTxt = "Some Text written in small size font for description") {
        if (boardsSystem.GetComponent<BoardsSystem>().GetSelectedBoard().GetComponentInChildren<BoardSystem>().CanCreate())
        {
            GameObject cardTmp = Instantiate(cardsPrf);
            cardTmp.layer = LayerMask.NameToLayer("CardsLayer");
            cardTmp.GetComponent<CardScript>().ChangeMainTxt(mainTxt);
            cardTmp.GetComponent<CardScript>().ChangeSubTxt(subTxt);
            boardsSystem.GetComponent<BoardsSystem>().GetSelectedBoard().GetComponentInChildren<BoardSystem>().CardLocator(cardTmp);
        }
    }

    public GameObject initMindMapCardObj(string subTxt)
    {
        GameObject cardTmp = Instantiate(cardsMindMapPrf);
        cardTmp.layer = LayerMask.NameToLayer("CardsLayer");
        cardTmp.GetComponent<CardScript>().ChangeSubTxt(subTxt);
        return cardTmp;
    }

    public Node CreateRootNood(string str)
    {
        Node mainNode = CreateNode(spawnPoint.position,str);
        mainNode.parentNode = null;
        rootNode = mainNode;
        return rootNode;
    }
    public void CreateChildrenNodes(Node parentNode,int count, List<string> strs,int countSpace)
    {
        if (parentNode == null)
        {
            parentNode = lastNode;
        }
        CalculateHorizontalSpacing(countSpace);
        List<Vector3> positions = CalculateChildNodePositions(parentNode, count);
        for (int i = 0; i < positions.Count; i++)
        {
            Node child = CreateNode(positions[i], strs[i]);
            child.parentNode = parentNode;
            parentNode.children.Add(child);
            Debug.Log(child.gameObject);
            Debug.Log(child.parentNode.gameObject);
            child.gameObject.GetComponent<CardScript>().SetCard(child.parentNode.gameObject);


        }
    }
    public bool IsRootNodeCreated()
    {
        if (rootNode != null)
        {
            return true;
        }
        else return false;
    }
    public void CreateChildNode(Node parentNode, string str, int countSpace)
    {
        CalculateHorizontalSpacing(countSpace);
        List<Vector3> positions = CalculateChildNodePositions(parentNode, 1);
        for (int i = 0; i < positions.Count; i++)
        {
            Node child = CreateNode(positions[i], str);
            child.parentNode = parentNode;
            parentNode.children.Add(child);
            Debug.Log(child.gameObject);
            Debug.Log(child.parentNode.gameObject);
            child.gameObject.GetComponent<CardScript>().SetCard(child.parentNode.gameObject);


        }
    }

    public void UserAttachCardNode(string str, GameObject card)
    {
        CreateChildNode(GetNodeByName(null,str), card.GetComponent<CardScript>().subTitleTxt.text,0);
    }
    List<Vector3> CalculateChildNodePositions(Node parentNode, int numberOfNodes)
    {
        List<Vector3> positions = new List<Vector3>();
        Debug.Log(horzSpacing);
        for (int i = 0; i < numberOfNodes; i++)
        {
            float yOffset = (i - (numberOfNodes - 1) * 0.5f) * horzSpacing;
            ;

            Vector3 childPosition = new Vector3(
               // parentNode.transform.position.x - 1f,
               // parentNode.transform.position.y  + yOffset,
                parentNode.transform.position.x + yOffset,
                parentNode.transform.position.y - 0.7f,   
                parentNode.transform.position.z);

            positions.Add(childPosition);
        }

        return positions;
    }
    Node CreateNode(Vector3 position, string subTxt = "Main")
    {
        GameObject nodeObject = Instantiate(cardsMindMapPrf, position, Quaternion.identity);
        nodeObject.transform.Rotate(0f, -90f, 0f);
        Node node = nodeObject.AddComponent<Node>();
        nodeObject.layer = LayerMask.NameToLayer("CardsLayer");
        nodeObject.GetComponent<CardScript>().ChangeSubTxt(subTxt);
        node.name= subTxt;
        lastNode = node;
        return node;
    }


    public Node GetNodeByName(Node currentNode,string name) {
        if (currentNode == null) { currentNode = rootNode; };
        if (currentNode.name == name)
        {
            return currentNode;
        }

        foreach (var childNode in currentNode.children)
        {
            Node result = GetNodeByName(childNode, name);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    void CalculateHorizontalSpacing(int numberOfNodes)
    {
        Debug.Log(numberOfNodes);
        horzSpacing = Mathf.Lerp(4f, 0.6f, Mathf.InverseLerp(1, 6, numberOfNodes));
        Debug.Log(horzSpacing);

    }
}

public class Node : MonoBehaviour
{
    string name;
 public Node parentNode; 
    public List<Node> children = new List<Node>();
}
