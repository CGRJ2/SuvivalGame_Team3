using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class ApplyFrictionToChildren : EditorWindow
{
    private PhysicMaterial targetMaterial;

    [MenuItem("FrictionSet/Apply Physic Material to All Child Colliders")]
    public static void ShowWindow()
    {
        GetWindow<ApplyFrictionToChildren>("Apply Friction");
    }

    private void OnGUI()
    {
        GUILayout.Label("Apply Physic Material to All Child Colliders", EditorStyles.boldLabel);
        targetMaterial = (PhysicMaterial)EditorGUILayout.ObjectField("Physic Material", targetMaterial, typeof(PhysicMaterial), false);

        if (GUILayout.Button("Apply to Selected Object"))
        {
            ApplyMaterialToSelection();
        }
    }

    private void ApplyMaterialToSelection()
    {
        GameObject selected = Selection.activeGameObject;

        if (selected == null)
        {
            Debug.LogError("������Ʈ�� �������ּ���.");
            return;
        }

        Collider[] colliders = selected.GetComponentsInChildren<Collider>(true);
        int count = 0;

        foreach (var col in colliders)
        {
            Undo.RecordObject(col, "Apply Physic Material");
            col.material = targetMaterial;  // �� �κ��� null�̾ None���� ������
            EditorUtility.SetDirty(col);
            count++;
        }

        string matName = (targetMaterial == null) ? "None (null)" : targetMaterial.name;
        Debug.Log($"�� {count}���� �ݶ��̴��� '{matName}' ���� �Ϸ�.");

        EditorSceneManager.MarkSceneDirty(selected.scene);
    }
}