using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    public NodeType nodeType; // Tipo de nodo
    public List<MapNode> connectedNodes = new List<MapNode>();

    public void SetNodeType(NodeTypeData typeData)
    {
        nodeType = typeData.type;
    }

    private void OnMouseDown()
    {
        Debug.Log($"Nodo seleccionado: {gameObject.name}, Tipo: {nodeType}");
    }
}