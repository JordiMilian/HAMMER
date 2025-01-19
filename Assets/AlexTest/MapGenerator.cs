using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public MapConfig mapConfig;
    public List<List<MapNode>> mapNodes = new List<List<MapNode>>();

    void Start()
    {
        GenerateMap();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RegenerateMap();
        }
    }

    void RegenerateMap()
    {
        foreach (List<MapNode> list in mapNodes)
        {
            foreach (MapNode node in list)
            {
                Destroy(node.gameObject);
            }
            list.Clear();
        }
        mapNodes.Clear();
        GenerateMap();
    }

    void GenerateMap()
    {
        for (int i = 0; i < mapConfig.levels; i++)
        {
            List<MapNode> levelNodes = new List<MapNode>();
            HashSet<NodeType> selectedNodes = new HashSet<NodeType>();

            Vector2 nodesXlevel = mapConfig.minMaxNodesPerLevel[i];
            int minNodesThisLevel = (int)nodesXlevel.x;
            int maxNodesThisLevel = (int)nodesXlevel.y;

            // Obtener nodos habilitados o con spawnChance > 0
            MapConfig.LevelNodeChances levelConfig = mapConfig.levelConfigs[i];
            List<MapConfig.NodeChance> availableNodes = levelConfig.nodeChances.FindAll(n => n.enabled || n.spawnChance > 0);

            // **Nueva validación:** Si no hay suficientes nodos, cancelar generación
            if (maxNodesThisLevel > availableNodes.Count)
            {
                Debug.LogError($"ERROR en Level {i + 1}: Se necesitan hasta {maxNodesThisLevel} nodos, pero solo hay {availableNodes.Count} disponibles.");
                return;
            }

            // Definir cuántos nodos generar en este nivel
            int nodesThisLevel = (i == mapConfig.levels - 1) ? 1 : Random.Range(minNodesThisLevel, maxNodesThisLevel + 1);

            for (int j = 0; j < nodesThisLevel; j++)
            {
                Vector3 position = new Vector3(j * 2.5f - nodesThisLevel * 1.25f, i * 2.5f, 0);
                NodeTypeData selectedNodeType = SelectNodeType(i, selectedNodes);

                if (selectedNodeType != null)
                {
                    selectedNodes.Add(selectedNodeType.type);
                    GameObject nodeObj = crearNodo(selectedNodeType, position, i.ToString());
                    MapNode node = nodeObj.GetComponent<MapNode>();
                    node.SetNodeType(selectedNodeType);
                    levelNodes.Add(node);

                    if (i > 0 && mapNodes[i - 1].Count > 0)
                    {
                        int prevLevelIndex = Random.Range(0, mapNodes[i - 1].Count);
                        mapNodes[i - 1][prevLevelIndex].connectedNodes.Add(node);
                    }
                }
                else
                {
                    Debug.LogError($"ERROR en Level {i + 1}: No se pudo seleccionar un nodo válido.");
                }
            }

            mapNodes.Add(levelNodes);
        }
    }

    NodeTypeData SelectNodeType(int level, HashSet<NodeType> selectedNodes)
    {
        if (level == mapConfig.levels - 1)
        {
            return SelectBossNode();
        }

        MapConfig.LevelNodeChances levelConfig = mapConfig.levelConfigs[level];
        List<MapConfig.NodeChance> availableChances = levelConfig.nodeChances.FindAll(n => (n.enabled || n.spawnChance > 0) && !selectedNodes.Contains(n.type));

        if (availableChances.Count == 0)
        {
            Debug.LogError($"ERROR: No hay nodos válidos disponibles para Level {level + 1}. Total nodos posibles: {levelConfig.nodeChances.Count}");
            return null;
        }
        Debug.Log("ENTRO A SELECT BASED CHANCES");
        return SelectNodeBasedOnChances(availableChances);
    }

    NodeTypeData SelectNodeBasedOnChances(List<MapConfig.NodeChance> nodeChances)
    {
        float totalWeight = 0;
        foreach (var nodeChance in nodeChances)
        {
            totalWeight += nodeChance.spawnChance;
        }
        Debug.Log($"Total Weight: {totalWeight}");

        if (totalWeight == 0)
        {
            Debug.LogError("ERROR: La suma de probabilidades de nodos es 0. Esto no debería ocurrir.");
            return null;
        }

        float randomValue = Random.Range(0, totalWeight);
        float currentWeight = 0;

        foreach (var nodeChance in nodeChances)
        {
            currentWeight += nodeChance.spawnChance;
            Debug.Log(currentWeight);
            if (randomValue <= currentWeight)
            {
                Debug.Log(randomValue + " " + currentWeight);
                NodeTypeData selectedNode = null;
                foreach (var node in mapConfig.possibleNodes)
                {
                    if (node.type == nodeChance.type)
                    {
                        selectedNode = node;
                        break; // Sale del ciclo una vez que se encuentra el nodo
                    }
                }

                if (selectedNode == null)
                {
                    Debug.LogError($"ERROR: No se encontró un nodo con el tipo {nodeChance.type}.");
                    return null;
                }

                return selectedNode;
            }
        }

        Debug.LogError("ERROR: No se pudo seleccionar un nodo a pesar de tener probabilidades válidas.");
        return null;
    }

    NodeTypeData SelectBossNode()
    {
        return mapConfig.possibleNodes.Find(n => n.type == NodeType.Boss);
    }

    private GameObject crearNodo(NodeTypeData data, Vector3 pos, string i)
    {
        GameObject miObjeto = new GameObject($"Level {i} Nodo {data.name} {data.type}");
        SpriteRenderer sr = miObjeto.AddComponent<SpriteRenderer>();
        BoxCollider2D coll2D = miObjeto.AddComponent<BoxCollider2D>();
        MapNode mapNode = miObjeto.AddComponent<MapNode>();
        sr.sprite = data.image;
        miObjeto.transform.position = pos;
        miObjeto.transform.parent = transform;

        Vector2 S = miObjeto.GetComponent<SpriteRenderer>().sprite.bounds.size;
        miObjeto.GetComponent<BoxCollider2D>().size = S;
        miObjeto.GetComponent<BoxCollider2D>().offset = Vector2.zero;

        miObjeto.transform.localScale = Vector3.one * 1.2f;
        return miObjeto;
    }
}
