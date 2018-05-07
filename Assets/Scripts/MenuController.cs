using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

	public void OpenScene(string sceneName)
    {
        Application.LoadLevel(sceneName);
    }

    public void EndGame()
    {
        Application.Quit();
    }

}
