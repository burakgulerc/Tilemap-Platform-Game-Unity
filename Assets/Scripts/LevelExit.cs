using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] AudioClip nextLevelSound;

     void OnTriggerEnter2D(Collider2D collision)
     {
       if(collision.tag == "Player")
       {
            AudioSource.PlayClipAtPoint(nextLevelSound,Camera.main.transform.position);
            StartCoroutine(LoadNextLevel());
       }
     }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        // Check total scene number, nextscene index, return to first or next level

        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        FindObjectOfType<ScenePersist>().ResetScenePersist();

        SceneManager.LoadScene(nextSceneIndex);
    }

}
