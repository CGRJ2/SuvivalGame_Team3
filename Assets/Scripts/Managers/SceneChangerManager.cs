using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerManager : Singleton<SceneChangerManager>
{
    // ����ȯ UI �������� �������� �����صθ�, Ư�� ���� ���� �� ��ȯ ȿ���� �ٸ��� �������� �� ����
    //sceneChangeUI_Prefab_Bonus; ==> Ȳ������ ���ĳ��� �� ��ȯ
    //sceneChangeUI_Prefab_BossStage; ==> Danger!! ������ �� ��ȯ
    //sceneChangeUI_Prefab_PlayerDead; ==> ������ ���̵�ƿ� ���

    [SerializeField] private List<GameObject> TransSceneEffect_Prefab;
    [SerializeField] private int index;
    private TransitionSceneEffect TransSceneEffect_Instance;



    public void Init()
    {
        base.SingletonInit();
        FadeTransitionSelect(index);
    }

  

    public void FadeTransitionSelect(int index)
    {
        // ���� �ν��Ͻ� �����ϴ��� üũ
        if (TransSceneEffect_Instance != null)
        {
            // �����Ѵٸ�? ���� �ν��Ͻ��� ���� ǥ���� UI������� ������ üũ
            if (TransSceneEffect_Instance != TransSceneEffect_Prefab[index])
            {
                // ���� ���� �ٸ� UI���̵带 ���� ���� �� �ı� �� ����
                Destroy(TransSceneEffect_Instance.gameObject);
                TransSceneEffect_Instance = null;

                TransSceneEffect_Instance = Instantiate(TransSceneEffect_Prefab[index]).GetComponent<TransitionSceneEffect>();
                DontDestroyOnLoad(TransSceneEffect_Instance.gameObject);
            }
        }
        else
        {
            TransSceneEffect_Instance = Instantiate(TransSceneEffect_Prefab[index]).GetComponent<TransitionSceneEffect>();
            DontDestroyOnLoad(TransSceneEffect_Instance.gameObject);
        }
    }


    public void ChangeScene(string sceneName)
    {
        Debug.Log($"{sceneName}���� �̵� �õ�.");
        TransSceneEffect_Instance.StartLoading(sceneName);
    }

    public void MoveToFirstStage()
    {
        TransSceneEffect_Instance.StartLoading(StageManager.Instance.stageDatas[0].StageName);
    }
}
