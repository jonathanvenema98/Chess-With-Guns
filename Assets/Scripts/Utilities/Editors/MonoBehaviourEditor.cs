using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(MonoBehaviour), true)]
public class MonobehaviourInspectorButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (!Application.isEditor) return;
        
        var type = target.GetType();

        foreach (var method in type.GetMethods(
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
        {
            var attributes = method.GetCustomAttributes(typeof(InspectorButtonAttribute), true);

            if (attributes.Length != 0)
            {
                if (GUILayout.Button("Run: " + method.Name))
                {
                    //If the user clicks the button, call the method
                    method.Invoke(target, new object[] { });
                }
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}
