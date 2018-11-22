using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class Exit : MonoBehaviour {

    public string nextScene;
    public bool EndGame;
    public float Delay;
	private void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.tag == "Player")
        {
            // END
            Debug.Log("END");
            if (EndGame) {
                FindObjectOfType<PlayerManager>().GameIsWon();
                StartCoroutine(DelayTransition());
            } else {
                SceneManager.LoadScene(nextScene);
            }
        }
	}

    IEnumerator DelayTransition() {
        yield return new WaitForSeconds(Delay);
        SceneManager.LoadScene(nextScene);
    }
}
