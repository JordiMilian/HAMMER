using UnityEngine;

public enum NodeType
{
    Start,
    ZonaDescanso,
    Boss,
    CombateFacil,
    CombateMedio,
    CombateDificil,
    CombateSuperDificil,
    Tienda
}

[CreateAssetMenu(fileName = "NewNodeType", menuName = "Map/Node Type")]
public class NodeTypeData : ScriptableObject
{
    public string nodeName;
    public NodeType type;
    [SerializeField] private GameObject[] Salas;
    public Sprite image;
}