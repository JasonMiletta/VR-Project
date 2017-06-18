using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {
    
    public string[] SceneNameList;
    string currentSceneName;
    int currentSceneIndex;

	// Use this for initialization
	void Start () {
        currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        var i = 0;
        foreach(string s in SceneNameList)
        {
            if(s == currentSceneName)
            {
                currentSceneIndex = i;
                break;
            }
            ++i;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void loadNextScene()
    {
        ++currentSceneIndex;
        loadSceneAtIndex(currentSceneIndex);
    }

    public void loadPreviousScene()
    {
        --currentSceneIndex;
        loadSceneAtIndex(currentSceneIndex);
    }

    private void loadSceneAtIndex(int i)
    {
        if (currentSceneIndex > SceneNameList.Length)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(SceneNameList[0]);
            currentSceneIndex = 0;
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(SceneNameList[currentSceneIndex]);
        }
    }
}
