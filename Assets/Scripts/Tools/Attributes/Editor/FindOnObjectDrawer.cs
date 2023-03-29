using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(FindOnObject))]
public class FindOnObjectDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GameObject gameObject = (property.serializedObject.targetObject as MonoBehaviour).gameObject;
        int i = (attribute as FindOnObject).depthIndex;
        Type parentType = property.serializedObject.targetObject.GetType();
        var fieldType = parentType.GetField(property.propertyPath);
        Type type = fieldType.FieldType;
        Component tar;
        if (i == 0)
        {
            tar = gameObject.GetComponent(type);
            if (tar == null)
                Debug.LogWarning("Couldn't find object of type " + type + " on " + gameObject+" gameobject");
        }
        else if (i > 0)
        {
            checkChilds(gameObject.transform, type,out tar);
            if (tar == null)
                Debug.LogWarning("Couldn't find object of type " + type + " in any parent of object "+gameObject);
        }
        else
        {
            checkParent(gameObject.transform,type,out tar);
            if (tar == null)
                Debug.LogWarning("Couldn't find object of type " + type + " in any child of object " + gameObject);
        }
        property.objectReferenceValue = tar;
        position = EditorGUI.IndentedRect(position);
        Rect rect = new Rect(position);
        rect.height = 20f;
        EditorGUI.LabelField(rect, label);
        rect.y += rect.height;
        EditorGUI.indentLevel++;
        EditorGUI.indentLevel--;

    }

    private void checkChilds(Transform transform, Type type,out Component comp)
    {
        Queue<Transform> q = new Queue<Transform>();
        q.Enqueue(transform);
        while (q.Count > 0)
        {
            var t = q.Dequeue();
            foreach (Transform ch in t)
            {
                if (transform.TryGetComponent(type, out  comp))
                {
                    return;
                }
                q.Enqueue(ch);
            }
        }
        comp = null;
    }

    private Transform checkParent(Transform transform,Type type,out Component comp)
    {
        if(transform.TryGetComponent(type,out comp))
        {
            return null;
        }
        else
        {
            if (transform.parent != null)
                return checkParent(transform.parent, type,out comp);
            else            
                return null;          
        }
    }
}
