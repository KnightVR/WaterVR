using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMangerController : MonoBehaviour
{
    public string[] scenes;
    public OVRInput.Controller controller;
    public GameObject[] VR_Only_Objects;
    public GameObject[] Windows_Only_Objects;

    private Scene current_scene;
    private int current_scene_idx;
    private bool lastFrame_ButtonOne_State = false; //Store OVR Button One state as GetDown is currently broken
    private bool lastFrame_ButtonTwo_State = false; //Store OVR Button One state as GetDown is currently broken
    private bool lastFrame_ButtonMiddle_State = false; //Store OVR Button One state as GetDown is currently broken

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        //Enable VR only game objects
        foreach (GameObject obj in VR_Only_Objects)
        {
            obj.SetActive(true);
        }
        foreach (GameObject obj in Windows_Only_Objects)
        {
            obj.SetActive(false);
        }
#endif
#if (UNITY_STANDALONE || UNITY_EDITOR)
        //Enable Windows only game objects
        foreach (GameObject obj in VR_Only_Objects)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in Windows_Only_Objects)
        {
            obj.SetActive(true);
        }
#endif
        LoadInintalScene();
    }

    void Update()
    {
        bool loadNextScene = false;
        bool loadPrevScene = false;
        bool loadReloadScene = false;

        OVRInput.Update();
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("Right arrow key was pressed.");
            loadNextScene = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("Left arrow key was pressed.");
            loadPrevScene = true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space key was pressed.");
            loadReloadScene = true;
        }

        // Manually checking for keydown event on OVR Buttons as GetDown is currently broken
        bool currentFrame_ButtonOne_State = OVRInput.Get(OVRInput.Button.One, controller);
        bool currentFrame_ButtonTwo_State = OVRInput.Get(OVRInput.Button.Two, controller);
        bool currentFrame_ButtonMiddle_State = OVRInput.Get(OVRInput.Button.PrimaryThumbstick, controller);
        if (currentFrame_ButtonOne_State && !lastFrame_ButtonOne_State)
        {   
            // Change scene on Button One press
            Debug.Log("OVR Button One was pressed.");
            loadPrevScene = true;
        }
        if (currentFrame_ButtonTwo_State && !lastFrame_ButtonTwo_State)
        {
            // Change scene on Button One press
            Debug.Log("OVR Button Two was pressed.");
            loadNextScene = true;
        }
        if (currentFrame_ButtonMiddle_State && !lastFrame_ButtonMiddle_State)
        {
            // Change scene on Button One press
            Debug.Log("OVR Button Middle was pressed.");
            loadReloadScene = true;
        }
        lastFrame_ButtonOne_State = currentFrame_ButtonOne_State;
        lastFrame_ButtonTwo_State = currentFrame_ButtonTwo_State;
        lastFrame_ButtonMiddle_State = currentFrame_ButtonMiddle_State;

        if (loadNextScene)
        {
            NextScene();
        }
        else if (loadPrevScene)
        {
            PreviousScene();
        }
        else if (loadReloadScene)
        {
            ReloadScene();
        }
    }
    private void LoadInintalScene()
    {
        LoadScene(0);
    }

    private void PreviousScene()
    {
        int prev_idx = current_scene_idx - 1;
        if (prev_idx < 0){
            prev_idx = scenes.Length - 1;
        }
        LoadScene(prev_idx);
    }

    private void NextScene()
    {
        int next_idx = current_scene_idx + 1;
        if (next_idx >= scenes.Length)
        {
            next_idx = 0;
        }
        LoadScene(next_idx);
    }

    private void ReloadScene()
    {
        LoadScene(current_scene_idx);
    }

    private void LoadScene(int scene_idx,bool async=false)
    {
        current_scene_idx = scene_idx;

        // Load next scene in background
        string loadingScene = scenes[scene_idx % scenes.Length];
        if (async)
        {
            StartCoroutine(LoadYourAsyncScene(loadingScene, LoadSceneMode.Additive));
        } else
        {
            SceneManager.LoadSceneAsync(loadingScene, LoadSceneMode.Additive);
        }
    }

    IEnumerator LoadYourAsyncScene(string sceneName, LoadSceneMode sceneMode)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, sceneMode);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode != LoadSceneMode.Additive)
            return;

        SceneManager.SetActiveScene(scene);

        DisableOldScene();

        current_scene = scene;
    }

    private void OnSceneUnloaded(Scene scene)
    {

    }

    private void DisableOldScene()
    {
        if (current_scene.IsValid())
        {
            GameObject[] oldSceneObjects = current_scene.GetRootGameObjects();
            for (int i = 0; i < oldSceneObjects.Length; i++)
            {
                oldSceneObjects[i].SetActive(false);
            }

            SceneManager.UnloadSceneAsync(current_scene);
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

}
