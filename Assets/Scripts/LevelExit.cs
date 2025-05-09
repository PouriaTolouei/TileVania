using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
 
    [SerializeField] float levelLoadingTime = 1f;
   
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            StartCoroutine(LoadNextLevel());
        }
    }

    IEnumerator LoadNextLevel() {
        yield return new WaitForSecondsRealtime(levelLoadingTime);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) {
            nextSceneIndex = 0;
            FindFirstObjectByType<GameSession>().ResetPlayerScore();
        }
        FindFirstObjectByType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene((nextSceneIndex) % SceneManager.sceneCountInBuildSettings);
    }
}
