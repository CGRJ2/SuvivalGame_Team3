using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEscape : MonoBehaviour
{
    public static UIEscape Instance;

    private Stack<GameObject> uiStack = new Stack<GameObject>();

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && uiStack.Count > 0)
        {
            var topPanel = uiStack.Peek();
            if (topPanel.activeSelf)
            {
                topPanel.SetActive(false);
                uiStack.Pop();
            }
        }
    }
    public void OpenPanel(GameObject panel)
    {
        if (!uiStack.Contains(panel))
        {
            panel.SetActive(true);
            uiStack.Push(panel);
        }
    }
    public void ClosePanel(GameObject panel) 
    {
        if (uiStack.Count > 0 && uiStack.Peek() == panel)
        { 
            panel.SetActive(false);
            uiStack.Pop();
        }
        else 
        { 
            panel.SetActive(false);
            var tmpStack = new Stack<GameObject>();
            while (uiStack.Count > 0)
            {
                var p = uiStack.Pop();
                if (p != null)
                {
                    tmpStack.Push(p);
                }
                else break;
            }
            while (tmpStack.Count > 0) 
            { 
                uiStack.Push(tmpStack.Pop());
            }
        }
    }
}
