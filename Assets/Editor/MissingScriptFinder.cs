using UnityEditor;
using UnityEngine;

public class MissingScriptFinder
{
    [MenuItem("Tools/Find Missing Scripts In Scene")]
    private static void FindMissingInScene()
    {
        var allGameObjects = Object.FindObjectsOfType<GameObject>(true); // 비활성 포함

        foreach (var go in allGameObjects)
        {
            var comps = go.GetComponents<Component>();
            for (int i = 0; i < comps.Length; i++)
            {
                if (comps[i] == null)
                {
                    Debug.LogWarning(
                        $"[MissingScript] {GetHierarchyPath(go)} 에서 Missing Script 발견 (index {i})",
                        go);
                }
            }
        }
    }

    private static string GetHierarchyPath(GameObject obj)
    {
        string path = obj.name;
        Transform t = obj.transform.parent;

        while (t != null)
        {
            path = t.name + "/" + path;
            t = t.parent;
        }

        return path;
    }
}
