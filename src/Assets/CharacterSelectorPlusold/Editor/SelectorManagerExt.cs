using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(SelectorManager))]
public class SelectorManagerExt : Editor
{
    private SerializedObject serObj;
    private SerializedProperty 
        GlobalVariables, Menu, SelectionSpeed, DragSpeed, RotationVector, LockMaterial, FrameCamera,
        AnimToPlay,RotateItem, RotationItem, FirstItem, Onclick;

    private void OnEnable()
    {

        serObj = new SerializedObject(target);
        GlobalVariables = serObj.FindProperty("GlobalVariables");
        Menu = serObj.FindProperty("Menu");
        SelectionSpeed = serObj.FindProperty("SelectionSpeed");
        DragSpeed = serObj.FindProperty("DragSpeed");
        FirstItem = serObj.FindProperty("FirstItem");
        RotateItem = serObj.FindProperty("RotateItem");
        RotationVector = serObj.FindProperty("RotationVector");
        RotationItem = serObj.FindProperty("RotationItem");
        LockMaterial = serObj.FindProperty("LockMaterial"); FrameCamera = serObj.FindProperty("FrameCamera");
        Onclick = serObj.FindProperty("Onclick");
        AnimToPlay = serObj.FindProperty("AnimToPlay");
    }
    public override void OnInspectorGUI()
    {
        serObj.Update();
        SelectorManager MySelectorManager = (SelectorManager)target;
        EditorGUILayout.PropertyField(GlobalVariables, new GUIContent("Global Variables","Reference the global variables to use it on the selector: (Money, Level)"));
        EditorGUILayout.PropertyField(Menu, new GUIContent("Main Menu","Reference the main menu to return when the object is selected"));
        EditorGUILayout.PropertyField(FirstItem, new GUIContent("First", "First Object to appear on Focus"));
        EditorGUILayout.PropertyField(SelectionSpeed, new GUIContent("Selection Speed","Speed between the selection among the objects"));
        EditorGUILayout.PropertyField(DragSpeed, new GUIContent("Drag Speed","Swipe speed when swiping duh!! :)"));
        EditorGUILayout.PropertyField(LockMaterial, new GUIContent("Lock Material","Material choosed for the locked objects"));
        EditorGUILayout.PropertyField(FrameCamera, new GUIContent("FrameCamera"," Auto Adjust the camera position by the size of the object"));
        EditorGUILayout.PropertyField(RotateItem, new GUIContent("Rotate Object","Turning table for the focused object"));

        if (RotateItem.boolValue)
        {
            EditorGUILayout.PropertyField(RotationItem, new GUIContent("Velocity","How fast the focused object will rotate"));
            EditorGUILayout.PropertyField(RotationVector, new GUIContent("Rotation Vector","Choose your desire vector to rotate around"));
        }
        EditorGUILayout.Separator();

        SelectorManager.ActionOnCLick Action = (SelectorManager.ActionOnCLick) Onclick.enumValueIndex;
        EditorGUILayout.PropertyField(Onclick, new GUIContent("On Click/Touch","Choose your Action when Click/Touch the object"));
        if (Action == SelectorManager.ActionOnCLick.PlayAnimation)
        {
            EditorGUILayout.PropertyField(AnimToPlay, new GUIContent("Anim to Play","It will play for all the Objects the named State on its Animator Controller"));
        }
        if (MySelectorManager.GlobalVariables == null)
        {
            EditorGUILayout.HelpBox("No Game Manager found, please link the Game Manager Script", MessageType.Warning);
        }
        if (MySelectorManager.Menu == null)
        {
            EditorGUILayout.HelpBox("No Main Menu Canvas found, please link the Main Menu Canvas", MessageType.Warning);
        }
        if (MySelectorManager.LockMaterial == null)
        {
            EditorGUILayout.HelpBox("No Lock material found, please add a material for locked objects", MessageType.Warning);
        }  
       

        serObj.ApplyModifiedProperties();
    }
}