using UnityEngine;

public class HUD_UIGroup : MonoBehaviour
{
    public HUD_Time HUD_Time;

    private void Awake() => Init();

    public void Init()
    {
        UIManager.Instance.hudGroup = this;
    }
}

