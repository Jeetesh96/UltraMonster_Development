using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets; 
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class Loading : MonoBehaviour
{
    private AsyncOperationHandle<SceneInstance> SceneHandle;

    [SerializeField]
    private AssetReference AddressableScene;
    
    [SerializeField]
    private Slider LoadingSlider;

    [SerializeField]
    private Text LoadingText;

    void Start()
    {
        SceneHandle = Addressables.LoadSceneAsync(AddressableScene, UnityEngine.SceneManagement.LoadSceneMode.Single, false);  
        SceneHandle.Completed += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneHandle.Completed -= OnSceneLoaded;
    }

    private void OnSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
    {
        if(obj.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Success");
            //Addressables.LoadSceneAsync(AddressableScene, UnityEngine.SceneManagement.LoadSceneMode.Single, true);
        }
    }

    void Update()
    {
        float percent = SceneHandle.PercentComplete*100;
        LoadingSlider.value = percent;
        LoadingText.text = "Loading Scene "+ percent + "%".ToString();
        
        if (SceneHandle.PercentComplete == 1)
        {
            LoadingText.text = "Please Wait ... ";
            SceneHandle.Result.ActivateAsync(); 
        }
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("HomeScene");

    }
}
