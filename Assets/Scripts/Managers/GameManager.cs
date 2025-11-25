using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private void Awake() => Init();

    private void Init()
    {
        base.SingletonInit();
    }
}
