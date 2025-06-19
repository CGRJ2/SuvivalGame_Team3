using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class BlenderStyleViewKeypad
{
    static BlenderStyleViewKeypad()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        if (e.type != EventType.KeyDown)
            return;

        switch (e.keyCode)
        {
            case KeyCode.Keypad8:
                SetView(Quaternion.Euler(90, 0, 0), sceneView);
                break;
            case KeyCode.Keypad2:
                SetView(Quaternion.Euler(-90, 0, 0), sceneView);
                break;
            case KeyCode.Keypad4:
                if (IsExactSideView(sceneView.rotation))
                    RotateView(sceneView, -90);
                else
                    SetView(Quaternion.Euler(0, 90, 0), sceneView);
                break;
            case KeyCode.Keypad6:
                if (IsExactSideView(sceneView.rotation))
                    RotateView(sceneView, 90);
                else
                    SetView(Quaternion.Euler(0, -90, 0), sceneView);
                break;
            case KeyCode.Keypad0:
                SetView(Quaternion.Euler(0, 0, 0), sceneView);
                break;
            case KeyCode.Keypad5:
                sceneView.orthographic = !sceneView.orthographic;
                sceneView.Repaint();
                break;
            case KeyCode.Keypad1: 
                SetView(Quaternion.Euler(30, 45, 0), sceneView);
                break;
            case KeyCode.Keypad3:
                SetView(Quaternion.Euler(30, -45, 0), sceneView);
                break;
            case KeyCode.Keypad9:
                SetView(Quaternion.Euler(30, -135, 0), sceneView);
                break;
            case KeyCode.Keypad7:
                SetView(Quaternion.Euler(30, 135, 0), sceneView);
                break;
        }
    }

    static void SetView(Quaternion rotation, SceneView sceneView)
    {
        sceneView.rotation = rotation;
        sceneView.orthographic = true; // iso 모드 강제 적용
        sceneView.LookAt(sceneView.pivot);
        sceneView.Repaint();
    }

    static void RotateView(SceneView sceneView, float angle)
    {
        sceneView.rotation *= Quaternion.Euler(0, angle, 0);
        sceneView.LookAt(sceneView.pivot);
        sceneView.Repaint();
    }

    static bool IsExactSideView(Quaternion rotation)
    {
        // Y축만 비교 (0, 90, 180, 270 근처인지 확인)
        float y = rotation.eulerAngles.y % 360f;
        float x = rotation.eulerAngles.x % 360f;

        return Mathf.Abs(x) < 1f && (
            Mathf.Abs(y - 0f) < 1f ||
            Mathf.Abs(y - 90f) < 1f ||
            Mathf.Abs(y - 180f) < 1f ||
            Mathf.Abs(y - 270f) < 1f
        );
    }
}