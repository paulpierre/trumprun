using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(SelectorEditor))]
public class SelectorEditorExt : Editor
{
    private SerializedObject serObj;
    private SerializedProperty
            Radius, Type, LinearX, LinearY, LinearZ;


    private void OnEnable()
    {
        serObj = new SerializedObject(target);
        Radius = serObj.FindProperty("Radius");
        Type = serObj.FindProperty("Type");
        LinearX = serObj.FindProperty("LinearX");
        LinearY = serObj.FindProperty("LinearY");
        LinearZ = serObj.FindProperty("LinearZ");
       
    }

    public override void OnInspectorGUI()
    {
       serObj.Update();

        SelectorEditor MySelector = (SelectorEditor)target;

        DrawDefaultInspector();

        SelectorEditor.SelectorType type = (SelectorEditor.SelectorType) Type.enumValueIndex;


        if (type == SelectorEditor.SelectorType.Circular)
        {
            EditorGUILayout.PropertyField(Radius, new GUIContent("Radius","Radius of the selector" ));
        }
        if (type == SelectorEditor.SelectorType.Linear)
        {
            EditorGUILayout.PropertyField(Radius, new GUIContent("Distance", "Distance between objects"));
            EditorGUILayout.Slider(LinearX, 0, 1, new GUIContent("Linear X"));
            EditorGUILayout.Slider(LinearY, 0, 1, new GUIContent("Linear Y"));
            EditorGUILayout.Slider(LinearZ, 0, 1, new GUIContent("Linear Z"));
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("Reset Rotation", "Reset the objects rotation to zero ")))
        {
            MySelector.ResetItemRotation();
        }
        if (GUILayout.Button(new GUIContent("Look Rotation", "All items look away from the center")))
        {
            MySelector.LookAtRotation();
        }
        GUILayout.EndHorizontal();


        if (MySelector.MenuCamera == null)
        {
            EditorGUILayout.HelpBox("No camera found, please add a camera to the script", MessageType.Warning);
        }
        

        serObj.ApplyModifiedProperties();
    }
}


