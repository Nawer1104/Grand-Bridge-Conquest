using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button roadButton;
    public Button tileButton;
    public BarCreator barCreator;

    public void Play()
    {
        Time.timeScale = 1;

        BarCreator.Instance.barCreationStarted = false;
        BarCreator.Instance.DeleteCurrentBar();
        BarCreator.Instance.gameStart = true;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
