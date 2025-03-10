using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject principal. Reglas:
/// 1) totalFloors >= 1.
/// 2) Primer piso: 1 sala, Start=100%.
/// 3) Último piso (si totalFloors>1): 1 sala, Boss=100%.
/// 4) maxRooms <= 4.
/// 5) Si un piso (i-1) tiene maxRooms >=3, el piso i debe tener minRooms >=2.
/// 6) Piso i no supera (maxRooms del piso i-1 * 2).
/// 7) Al aumentar totalFloors, el antiguo último piso pasa a piso intermedio, se regenera.
/// 8) Los pisos intermedios se pueden editar manualmente, salvo la parte de clamping en OnValidate.
/// </summary>
[CreateAssetMenu(fileName = "MapConfig", menuName = "Map/MapConfig")]
public class MapConfig : ScriptableObject
{
    [Header("Numero de pisos")]
    public int totalFloors;

    [Header("Pisos")]
    public List<FloorConfig> floors = new List<FloorConfig>();

    [Header("Tipos de Nodo Disponibles")]
    public List<NodeTypeData> possibleNodeTypes = new List<NodeTypeData>();

    [Header("Tipos de Nodo Especiales")]
    public NodeTypeData startNodeType;
    public NodeTypeData bossNodeType;

    /// <summary>
    /// Este método se llama cuando cambias valores en el Inspector.
    /// Ajusta la lista de pisos y aplica las reglas de validación.
    /// </summary>
    private void OnValidate()
    {
        // Nunca menos de 1
        if (totalFloors < 1)
            totalFloors = 1;

        // Si el valor de TotalFloors no es igual al numero de pisos
        // Añadimos o quitamos pisos
        while (floors.Count < totalFloors)
        {
            floors.Add(new FloorConfig());
        }
        while (floors.Count > totalFloors)
        {
            floors.RemoveAt(floors.Count - 1);
        }

        for (int i = 0; i < floors.Count; i++)
        {
            FloorConfig floor = floors[i];

            // --- Piso 0 asignamos nodo Start
            if (i == 0)
            {
                floor.minRooms = 1;
                floor.maxRooms = 1;
                floor.nodeTypeChances = new List<NodeTypeChance>()
            {
                new NodeTypeChance { type = NodeType.Start, percentage = 100 }
            };
            }
            // --- Último piso asignamos nodo boss
            else if (i == floors.Count - 1 && floors.Count > 1)
            {
                floor.minRooms = 1;
                floor.maxRooms = 1;
                floor.nodeTypeChances = new List<NodeTypeChance>()
            {
                new NodeTypeChance { type = NodeType.Boss, percentage = 100 }
            };
            }
            // --- Pisos intermedios
            else
            {
                // Si era Boss o Start forzado, lo hacemos intermedio
                if (IsBossFloor(floor) || IsStartFloor(floor))
                {
                    RegenerateFloorAsIntermediate(floor);
                }

                // Limite maximo de nodos por piso
                // 1) maxRooms <= 4
                floor.maxRooms = Mathf.Min(floor.maxRooms, 4);

                // Comprobacion que los nodos del piso actual
                // sean los del piso anterior*2 (2 conecxiones por nodo)
                // 3) maxRooms <= (maxRooms piso anterior * 2)
                int prevMax = floors[i - 1].maxRooms;
                floor.maxRooms = Mathf.Min(floor.maxRooms, prevMax * 2);

                // Min room <0 y >maxRooms
                // 4) Re-clamp tras esa regla
                floor.minRooms = Mathf.Clamp(floor.minRooms, 1, floor.maxRooms);

                // Si el piso anterior tiene mas de 2 nodos, en este piso tiene que haber
                // minimos >=2
                // 5) Si el piso anterior tenía 3 o 4 => minRooms >= 2
                if (prevMax >= 3)
                {
                    floor.minRooms = Mathf.Max(floor.minRooms, 2);
                }

                // 6) *** NUEVA REGLA ***: minRooms del piso i <= (minRooms del piso i-1) * 2
                //    Para que incluso en el peor caso (piso anterior solo genera su minRooms),
                //    el piso actual sea conectable.
                int prevMin = floors[i - 1].minRooms;
                floor.minRooms = Mathf.Min(floor.minRooms, prevMin * 2);

                // 7) Clamp final: minRooms no exceda maxRooms
                floor.minRooms = Mathf.Clamp(floor.minRooms, 1, floor.maxRooms);


                
                // Si la lista nodeTypeChances está vacía, la llenamos con 0% de cada tipo
                if (floor.nodeTypeChances == null || floor.nodeTypeChances.Count == 0)
                {
                    floor.nodeTypeChances = new List<NodeTypeChance>();
                    foreach (NodeType nt in Enum.GetValues(typeof(NodeType)))
                    {
                        floor.nodeTypeChances.Add(new NodeTypeChance
                        {
                            type = nt,
                            percentage = 1
                        });
                    }
                }
            }
        }
    }

    /// <summary>
    /// Comprueba si este piso está forzado a Boss=100% (1 sala).
    /// </summary>
    private bool IsBossFloor(FloorConfig floor)
    {
        if (floor.minRooms == 1 && floor.maxRooms == 1 && floor.nodeTypeChances.Count == 1)
        {
            var chance = floor.nodeTypeChances[0];
            return (chance.type == NodeType.Boss && chance.percentage == 100);
        }
        return false;
    }

    /// <summary>
    /// Comprueba si este piso está forzado a Start=100% (1 sala).
    /// </summary>
    private bool IsStartFloor(FloorConfig floor)
    {
        if (floor.minRooms == 1 && floor.maxRooms == 1 && floor.nodeTypeChances.Count == 1)
        {
            var chance = floor.nodeTypeChances[0];
            return (chance.type == NodeType.Start && chance.percentage == 100);
        }
        return false;
    }

    /// <summary>
    /// Convierte un piso que estaba forzado como Boss (o Start) en un piso intermedio
    /// con valores por defecto.
    /// </summary>
    private void RegenerateFloorAsIntermediate(FloorConfig floor)
    {
        floor.minRooms = 1;
        floor.maxRooms = 3; // Valor por defecto, ajustable
        floor.nodeTypeChances = new List<NodeTypeChance>();

        foreach (NodeType nt in Enum.GetValues(typeof(NodeType)))
        {
            floor.nodeTypeChances.Add(new NodeTypeChance
            {
                type = nt,
                percentage = 1
            });
        }
    }
}

/// <summary>
/// Representa la probabilidad (en %) de que aparezca un cierto NodeType.
/// </summary>
[System.Serializable]
public class NodeTypeChance
{
    public NodeType type;
    [Range(0, 100)]
    public int percentage;
}

/// <summary>
/// Configuración de un piso:
/// - minRooms, maxRooms
/// - nodeTypeChances: lista con probabilidades para cada NodeType
/// </summary>
[System.Serializable]
public class FloorConfig
{
    public int minRooms = 1;
    public int maxRooms = 1;
    public List<NodeTypeChance> nodeTypeChances = new List<NodeTypeChance>();
}
