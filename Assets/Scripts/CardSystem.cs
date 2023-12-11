using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public Node CreateRootNode(string str)
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

    public void UserAttachCardNode(string str, GameObject card)
    {
        Node2 root2 = ConvertToNode2(rootNode);
        Node2 node2Tmp = new Node2();
        node2Tmp.name = card.GetComponent<CardScript>().subTitleTxt.text;
        node2Tmp.parentNode = GetNode2ByName(root2, str);
        node2Tmp.parentNode.children.Add(node2Tmp);
        rootNode.DeleteHierarchy();
        CreateRootNode(root2.name);
        ReconstructHierarchy(root2);
    }

    private void ReconstructHierarchy(Node2 currentNode, int depth = 0)
    {
    
        if (currentNode == null)
        {
            return;
        }


        int count = 0;
        if (currentNode.parentNode== null)
        {
            count = currentNode.children.Count;
        } else
        {
            foreach (Node2 n in currentNode.parentNode.children)
            {
                count = count + n.children.Count;
            }
        }
            List<string> childrenList = new List<string>();
            currentNode.children.ForEach(child => childrenList.Add(child.name));
            CreateChildrenNodes(GetNodeByName(null,currentNode.name), childrenList.Count, childrenList, count);
       
    

        string indentation = new string(' ', depth * 2);
        Debug.Log($"{indentation}Node: {currentNode.name}, Children Count: {currentNode.children.Count}");

        Debug.Log($"{indentation}Children: {string.Join(", ", currentNode.children.Select(child => child.name))}");

        // Recursively print information for each child
        foreach (var childNode in currentNode.children)
        {
            ReconstructHierarchy(childNode, depth + 1);
        }
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

    private Node2 ConvertToNode2(Node node)
    {
        if (node == null)
        {
            return null;
        }

        Node2 node2 = new Node2();
        node2.name = node.name;

        foreach (Node childNode in node.children)
        {
            Node2 childNode2 = ConvertToNode2(childNode);
            childNode2.parentNode = node2;
            node2.children.Add(childNode2);
        }

        return node2;
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
    public static Node2 GetNode2ByName(Node2 currentNode, string name)
    {
        if (currentNode.name == name)
        {
            return currentNode;
        }

        foreach (var childNode in currentNode.children)
        {
            Node2 result = GetNode2ByName(childNode, name);
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
    public void DeleteHierarchy()
    {
        // Delete children first
        foreach (var childNode in children)
        {
            childNode.DeleteHierarchy();
        }

        // Then delete the current node's GameObject
        Destroy(gameObject);
    }
}

public class Node2
{
    public string name;
    public Node2 parentNode;
    public List<Node2> children = new List<Node2>();
}
