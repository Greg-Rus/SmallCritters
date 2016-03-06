using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomPropertyDrawer (typeof (DifficultyParameter))]
public class DifficultyParameterDrawer : PropertyDrawer {

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) 
	{

		
		EditorGUI.BeginProperty (position, label, property);		
		int indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;
		EditorGUIUtility.labelWidth = 50f;
		EditorGUI.LabelField(position, property.name);
		
		//position = EditorGUI.PrefixLabel (position, GUIUtility.GetControlID (FocusType.Passive), label);
		
		var minRect = new Rect (position.x + position.width*0.25f, position.y, position.width*0.2f, position.height);
		var maxRect = new Rect (position.x+position.width*0.5f, position.y, position.width*0.2f, position.height);
		var currentRect = new Rect (position.x+position.width*0.75f, position.y, position.width*0.2f, position.height);
		
		EditorGUIUtility.labelWidth = 28f;
		EditorGUI.PropertyField (minRect, property.FindPropertyRelative ("min"));
		EditorGUI.PropertyField (maxRect, property.FindPropertyRelative ("max"));//, GUIContent.none);
		EditorGUI.PropertyField (currentRect, property.FindPropertyRelative ("current"));
		
		EditorGUI.indentLevel = indent;
		EditorGUI.EndProperty ();
		
	}
}
