using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool gameStart;
    public int stage;
    public float stageTime;
    public int score;
    public int highScore;
    public string currentScore;
    string gameId;
    public int adCountDown;
    //spawning
    GameObject[] bosses;
    public GameObject player;
    GameObject _player;
    public GameObject dragonfly, mantis, spider;

    public int spawned = 0;

    Vector3 left = new Vector3(-3, 2, -1);
    Vector3 right = new Vector3(3, 2, -1);
    Vector3[] twoPos;

    //UI
    public GameObject background1, background2;
    public Sprite[] bgImages;
    public Sprite bg1, bg2, bg3;
    public Text hs1, hs2, hs3;
    public GameObject startButton;
    public GameObject bombPrefab;
    public GameObject bombText;
    public GameObject bombButton;
    public int bombLeft;
    
    public GameObject you;
    DragonFlyMove dfm;

    //Main Menu
    public Camera startCam, gameCam;
    public Canvas startCan, gameCan;
    bool inGame = false;

    // Start is called before the first frame update
    void Start()
    {
        gameStart = false;

        stage = 0;

        currentScore = "";
        gameId = "3464115";
        Advertisement.Initialize(gameId, true);
        adCountDown = PlayerPrefs.GetInt("countDown",2);
        bosses = new GameObject[3];
        bosses[0] = dragonfly;
        bosses[1] = mantis;
        bosses[2] = spider;

        dfm = player.GetComponent<DragonFlyMove>();
        twoPos = new Vector3[2];
        twoPos[0] = right;
        twoPos[1] = left;
    
        //UI
        bgImages = new Sprite[3];
        bgImages[0] = bg1; bgImages[1] = bg2; bgImages[2] = bg3;
        hs1.text = PlayerPrefs.GetInt("0").ToString() ;
        hs2.text = PlayerPrefs.GetInt("1").ToString();
        hs3.text = PlayerPrefs.GetInt("2").ToString();


        bombLeft = 3;
        bombText.GetComponent<Text>().text = bombLeft.ToString();
        //Main Menu
        gameCam.enabled = false;
        CameraShift();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStart)
        {
            Time.timeScale = 0;

        }

        if (gameStart == true)
        {
            Time.timeScale = 1;
            StartGameUI();
            stageTime += Time.deltaTime;
        }
        BackOrExitGame();

    }

    void StartGameUI()
    {
        //UI
        startButton.SetActive(false);
    }
    void EndGameUI()
    {
        //UI
        startButton.SetActive(true);
    }

    public bool once = false;
    void Boss()
    {
        GameObject _boss = Instantiate(bosses[stage], GameObject.Find("GameManager").transform);
        _boss.transform.position = new Vector3(0, 4.5f, -1);
        _boss.transform.rotation = Quaternion.Euler(new Vector3(180, 0, 0));
        _player = Instantiate(player, GameObject.Find("GameManager").transform);
        _player.transform.position = new Vector3(0, -2.4f, -1);
        bombButton.SetActive(true);
    }

    public void UseBomb()
    {
        if (bombLeft > 0)
        {
            GameObject _bomb = Instantiate(bombPrefab, GameObject.Find("GameManager").transform);
            _bomb.transform.position = _player.transform.position;
            bombLeft--;
            bombText.GetComponent<Text>().text = bombLeft.ToString();
        }
    }
    
    public void StartGame()
    {
        gameStart = true;
    }
    public bool GameStart
    {
        get { return gameStart; }
        set { gameStart = value; }
    }

    //Main menu
    public void FightDragonfly()
    {
        stage = 0;
        background1.GetComponent<SpriteRenderer>().sprite = bgImages[stage];
        background2.GetComponent<SpriteRenderer>().sprite = bgImages[stage];
        inGame = true;
        CameraShift();
        Boss();

    }
    public void FightMantis()
    {
        stage = 1;
        background1.GetComponent<SpriteRenderer>().sprite = bgImages[stage];
        background2.GetComponent<SpriteRenderer>().sprite = bgImages[stage];
        inGame = true;
        CameraShift();
        Boss();

    }
    public void FightSpider()
    {
        stage = 2;
        background1.GetComponent<SpriteRenderer>().sprite = bgImages[stage];
        background2.GetComponent<SpriteRenderer>().sprite = bgImages[stage];
        inGame = true;
        CameraShift();
        Boss();

    }
    void CameraShift()
    {
        if (inGame)
        {
            startCam.enabled = false;
            gameCam.enabled = true;
            startCan.enabled = false;
            gameCan.enabled = true;
        }
        else
        {
            startCam.enabled = true;
            gameCam.enabled = false;
            startCan.enabled = true;
            gameCan.enabled = false;
        }
    }

    public void ShowAds()
    {
        Advertisement.Show();
    }

    void BackOrExitGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(inGame == true)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                Application.Quit();
            }
        }
    }

    public void PrivacyPolicy()
    {
        Application.OpenURL("http://khwan789.github.io/privacy_policy/vs_privacy_policy.html");
    }
}
