using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public Button retryButton;
    public Button quitButton;

    // Start is called before the first frame update
    void Start()
    {
        Button b1 = retryButton.GetComponent<Button>();
        b1.onClick.AddListener(Retry);

        Button b2 = quitButton.GetComponent<Button>();
        b2.onClick.AddListener(Quit);
    }

    void Retry() {
        SceneManager.LoadScene("MainScene");
    }

    void Quit() {
        Application.Quit();
    }
    
}
