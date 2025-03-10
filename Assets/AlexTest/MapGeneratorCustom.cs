using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorCustom : MonoBehaviour
{
    [Header("Configuración de MapConfig")]
    public MapConfig mapConfig; // Referencia al ScriptableObject

    [Header("Espaciado visual")]
    public float verticalSpacing = 2f;
    public float horizontalSpacing = 2f;

    [Header("Opciones de visualización")]
    [Tooltip("Material usado para las líneas de conexión")]
    public Material lineMaterial;

    [Tooltip("Grosor de la línea de conexión")]
    public float lineWidth = 0.05f;

    // Estructura interna para guardar datos de cada nodo/piso
    private List<List<MapNodeData>> mapFloors = new List<List<MapNodeData>>();

    // Guardamos los GOs generados para poder reiniciar fácilmente
    private List<GameObject> generatedNodes = new List<GameObject>();
    // Contenedor para almacenar los LineRenderers
    private List<LineRenderer> generatedLines = new List<LineRenderer>();

    void Start()
    {
        if (mapConfig == null)
        {
            Debug.LogError("MapConfig no asignado. Por favor, asigna un ScriptableObject MapConfig.");
            return;
        }

        GenerarMapa();
    }

    private void Update()
    {
        // Reiniciar Mapa con la barra espaciadora
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LimpiarMapa();
            GenerarMapa();
        }
    }

    private void LimpiarMapa()
    {
        // Eliminar los nodos
        foreach (GameObject go in generatedNodes)
        {
            Destroy(go);
        }
        generatedNodes.Clear();

        // Eliminar las líneas
        foreach (var line in generatedLines)
        {
            if (line != null)
                Destroy(line.gameObject);
        }
        generatedLines.Clear();

        // Limpiar estructura interna
        mapFloors.Clear();
    }

    private void GenerarMapa()
    {
        // 1) Crear estructura de nodos en memoria
        CrearNodos();

        // 2) Conectar nodos con la regla de máx 2 conexiones en cada lado
        //    y garantizando que todos los nodos estén conectados hasta
        //    llegar al inicio y al final.
        ConectarNodos();

        // 3) Instanciar GameObjects de nodos
        InstanciarNodosEnEscena();

        // 4) Crear LineRenderers para las conexiones
        CrearLineasDeConexion();
    }

    //----------------------------------------------------------------------------------
    // 1) Crear la lista de nodos según la configuración
    //----------------------------------------------------------------------------------
    private void CrearNodos()
    {
        int[] nodosPorPiso = new int[mapConfig.totalFloors];
        for (int floorIndex = 0; floorIndex < mapConfig.totalFloors; floorIndex++)
        {
            FloorConfig floorConfig = mapConfig.floors[floorIndex];
            List<MapNodeData> floorNodes = new List<MapNodeData>();

            int nodeCount = Random.Range(floorConfig.minRooms, floorConfig.maxRooms + 1);

            if (floorIndex - 1 >= 0)
            {
                if (nodosPorPiso[floorIndex - 1] != 0)
                {
                    if(nodosPorPiso[floorIndex]*2<= nodeCount / 2)
                    {
                        int nodosPosibles;
                        if (nodosPorPiso[floorIndex - 1] == 3)
                        {
                             nodosPosibles = Random.Range(nodosPorPiso[floorIndex - 1] / 2+1, (nodosPorPiso[floorIndex - 1] * 2) + 1);
                        }
                        else
                        {
                             nodosPosibles = Random.Range(nodosPorPiso[floorIndex - 1] / 2, (nodosPorPiso[floorIndex - 1] * 2) + 1);
                        }
                        nodeCount = nodosPosibles;

                        if (nodeCount <= 0)
                        {
                            nodeCount = 1;
                        }

                        if (nodeCount >= 4)
                        {
                            nodeCount = 4;
                        }

                       
                    }
                }
                
            }

            //Piso anterior al final 2 nodos
            if(floorIndex+1 == mapConfig.totalFloors - 1)
            {
                nodeCount = 2;
            }

            //Piso final 1 final 1 nodo
            if(floorIndex+1 >= mapConfig.totalFloors)
            {
                nodeCount = 1;
            }

            for (int i = 0; i < nodeCount; i++)
            {
                NodeTypeData nodeTypeData;

                if (floorIndex == 0) // Primer piso (Start)
                {
                    nodeTypeData = mapConfig.startNodeType;
                }
                else if (floorIndex == mapConfig.totalFloors - 1) // Último piso (Boss)
                {
                    nodeTypeData = mapConfig.bossNodeType;
                }
                else // Pisos intermedios
                {
                    nodeTypeData = ObtenerTipoDeNodoAleatorio(floorConfig);
                }

                MapNodeData newNodeData = new MapNodeData();
                newNodeData.floor = floorIndex;
                newNodeData.nodeTypeData = nodeTypeData;
                floorNodes.Add(newNodeData);
                nodosPorPiso[floorIndex] += 1;
            }

            mapFloors.Add(floorNodes);
        }
    }

    //----------------------------------------------------------------------------------
    // 2) Conectar nodos con la regla de máx 2 conexiones arriba/abajo,
    //    garantizando que cada nodo pueda llegar al piso 0 y al piso final.
    //----------------------------------------------------------------------------------
    private void ConectarNodos()
    {
        // Recorremos piso a piso
        for (int i = 0; i < mapConfig.totalFloors - 1; i++)
        {
            List<MapNodeData> currentFloor = mapFloors[i];
            List<MapNodeData> nextFloor = mapFloors[i + 1];

            // ----------------------------------------------------------
            // PASO A: Asegurar que cada nodo de nextFloor tenga al menos
            //         una conexión con currentFloor.
            // ----------------------------------------------------------
            foreach (var nextNode in nextFloor)
            {
                if (nextNode.upConnections == 0)
                {
                    // Busca un candidato en currentFloor con downConnections < 2
                    var candidates = currentFloor.FindAll(n => n.downConnections < 2);
                    if (candidates.Count > 0)
                    {
                        var randomCandidate = candidates[Random.Range(0, candidates.Count)];
                        CrearConexion(randomCandidate, nextNode);
                    }
                }
            }

            // ----------------------------------------------------------
            // PASO B: Asegurar que cada nodo de currentFloor tenga al menos
            //         una conexión con nextFloor.
            // ----------------------------------------------------------
            foreach (var currentNode in currentFloor)
            {
                if (currentNode.downConnections == 0)
                {
                    var candidates = nextFloor.FindAll(n => n.upConnections < 2);
                    if (candidates.Count > 0)
                    {
                        var randomCandidate = candidates[Random.Range(0, candidates.Count)];
                        CrearConexion(currentNode, randomCandidate);
                    }
                }
            }

            // ----------------------------------------------------------
            // PASO C: Conexión extra aleatoria (solo 1) en caso de que
            //         TODOS los nodos ya tengan >= 1 conexión.
            // ----------------------------------------------------------

            // 1) Verificamos si aún queda algún nodo sin conexión mínima
            bool anyNodeWithoutConnection = false;

            // 1) Recorremos currentFloor buscando si algún nodo tiene downConnections == 0
            foreach (var node in currentFloor)
            {
                if (node.downConnections == 0)
                {
                    anyNodeWithoutConnection = true;
                    break;
                }
            }

            // 2) Solo si no encontramos ninguno en currentFloor, buscamos en nextFloor
            if (!anyNodeWithoutConnection)
            {
                foreach (var node in nextFloor)
                {
                    if (node.upConnections == 0)
                    {
                        anyNodeWithoutConnection = true;
                        break;
                    }
                }
            }

            // 2) Solo hacemos el paso "extra" si TODOS tienen al menos 1.
            if (!anyNodeWithoutConnection)
            {
                // Generamos un número del 1 al 11.
                int randomValue = Random.Range(1, 12); // 12 es excluyente, así que es 1..11
                                                       // Si es 4 o menos, generamos una conexión extra aleatoria.
                if (randomValue <= 4)
                {
                    // Elegimos un nodo de currentFloor que aún pueda conectarse más (downConnections < 2)
                    var currentCandidates = currentFloor.FindAll(n => n.downConnections < 2);
                    // Elegimos un nodo de nextFloor que aún pueda conectarse más (upConnections < 2)
                    var nextCandidates = nextFloor.FindAll(n => n.upConnections < 2);

                    if (currentCandidates.Count > 0 && nextCandidates.Count > 0)
                    {
                        var currentNode = currentCandidates[Random.Range(0, currentCandidates.Count)];
                        var nextNode = nextCandidates[Random.Range(0, nextCandidates.Count)];

                        // Evitamos duplicar conexión
                        if (!currentNode.connectedNodes.Contains(nextNode))
                        {
                            CrearConexion(currentNode, nextNode);
                        }
                    }
                }
            }
        }
    }


    //----------------------------------------------------------------------------------
    // 3) Instanciar en escena los nodos
    //----------------------------------------------------------------------------------
    private void InstanciarNodosEnEscena()
    {
        for (int floorIndex = 0; floorIndex < mapConfig.totalFloors; floorIndex++)
        {
            List<MapNodeData> floorNodes = mapFloors[floorIndex];
            int nodeCount = floorNodes.Count;

            for (int i = 0; i < nodeCount; i++)
            {
                MapNodeData nodeData = floorNodes[i];

                float xPos = (i - (nodeCount - 1) / 2f) * horizontalSpacing;
                float yPos = floorIndex * -verticalSpacing;
                Vector3 pos = new Vector3(xPos, yPos, 0);

                GameObject nodoGO = CrearNodo(nodeData.nodeTypeData, pos, $"{floorIndex}-{i}");
                nodeData.attachedGO = nodoGO;

                generatedNodes.Add(nodoGO);
            }
        }
    }

    //----------------------------------------------------------------------------------
    // 4) Crear las líneas de conexión con LineRenderer
    //----------------------------------------------------------------------------------
    private void CrearLineasDeConexion()
    {
        for (int floorIndex = 0; floorIndex < mapConfig.totalFloors; floorIndex++)
        {
            List<MapNodeData> floorNodes = mapFloors[floorIndex];

            foreach (var nodeData in floorNodes)
            {
                foreach (var connectedNode in nodeData.connectedNodes)
                {
                    // Dibujamos la línea solo si el nodo conectado está en un piso distinto
                    // y para no duplicar, dibujamos si connectedNode está más abajo
                    if (connectedNode.floor > nodeData.floor)
                    {
                        GameObject lineGO = new GameObject($"Line_{nodeData.floor}->{connectedNode.floor}");
                        LineRenderer lr = lineGO.AddComponent<LineRenderer>();

                        lr.material = (lineMaterial != null) ? lineMaterial : new Material(Shader.Find("Sprites/Default"));
                        lr.widthMultiplier = lineWidth;

                        Vector3 startPos = nodeData.attachedGO.transform.position;
                        Vector3 endPos = connectedNode.attachedGO.transform.position;

                        lr.SetPosition(0, startPos);
                        lr.SetPosition(1, endPos);

                        // Opcional: Ajustes de sorting layer, etc.
                        lr.sortingOrder = -1;

                        generatedLines.Add(lr);
                    }
                }
            }
        }
    }

    //----------------------------------------------------------------------------------
    // Función auxiliar para crear/registrar la conexión bidireccional
    //----------------------------------------------------------------------------------
    private void CrearConexion(MapNodeData from, MapNodeData to)
    {
        from.connectedNodes.Add(to);
        to.connectedNodes.Add(from);

        from.downConnections++;
        to.upConnections++;
    }

    //----------------------------------------------------------------------------------
    // Creación individual de un nodo como GameObject
    //----------------------------------------------------------------------------------
    private GameObject CrearNodo(NodeTypeData data, Vector3 pos, string i)
    {
        GameObject miObjeto = new GameObject($"Nodo {i} ({data.name})");

        SpriteRenderer sr = miObjeto.AddComponent<SpriteRenderer>();
        BoxCollider2D coll2D = miObjeto.AddComponent<BoxCollider2D>();
        MapNode mapNode = miObjeto.AddComponent<MapNode>();

        // Asigna valores al nodo (cambia sprite, etc.)
        mapNode.Initialize(data);
        if (data.image != null) sr.sprite = data.image;

        miObjeto.transform.position = pos;
        miObjeto.transform.parent = this.transform;

        if (sr.sprite != null)
        {
            Vector2 spriteSize = sr.sprite.bounds.size;
            coll2D.size = spriteSize;
            coll2D.offset = Vector2.zero;
        }

        miObjeto.transform.localScale = Vector3.one * 1.2f;

        return miObjeto;
    }

    //----------------------------------------------------------------------------------
    // Para asignar el tipo de nodo en pisos intermedios según las probabilidades
    //----------------------------------------------------------------------------------
    private NodeTypeData ObtenerTipoDeNodoAleatorio(FloorConfig floorConfig)
    {
        int totalPercentage = 0;
        foreach (var chance in floorConfig.nodeTypeChances)
        {
            totalPercentage += chance.percentage;
        }

        int randomValue = Random.Range(0, totalPercentage);
        int cumulativePercentage = 0;

        foreach (var chance in floorConfig.nodeTypeChances)
        {
            cumulativePercentage += chance.percentage;
            if (randomValue < cumulativePercentage)
            {
                return mapConfig.possibleNodeTypes.Find(node => node.type == chance.type);
            }
        }

        // Si por alguna razón no encontró nada, retorna null
        return null;
    }

    //----------------------------------------------------------------------------------
    // Clase interna para almacenar la info de cada nodo
    //----------------------------------------------------------------------------------
    private class MapNodeData
    {
        public int floor;
        public NodeTypeData nodeTypeData;
        public GameObject attachedGO;
        public List<MapNodeData> connectedNodes = new List<MapNodeData>();

        // Contadores de conexiones para respetar el máximo de 2 hacia arriba o 2 hacia abajo
        public int upConnections = 0;   // conexiones con nodos de pisos superiores
        public int downConnections = 0; // conexiones con nodos de pisos inferiores
    }
}
