using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScript : MonoBehaviour
{
    // public static LoadingScript Instance;

    [SerializeField] private Image progressbar;

    public void LoadScene(string scenename)
    {
        StartCoroutine(LoadScene_Coroutine(scenename));
        //PlayerPrefs.DeleteAll();
    }

    public IEnumerator LoadScene_Coroutine(string name)
    {
        progressbar.fillAmount = 0;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);
        asyncOperation.allowSceneActivation = false;
        float progress = 0;
        while (!asyncOperation.isDone)
        {
            progress = Mathf.MoveTowards(progress, asyncOperation.progress, Time.deltaTime);
            progressbar.fillAmount = progress;
            if (progress >= 0.9f)
            {
                progressbar.fillAmount = 1;
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}