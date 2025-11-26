using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class UnusedScriptScanner : EditorWindow
{
    private Vector2 scrollPos;
    private List<MonoScript> unusedScripts = new List<MonoScript>();
    private bool includeAddressables = false;

    [MenuItem("Tools/Unused Script Scanner")]
    static void OpenWindow()
    {
        UnusedScriptScanner window = GetWindow<UnusedScriptScanner>("Unused Script Scanner");
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Unused Script Scanner", EditorStyles.boldLabel);

        includeAddressables = EditorGUILayout.Toggle("Include Addressables", includeAddressables);

        if (GUILayout.Button("Scan Project", GUILayout.Height(30)))
        {
            ScanScripts();
        }

        GUILayout.Space(10);

        if (unusedScripts.Count > 0)
        {
            GUILayout.Label($"Found {unusedScripts.Count} unused scripts:", EditorStyles.boldLabel);

            scrollPos = GUILayout.BeginScrollView(scrollPos);

            foreach (var script in unusedScripts)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.ObjectField(script, typeof(MonoScript), false);

                if (GUILayout.Button("Locate", GUILayout.Width(60)))
                {
                    EditorGUIUtility.PingObject(script);
                }

                EditorGUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();
        }
        else
        {
            GUILayout.Label("No unused scripts found.");
        }
    }

    private void ScanScripts()
    {
        unusedScripts.Clear();

        // 1) Assets 폴더 안의 스크립트만 검색
        var scriptGuids = AssetDatabase.FindAssets("t:MonoScript", new[] { "Assets" });
        var allScripts = scriptGuids
            .Select(guid => AssetDatabase.LoadAssetAtPath<MonoScript>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(s => s != null)
            .ToList();

        // Editor 폴더 제외
        allScripts = allScripts
            .Where(s => !AssetDatabase.GetAssetPath(s).Contains("/Editor/"))
            .ToList();

        // abstract 클래스 등은 제외
        allScripts = allScripts.Where(s =>
        {
            var type = s.GetClass();
            return type != null && !type.IsAbstract;
        }).ToList();

        // 2) 프로젝트 내에서 실제로 사용 중인 타입 수집
        var referencedTypes = new HashSet<System.Type>();

        // Prefabs (Assets 폴더만)
        var prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" });
        foreach (var guid in prefabGuids)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid));
            if (prefab == null) continue;

            foreach (var comp in prefab.GetComponentsInChildren<MonoBehaviour>(true))
            {
                if (comp != null)
                    referencedTypes.Add(comp.GetType());
            }
        }

        // ScriptableObjects (Assets 폴더만)
        var soGuids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { "Assets" });
        foreach (var guid in soGuids)
        {
            var so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(guid));
            if (so != null)
                referencedTypes.Add(so.GetType());
        }

        // Scenes (Assets 폴더만) – 패키지 씬은 건너뜀
        var sceneGuids = AssetDatabase.FindAssets("t:Scene", new[] { "Assets" });
        for (int i = 0; i < sceneGuids.Length; i++)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(sceneGuids[i]);

            // 혹시 모를 Packages 경로는 방어적으로 무시
            if (scenePath.StartsWith("Packages/"))
                continue;

            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);

            foreach (var root in scene.GetRootGameObjects())
            {
                foreach (var comp in root.GetComponentsInChildren<MonoBehaviour>(true))
                {
                    if (comp != null)
                        referencedTypes.Add(comp.GetType());
                }
            }

            // true = 변경사항 저장 없이 닫기
            EditorSceneManager.CloseScene(scene, true);
        }

        // (선택) Addressables 폴더를 따로 두고 있다면 여기에 경로 맞게 바꿔 써도 됨
#if UNITY_ADDRESSABLES
        if (includeAddressables)
        {
            // 예시: Addressables 관련 프리팹이 Assets/Addressables 아래에 있을 때
            string[] addressablePrefabs = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Addressables" });
            foreach (var guid in addressablePrefabs)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid));
                if (prefab != null)
                {
                    foreach (var comp in prefab.GetComponentsInChildren<MonoBehaviour>(true))
                    {
                        if (comp != null)
                            referencedTypes.Add(comp.GetType());
                    }
                }
            }
        }
#endif

        // 3) 참조되지 않은 스크립트만 걸러내기
        foreach (var script in allScripts)
        {
            var type = script.GetClass();
            if (type == null) continue;

            if (!referencedTypes.Contains(type))
                unusedScripts.Add(script);
        }

        Debug.Log($"[Unused Script Scanner] Found {unusedScripts.Count} unused scripts.");
    }
}
