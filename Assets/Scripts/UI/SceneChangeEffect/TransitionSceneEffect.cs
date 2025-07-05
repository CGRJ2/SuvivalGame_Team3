using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public abstract class TransitionSceneEffect : MonoBehaviour
{
    [SerializeField] public TMP_Text loadingText;
    // Start is called before the first frame update

    protected Coroutine loadingRoutine;

    public void StartLoading(string sceneName)
    {
        if (loadingRoutine == null)
        {
            loadingRoutine = StartCoroutine(TransitionPattern(sceneName));
        }
        else
        {
            StopCoroutine(loadingRoutine);
            loadingRoutine = StartCoroutine(TransitionPattern(sceneName));
        }
    }

    protected void OnDestroy()
    {
        if (loadingRoutine != null) StopCoroutine(loadingRoutine);
    }

    public abstract IEnumerator TransitionPattern(string sceneName);
}
