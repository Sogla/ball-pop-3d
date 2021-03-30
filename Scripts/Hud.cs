using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public Text win_loseText;
    public Text levelText;
    public GameObject[] victoryButtons;
    GameObject lvm;
    LevelManager lm;
    void Start()
    {
        levelText.text = "LEVEL " + (PlayerPrefs.GetInt("level") + 1);
        lvm = GameObject.FindGameObjectWithTag("GameController");
        lm = lvm.GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lm.state == LevelManager.GameState.Victory)
        {
            win_loseText.gameObject.SetActive(true);
            win_loseText.text = "WIN";
            victoryButtons[0].SetActive(true);
            victoryButtons[1].SetActive(true);
        }
        else if (lm.state == LevelManager.GameState.Failure)
        {
            win_loseText.gameObject.SetActive(true);
            win_loseText.text = "LOSE";
            victoryButtons[0].SetActive(true);
        }
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Next()
    {
        int i = PlayerPrefs.GetInt("level");
        PlayerPrefs.SetInt("level", i + 1);
        Replay();
    }
}
