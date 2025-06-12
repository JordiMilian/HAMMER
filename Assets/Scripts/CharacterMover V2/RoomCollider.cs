using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public struct wallInfo
{
    public Vector2 Pos1, Pos2;
    public Vector2 DiferenceVector1to2;
    public Vector2 Normal;
    public float Lenght;
    public wallInfo(Vector2 pos1, Vector2 pos2, Vector2 diferenceAtoB, Vector2 normal, float lenght)
    {
        Pos1 = pos1;
        Pos2 = pos2;
        DiferenceVector1to2 = diferenceAtoB;
        Normal = normal;
        Lenght = lenght;
    }
}

public class RoomCollider : MonoBehaviour
{
    public PolygonCollider2D polygonCollider;
    public List<wallInfo> wallInfosList = new List<wallInfo>();
    public bool ignoreInnerWalls;
    public bool ignoreOuterWalls;
    public int[] IgnoreCollisionsIndexes;
    public CollisionLayers collisionLayer;
    private void OnEnable()
    {
        SetUpWallInfos();
        CollisionsManager.instance.AddRoomCollider(this);
    }
    private void OnDisable()
    {
        CollisionsManager.instance.RemoveRoomCollider(this);
    }
    public void SetUpWallInfos()
    {
        wallInfosList.Clear();
        for (int p = 0; p < polygonCollider.points.Length; p++)
        {
           
            Vector2 pos1 = transform.TransformPoint(polygonCollider.points[p]);
            Vector2 pos2;
            if (p == polygonCollider.points.Length - 1) { pos2 = transform.TransformPoint(polygonCollider.points[0]); }
            else { pos2 = transform.TransformPoint(polygonCollider.points[p + 1]);}

            Vector2 diferenceVector = pos2 - pos1;
            float lenght = diferenceVector.magnitude;
            Vector2 normal = (new Vector2(diferenceVector.y, -diferenceVector.x)).normalized;

            wallInfosList.Add(new wallInfo(pos1, pos2,diferenceVector, normal, lenght));
        }
    }
    private void OnDrawGizmosSelected()
    {
        for (int w = 0; w < polygonCollider.points.Length; w++)
        {
            Vector2 pos1 = polygonCollider.points[w] + (Vector2)transform.position;
            Vector2 pos2;
            if (w == polygonCollider.points.Length - 1) { pos2 = polygonCollider.points[0] + (Vector2)transform.position; }
            else { pos2 = polygonCollider.points[w + 1] + (Vector2)transform.position; }

            Vector2 midPoint = pos1 + (pos2 - pos1) / 2;

            DrawString(w.ToString(), midPoint);
        }
    }
    public static void DrawString(string text, Vector3 worldPos, Color? textColor = null, Color? backColor = null)
    {
#if UNITY_EDITOR
        UnityEditor.Handles.BeginGUI();
        var restoreTextColor = GUI.color;
        var restoreBackColor = GUI.backgroundColor;

        GUI.color = textColor ?? Color.white;
        GUI.backgroundColor = backColor ?? Color.black;

        var view = UnityEditor.SceneView.currentDrawingSceneView;
        if (view != null && view.camera != null)
        {
            Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
            if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0)
            {
                GUI.color = restoreTextColor;
                UnityEditor.Handles.EndGUI();
                return;
            }
            Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
            var r = new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y);
            GUI.Box(r, text, EditorStyles.numberField);
            GUI.Label(r, text);
            GUI.color = restoreTextColor;
            GUI.backgroundColor = restoreBackColor;
        }
        UnityEditor.Handles.EndGUI();
#endif
    }
}
