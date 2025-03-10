using UnityEngine;

/// <summary>
/// Representa un nodo individual en el mapa.
/// Contiene lógica para interacción, asignación de datos, etc.
/// </summary>
public class MapNode : MonoBehaviour
{
    // Referencia a los datos del nodo (ScriptableObject)
    [SerializeField] private NodeTypeData nodeData;

    /// <summary>
    /// Inicializa el nodo con un determinado NodeTypeData.
    /// </summary>
    public void Initialize(NodeTypeData data)
    {
        nodeData = data;
        // Si deseas actualizar sprite, color, etc. en tiempo real, hazlo aquí.
        // Ejemplo:
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null && data.image != null)
        {
            sr.sprite = data.image;
        }
    }


    public NodeTypeData GetData()
    {
        return nodeData;
    }
    /// <summary>
    /// Ejemplo: si clicas sobre el nodo en modo 2D, puedes mostrar información.
    /// </summary>
    private void OnMouseDown()
    {
        // Aquí puedes manejar la lógica de selección del nodo.
        Debug.Log($"Has clicado en: {nodeData.nodeName} (tipo: {nodeData.type})");
        // O mostrar un panel, mover el jugador, etc.
    }
}
