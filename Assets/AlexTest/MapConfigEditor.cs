using UnityEditor;
using UnityEngine;


#if UNITY_EDITOR
/// <summary>
/// Custom Editor: muestra la lista de 'floors' como 'Piso 0', 'Piso 1', etc.
/// </summary>
[CustomEditor(typeof(MapConfig))]
public class MapConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Vincular objeto serializado
        serializedObject.Update();

        // Mostrar la propiedad totalFloors
        EditorGUILayout.PropertyField(serializedObject.FindProperty("totalFloors"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("possibleNodeTypes"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("startNodeType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("bossNodeType"));

        // Título
        EditorGUILayout.LabelField("Pisos", EditorStyles.boldLabel);

        // Recorremos floors y lo mostramos como 'Piso i'
        SerializedProperty floorsProp = serializedObject.FindProperty("floors");
        EditorGUI.indentLevel++;
        for (int i = 0; i < floorsProp.arraySize; i++)
        {
            SerializedProperty floorProp = floorsProp.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(floorProp, new GUIContent($"Piso {i}"), true);
        }
        EditorGUI.indentLevel--;

        // Aplicamos cambios
        serializedObject.ApplyModifiedProperties();
    }
}
#endif