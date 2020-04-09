/*
 * @file	NamedArrayDrawer.cs
 * @brief   
 *
 * @author	Kota Nakagami
 * @date1	2020/04/06(月)
 *
 * @version	1.00
 */


using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer(typeof(NamedArrayAttribute))]
public class NamedArrayDrawer : PropertyDrawer
{
    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        try
        {
            int pos = int.Parse(property.propertyPath.Split('[', ']')[1]);
            EditorGUI.PropertyField(rect, property, new GUIContent(((NamedArrayAttribute)attribute).names[pos]));

        }
        catch
        {
            EditorGUI.PropertyField(rect, property, label);
        }
    }
}


// EOF