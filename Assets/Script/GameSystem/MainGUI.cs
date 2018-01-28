using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGUI : MonoBehaviour {
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            transform.Find("Wrapper").gameObject.SetActive(true);
            SceneManager.UnloadSceneAsync(1);
        }	
	}

    public void StartGame()
    {
        SceneManager.LoadScene("Scene/Map_0", LoadSceneMode.Additive);
        transform.Find("Wrapper").gameObject.SetActive(false);
        GameObject.Find("GameSystem").SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
