using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "NewMapConfig", menuName = "Map/Map Configuration")]
public class MapConfig : ScriptableObject
{
    [Min(1)] public int levels = 5;
    public List<Vector2> minMaxNodesPerLevel = new List<Vector2>();
    [SerializeField] public List<NodeTypeData> possibleNodes;

    [System.Serializable]
    public class LevelNodeChances
    {
        public string levelName;
        public List<NodeChance> nodeChances = new List<NodeChance>();
    }

    [System.Serializable]
    public class NodeChance
    {
        public NodeType type;
        public bool enabled = false; // Nuevo check de activación
        [Range(0, 100)] public float spawnChance = 0; // Probabilidad por defecto
    }

    public List<LevelNodeChances> levelConfigs = new List<LevelNodeChances>();


#if UNITY_EDITOR
    public void OnValidate()
    {
        AdjustNodeLists();
    }
#endif

    private void AdjustNodeLists()
    {
        while (minMaxNodesPerLevel.Count < levels)
            minMaxNodesPerLevel.Add(new Vector2(1, 3));
        while (minMaxNodesPerLevel.Count > levels)
            minMaxNodesPerLevel.RemoveAt(minMaxNodesPerLevel.Count - 1);

        while (levelConfigs.Count < levels)
        {
            LevelNodeChances newLevel = new LevelNodeChances
            {
                levelName = $"Level {levelConfigs.Count + 1}",
                nodeChances = new List<NodeChance>()
            };

            foreach (NodeType nodeType in System.Enum.GetValues(typeof(NodeType)))
            {
                newLevel.nodeChances.Add(new NodeChance { type = nodeType, enabled = false, spawnChance = 1 });
            }
            levelConfigs.Add(newLevel);
        }

        while (levelConfigs.Count > levels)
        {
            levelConfigs.RemoveAt(levelConfigs.Count - 1);
        }

        foreach (var level in levelConfigs)
        {
            List<NodeType> existingTypes = new List<NodeType>();
            List<NodeChance> uniqueNodeChances = new List<NodeChance>();

            foreach (var nodeChance in level.nodeChances)
            {
                if (!existingTypes.Contains(nodeChance.type))
                {
                    existingTypes.Add(nodeChance.type);
                    uniqueNodeChances.Add(nodeChance);
                }
            }

            level.nodeChances = uniqueNodeChances;

            foreach (NodeType nodeType in System.Enum.GetValues(typeof(NodeType)))
            {
                if (!existingTypes.Contains(nodeType))
                {
                    level.nodeChances.Add(new NodeChance { type = nodeType, enabled = false, spawnChance = 0 });
                }
            }

            // Verificar que haya suficientes nodos con spawnChance > 0
            int requiredNodes = (int)minMaxNodesPerLevel[levelConfigs.IndexOf(level)].x;
            int availableNodeCount = 0;

            foreach (var nodeChance in level.nodeChances)
            {
                if (nodeChance.spawnChance > 0)
                {
                    availableNodeCount++;
                }
            }

            if (availableNodeCount < requiredNodes)
            {
                // Lanza un error si no hay suficientes nodos con spawnChance válido
                string missingNodeMessage = $"Error en {level.levelName}: Se requieren al menos {requiredNodes} nodos con spawnChance > 0, pero solo hay {availableNodeCount}.";
                Debug.LogError(missingNodeMessage);

                // Detalles adicionales sobre nodos con spawnChance en 0
                foreach (var nodeChance in level.nodeChances)
                {
                    if (nodeChance.spawnChance <= 0)
                    {
                        Debug.LogError($"Nodo {nodeChance.type} en {level.levelName} tiene spawnChance = 0, por lo que no podrá aparecer.");
                    }
                }
            }
        }
    }


}