using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    public void loadScene(string name) {
        SceneManager.LoadScene(name);
    }

    public void exit() {
        Application.Quit();
    }
}