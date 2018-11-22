using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour {

    public string nextScene;

	private void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.tag == "Player")
        {
            // END
            Debug.Log("END");
            SceneManager.LoadScene(nextScene);
        }
	}

}
