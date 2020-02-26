using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Popup : MonoBehaviour
{
    GameManager gm;
    Enemy enemy;
    public Text wonText;
    public Text score;
    public Text highScore;
    public Text time;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        enemy = GameObject.FindGameObjectWithTag("boss").GetComponent<Enemy>();
        //wonText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        score.text = gm.score.ToString();
        highScore.text = gm.highScore.ToString();
        time.text = enemy.bossTime.ToString("#.00");
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
