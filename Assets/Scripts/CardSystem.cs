using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;


public class Node : MonoBehaviour
{
    public string nodeName;
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
public class NodeStorage
{
    public string name;
    public NodeStorage parentNode;
    public List<NodeStorage> children = new List<NodeStorage>();
}

public class CardSystem : MonoBehaviour
{
    public GameObject cardsPrf;
    public GameObject cardsMindMapPrf;
    public GameObject boardsSystem;
    public Transform spawnPoint;
    public GameObject leftController;
    public GameObject spawnLocationReplaceAI;
    public GameObject openAIController;

    private Node lastNode;
    private Node rootNode;
    private NodeStorage nodeTmp;
    private List<GameObject> cardsTmp;
    private GameObject cardToReplace;
    private float horzSpacing = 2f;

    public void CreateCardObj(string mainTxt ="Main", string subTxt = "Sub Text") {
        if (boardsSystem.GetComponent<BoardsSystem>().GetSelectedBoard().GetComponentInChildren<BoardSystem>().CanCreateBoard())
        {
            GameObject cardTmp = Instantiate(cardsPrf);
            cardTmp.layer = LayerMask.NameToLayer("CardsLayer");
            cardTmp.GetComponent<CardScript>().ChangeMainTxt(mainTxt);
            cardTmp.GetComponent<CardScript>().ChangeSubTxt(subTxt);
            cardTmp.name = "Whiteboard(NormalCard): " + subTxt;
            boardsSystem.GetComponent<BoardsSystem>().GetSelectedBoard().GetComponentInChildren<BoardSystem>().CardLocator(cardTmp);
        }
    }
    public void CreateMindMapCardObj(string subTxt, Node node=null)
    {
        GameObject cardTmp = Instantiate(cardsMindMapPrf);
        cardTmp.layer = LayerMask.NameToLayer("CardsLayer");
        cardTmp.GetComponent<CardScript>().ChangeSubTxt(subTxt);
        cardTmp.name= "MindMap(UserMadeCard): " + subTxt;
        if (node != null)
        {
            Node nodeCard = cardTmp.AddComponent<Node>();
            nodeCard.nodeName = node.nodeName;
            nodeCard.parentNode = node.parentNode;
            nodeCard.children= node.children;
        }
    }
    public void CreateReplaceAICards(List<string> strs)
    {
        // Vector3 startPoint = spawnLocationReplaceAI.transform.position;
        Vector3 startPoint = Vector3.zero;
        cardsTmp = new List<GameObject>();
        strs.ForEach(str =>
        {
            GameObject cardTmp = Instantiate(cardsMindMapPrf, startPoint, Quaternion.identity);
            cardTmp.name = "MindMap(ReplaceAICard): " + str;
            cardTmp.layer = LayerMask.NameToLayer("CardsLayer");
            cardTmp.GetComponent<CardScript>().ChangeSubTxt(str);

            cardTmp.transform.SetParent(spawnLocationReplaceAI.transform);
            cardTmp.transform.localPosition = Vector3.zero;
            cardTmp.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
            cardTmp.transform.localPosition = new Vector3(cardTmp.transform.localPosition.x, startPoint.y, cardTmp.transform.localPosition.z);
            startPoint.y -= 0.45f;
            cardsTmp.Add(cardTmp);
        });
    }
    public void CreateRootNode(string str)
    {
        rootNode = CreateNode(spawnPoint.position,str);
        rootNode.parentNode = null;
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
    public void CreateMindMap(NodeStorage node)
    {
        CreateRootNode(node.name);
        ReconstructHierarchy(node);

    }
    public bool IsRootNodeCreated()
    {
        if (rootNode != null)
        {
            return true;
        }
        else return false;
    }
    public void UserAttachCardNode(string str, GameObject card, bool reconstruct = false)
    {
        if (card.GetComponent<Node>()!=null && card.GetComponent<Node>().children.Count > 0) { reconstruct = true; }
        NodeStorage root2 = ConvertToNode2(rootNode);
        NodeStorage node2Tmp;
        if (reconstruct)
        {
            node2Tmp = nodeTmp;
        }
        else {
            node2Tmp = new NodeStorage();
        }
        node2Tmp.name = card.GetComponent<CardScript>().subTitleTxt.text;
        node2Tmp.parentNode = GetNodeStorageByName(root2, str);
        node2Tmp.parentNode.children.Add(node2Tmp);
        rootNode.DeleteHierarchy();
        CreateRootNode(root2.name);
        ReconstructHierarchy(root2);
    }
    public void NodeStorageToJson()
    {
        NodeStorage node = ConvertToNode2(rootNode);
        openAIController.GetComponent<OpenAIController>().SetCurrentJson(node);
    }
    public Node GetNodeByName(Node currentNode,string name) {
        if (currentNode == null) { currentNode = rootNode; };
        if (currentNode.nodeName == name)
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
    public static void SelectiveDeleteRec(Node currentNode)
    {
        if (currentNode == null)
        {
            return;
        }
        foreach (var childNode in currentNode.children)
        {
            SelectiveDeleteRec(childNode);
        }
        Destroy(currentNode.gameObject);
    }
    public static NodeStorage GetNodeStorageByName(NodeStorage currentNode, string name)
    {
        if (currentNode.name == name)
        {
            return currentNode;
        }

        foreach (var childNode in currentNode.children)
        {
            NodeStorage result = GetNodeStorageByName(childNode, name);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }
    public void RelocateCard(GameObject card)
    {
        Node cardNode  = card.GetComponent<Node>();
        nodeTmp = ConvertToNode2(cardNode); 
        if (cardNode != null && cardNode.parentNode!=null)
        {
            leftController.GetComponent<LeftController>().AttachCard(CreateMindMapCardObj(cardNode.nodeName,cardNode));
            DeleteCard(cardNode.gameObject);
        }
    }
    public void ReplaceCard(string str)
    {
        cardToReplace.GetComponent<Node>().nodeName= str;
        cardToReplace.GetComponent<Node>().gameObject.GetComponent<CardScript>().ChangeSubTxt(str);
        // GetNodeByName(null,"c").name= str;
        // GetNodeByName(null, str).gameObject.GetComponent<CardScript>().ChangeSubTxt(str);
        // manual or gpt --> gpt 3 alternative cards
    }
    public void SetCardToReplace(GameObject card)
    {
        cardToReplace = card;
    }
    public void SetCardToReplaceByName(string str)
    {
        cardToReplace = GetNodeByName(null, str).gameObject;
    }
    public void DeleteCard(GameObject card)
    {
        Node child = card.GetComponent<Node>();
        Node parent = child.parentNode;
        SelectiveDeleteRec(child);
        parent.children.Remove(child);
    }
    public void DestroyAll()
    {
        if (rootNode != null)
        {
            Debug.Log("Deleting...");
            rootNode.DeleteHierarchy();
        }
    }
    public void DestroyAICards()
    {
        cardsTmp.ForEach(gmo =>
        {
            Destroy(gmo);
        });
    }
    private List<Vector3> CalculateChildNodePositions(Node parentNode, int numberOfNodes)
    {
        List<Vector3> positions = new List<Vector3>();
        //Debug.Log(horzSpacing);
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
    private Node CreateNode(Vector3 position, string subTxt = "Main")
    {
        GameObject nodeObject = Instantiate(cardsMindMapPrf, position, Quaternion.identity);
        nodeObject.transform.Rotate(0f, -90f, 0f);
        Node node = nodeObject.AddComponent<Node>();
        nodeObject.layer = LayerMask.NameToLayer("CardsLayer");
        nodeObject.GetComponent<CardScript>().ChangeSubTxt(subTxt);
        node.nodeName = subTxt;
        lastNode = node;
        nodeObject.gameObject.name = "MindMap(NormalCard): " + subTxt;
        // Debug.Log("Node Name: "+ node.nodeName);
        return node;
    }
    private NodeStorage ConvertToNode2(Node node)
    {
        if (node == null)
        {
            return null;
        }

        NodeStorage node2 = new NodeStorage();
        node2.name = node.nodeName;

        foreach (Node childNode in node.children)
        {
            NodeStorage childNode2 = ConvertToNode2(childNode);
            childNode2.parentNode = node2;
            node2.children.Add(childNode2);
        }

        return node2;
    }
    private void CalculateHorizontalSpacing(int numberOfNodes)
    {
        horzSpacing = Mathf.Lerp(4f, 0.6f, Mathf.InverseLerp(1, 6, numberOfNodes));
    }
    //private void ReconstructHierarchyStructure(NodeStorage currentNode, int depth = 0)
    //{

    //    if (currentNode == null)
    //    {
    //        return;
    //    }
    //    currentNode.children.ForEach(child => { child.parentNode = currentNode; });
    //    foreach (var childNode in currentNode.children)
    //    {
    //        ReconstructHierarchy(childNode, depth + 1);
    //    }
    //}
    private void ReconstructHierarchy(NodeStorage currentNode, int depth = 0)
    {

        if (currentNode == null)
        {
            return;
        }


        int count = 0;
        if (currentNode.parentNode == null)
        {
            count = currentNode.children.Count;
        }
        else
        {
            foreach (NodeStorage n in currentNode.parentNode.children)
            {
                count = count + n.children.Count;
            }
        }
        List<string> childrenList = new List<string>();
        currentNode.children.ForEach(child => childrenList.Add(child.name));
        CreateChildrenNodes(GetNodeByName(null, currentNode.name), childrenList.Count, childrenList, count);



        string indentation = new string(' ', depth * 2);
        //Debug.Log($"{indentation}Node: {currentNode.name}, Children Count: {currentNode.children.Count}");

        //Debug.Log($"{indentation}Children: {string.Join(", ", currentNode.children.Select(child => child.name))}");

        // Recursively print information for each child
        foreach (var childNode in currentNode.children)
        {
            ReconstructHierarchy(childNode, depth + 1);
        }
    }

}
