using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string uiSceneName = "UIScene";

    void Start()
    {
        if (!SceneManager.GetSceneByName(uiSceneName).isLoaded)
        {
            SceneManager.LoadSceneAsync(uiSceneName, LoadSceneMode.Additive);
        }
    }
}