using UnityEngine;


/// <summary>
/// Lista de tipos de nodo que se pueden usar en el mapa.
/// </summary>
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

/// <summary>
/// Define datos para cada tipo de nodo en el mapa.
/// Almacena un sprite, un array de "Salas" o prefabs, etc.
/// </summary>
[CreateAssetMenu(fileName = "NewNodeType", menuName = "Map/Node Type")]
public class NodeTypeData : ScriptableObject
{
    public string nodeName;     // Nombre descriptivo
    public NodeType type;       // Tipo del nodo (Start, Boss, Tienda, etc.)

    [SerializeField] private GameObject[] Salas; // Ejemplo de prefabs relacionados (opcional)

    public Sprite image;        // Sprite que se mostrará en el mapa

    // Puedes añadir más propiedades si quieres datos extras (enemigos, recompensas, etc.)
}
