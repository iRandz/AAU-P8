/* This wizard will replace a selection with an object or prefab.
 * Scene objects will be cloned (destroying their prefab links).
 * Original coding by 'yesfish', nabbed from Unity Forums
 * 'keep parent' added by Dave A (also removed 'rotation' option, using localRotation
 * Updated to work in Unity 2020.2.6f1 by Christian J
 */
using UnityEngine;
using UnityEditor;
using System.Collections;
 
public class ReplaceSelection : ScriptableWizard
{
    static GameObject replacement = null;
    static bool keep = false;
 
    public GameObject ReplacementObject = null;
    public bool KeepOriginals = false;
 
    [MenuItem("GameObject/-Replace Selection...")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard(
            "Replace Selection", typeof(ReplaceSelection), "Replace");
    }
 
    public ReplaceSelection()
    {
        ReplacementObject = replacement;
        KeepOriginals = keep;
    }
 
    void OnWizardUpdate()
    {
        replacement = ReplacementObject;
        keep = KeepOriginals;
    }
 
    void OnWizardCreate()
    {
        if (replacement == null)
            return;
 
        Undo.RegisterCreatedObjectUndo(replacement, "Replace Selection");
 
        Transform[] transforms = Selection.GetTransforms(
            SelectionMode.TopLevel | SelectionMode.Editable);
 
        foreach (Transform t in transforms)
        {
            GameObject g;
            PrefabAssetType pref = PrefabUtility.GetPrefabAssetType(replacement);
 
            if (pref != PrefabAssetType.NotAPrefab)
            {
                g = (GameObject)PrefabUtility.InstantiatePrefab(replacement);
            }
            else
            {
                g = (GameObject)Editor.Instantiate(replacement);
            }
 
            Transform gTransform = g.transform;
            gTransform.parent = t.parent;
            g.name = replacement.name;
            gTransform.localPosition = t.localPosition;
            gTransform.localScale = t.localScale;
            gTransform.localRotation = t.localRotation;
        }
 
        if (!keep)
        {
            foreach (GameObject g in Selection.gameObjects)
            {
                GameObject.DestroyImmediate(g);
            }
        }
    }
}