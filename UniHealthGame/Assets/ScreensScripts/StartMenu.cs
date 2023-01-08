using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    private GameObject helpScreen;

    private void Start()
    {
        helpScreen= GameObject.Find("PanelHelp");
        helpScreen.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            helpScreen.SetActive(false);
        }
    }

    public void startGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void loadHelp()
    {
        helpScreen.SetActive(true);
    }


}
