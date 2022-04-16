/*
 * Authors: José Javier Ortiz Vega
 * Date Created:4/10/2022
 * Date Edited: 4/15/2022
 * Description: This will allow to collor objects in the scene for easier recognition when creating certain items 
 *              Can be modified in the window editor
 *              
 *              ******************************
 *              ****         DONE         ****
 *              ******************************
 */

using UnityEngine;
using UnityEditor;

public class ColorWindow : EditorWindow
{
    Color color; //stores the color of the object

    [MenuItem("Window/Color Window")]
    public static void ShowWindow()
    {
        GetWindow<ColorWindow>("Color Window");
    }

    //window code
    private void OnGUI()
    {
        //simple label
        GUILayout.Label("Object Color Changer", EditorStyles.boldLabel);

        //stores the selected color 
        color = EditorGUILayout.ColorField("Color", color);

        if (GUILayout.Button("Color Selected GameObjects"))
        {
            ColorOBJ();
        }

        if (GUILayout.Button("Reset Selected GameObjects"))
        {
            ResetColor();
        }
    }

    void ColorOBJ()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (obj != null)
            {
                renderer.sharedMaterial.color = color;
            }
        }
    }

    void ResetColor()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (obj != null)
            {
                renderer.sharedMaterial.color = Color.white;
            }
        }
    }
}
