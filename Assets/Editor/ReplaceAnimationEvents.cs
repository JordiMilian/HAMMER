using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class ReplaceAnimationEvents : EditorWindow
{
    AnimatorController animatorController;
    string eventToReplace_name, newEvent_name;
    string eventToReplace_stringParam, newEvent_stringParam;
    bool oldEvent_hasStringParam;
    bool newEvent_hasStringParam;

    string eventToDestroy_name;

    [MenuItem("Tools/Animation Events Editor")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ReplaceAnimationEvents)); //I dont fully know what this does 
    }
    private void OnGUI()
    {
        EditorGUILayout.Space(20);
        GUILayout.Label("REPLACER TOOL", EditorStyles.whiteBoldLabel);
        EditorGUILayout.Space();

        animatorController = EditorGUILayout.ObjectField("Animator Controller", animatorController, typeof(AnimatorController),true) as AnimatorController;

        #region REPLACED EVENT
        EditorGUILayout.Space();
        GUILayout.Label("Event To Replace", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        eventToReplace_name = EditorGUILayout.TextField("Name", eventToReplace_name);
        oldEvent_hasStringParam = EditorGUILayout.BeginToggleGroup("String parameter", oldEvent_hasStringParam);
            EditorGUI.indentLevel++;
            eventToReplace_stringParam = EditorGUILayout.TextField("", eventToReplace_stringParam);
            EditorGUI.indentLevel--;
        EditorGUILayout.EndToggleGroup();
        EditorGUI.indentLevel--;
        #endregion

        #region NEW EVENT
        EditorGUILayout.Space();
        GUILayout.Label("New Event", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        newEvent_name = EditorGUILayout.TextField("Name", newEvent_name);

        newEvent_hasStringParam = EditorGUILayout.BeginToggleGroup("String parameter", newEvent_hasStringParam);
            EditorGUI.indentLevel++;
            newEvent_stringParam = EditorGUILayout.TextField("", newEvent_stringParam);
            EditorGUI.indentLevel--;
        EditorGUILayout.EndToggleGroup();
        EditorGUI.indentLevel--;
        #endregion

        #region REPLACE BUTTON
        EditorGUI.BeginDisabledGroup(animatorController == null || eventToReplace_name == "" || newEvent_name == "");
        if (GUILayout.Button("Replace Events"))
        {
            ReplaceEvents(eventToReplace_name, newEvent_name);
        }
        EditorGUI.EndDisabledGroup();
        #endregion

        EditorGUILayout.Space(40);

        #region REMOVER TOOL
        GUILayout.Label("REMOVER TOOL", EditorStyles.whiteBoldLabel);
        EditorGUILayout.Space();
        eventToDestroy_name = EditorGUILayout.TextField("Event to remove", eventToDestroy_name);

        EditorGUI.BeginDisabledGroup(animatorController == null || eventToDestroy_name == "");
        if (GUILayout.Button("Remove Events"))
        {
            DestroyEvents(eventToDestroy_name);
        }
        EditorGUI.EndDisabledGroup();
        #endregion
    }
    void DestroyEvents(string destroy)
    {
        int destroyedEvents = 0;
        foreach (AnimationClip animClip in animatorController.animationClips)
        {
            List<AnimationEvent> newEventsList = new List<AnimationEvent>();

            foreach (AnimationEvent animEvent in animClip.events)
            {
                if (animEvent.functionName != destroy)
                {
                    newEventsList.Add(animEvent);
                }
                else { destroyedEvents++; }
                
            }
            AnimationUtility.SetAnimationEvents(animClip, newEventsList.ToArray());
#if UNITY_EDITOR
            EditorUtility.SetDirty(animClip);
#endif
        }
#if UNITY_EDITOR
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
#endif
        Debug.Log($"Destroyed events: {destroyedEvents}");
    }
    public void ReplaceEvents(string eventToReplace, string s_newEvent)
    {
        int animationsChecked = 0;
        int eventsChecked = 0;
        int eventsReplaced = 0;

        List<int> DirtyAnimationIndexes = new List<int>();
        for (int a = 0; a < animatorController.animationClips.Length; a++)
        {
            bool isDirty = false;
            AnimationClip animClip = animatorController.animationClips[a];
            List<AnimationEvent> newEventsList = new List<AnimationEvent>();
            animationsChecked++;

            foreach (AnimationEvent animEvent in animClip.events)
            {
                eventsChecked++;

                if (animEvent.functionName == eventToReplace) //the event needs to be replaced
                {
                    if (oldEvent_hasStringParam) //If we check the string parameter of the old event
                    {
                        if (animEvent.stringParameter != eventToReplace_stringParam) { continue; }
                    }

                    AnimationEvent newEvent = new AnimationEvent
                    {
                        time = animEvent.time,
                        functionName = s_newEvent,
                    };
                    if (newEvent_hasStringParam) { newEvent.stringParameter = newEvent_stringParam; }

                    newEventsList.Add(newEvent);
                    eventsReplaced++;
                    isDirty = true;
                }
                else //If the event doesnt need replacement
                {
                    newEventsList.Add(animEvent);
                }
            }
            if (isDirty) 
            {
                AnimationUtility.SetAnimationEvents(animClip, newEventsList.ToArray());
                DirtyAnimationIndexes.Add(a); 
            }
        }

//Clean up to make it persistent. I dont quite understand it ITS NOT WORKING THE STRING PARAMETERS ARE NOT STAYING
#if UNITY_EDITOR
        for (int d = 0; d < DirtyAnimationIndexes.Count; d++)
        {
            EditorUtility.SetDirty(animatorController.animationClips[DirtyAnimationIndexes[d]]);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
#endif
        Debug.Log($"{newEvent_name}: All animations: {animationsChecked} - Events checked: { eventsChecked} - Events replaced: { eventsReplaced}");
    }
}
