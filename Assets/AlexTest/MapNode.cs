using UnityEngine;

/// <summary>
/// Representa un nodo individual en el mapa.
/// Contiene l�gica para interacci�n, asignaci�n de datos, etc.
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
        // Si deseas actualizar sprite, color, etc. en tiempo real, hazlo aqu�.
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
    /// Ejemplo: si clicas sobre el nodo en modo 2D, puedes mostrar informaci�n.
    /// </summary>
    private void OnMouseDown()
    {
        // Aqu� puedes manejar la l�gica de selecci�n del nodo.
        Debug.Log($"Has clicado en: {nodeData.nodeName} (tipo: {nodeData.type})");
        // O mostrar un panel, mover el jugador, etc.
    }
}
