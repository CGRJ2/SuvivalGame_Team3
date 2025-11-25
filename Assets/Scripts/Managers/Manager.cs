using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public static class Manager
{
    public static GameManager game => GameManager.Instance;
    public static DataManager data => DataManager.Instance;
    public static PlayerManager player => PlayerManager.Instance;
    public static UIManager ui => UIManager.Instance;
    public static CameraManager camera => CameraManager.Instance;
    public static SuvivalSystemManager survivalSystem => SuvivalSystemManager.Instance;
    public static BaseCampManager baseCamp => BaseCampManager.Instance;
    public static DailyManager dayTime => DailyManager.Instance;
    public static StageManager stage => StageManager.Instance;



    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initailize()
    {
        GameManager.CreateInstance();
        DataManager.CreateInstance();
        PlayerManager.CreateInstance();
        CameraManager.CreateInstance();
        SuvivalSystemManager.CreateInstance();
        BaseCampManager.CreateInstance();
        DailyManager.CreateInstance();
        StageManager.CreateInstance();
        UIManager.CreateInstance();

    }
}
