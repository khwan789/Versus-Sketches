using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public float percentage;
    public float fireTime;
    public float interval;
    public int trapAngle;
    public bool down;

    public GameManager gm;
    public DragonFlyMove player;
    public GameObject popup;
    public GameObject healthBar;
    public GameObject[] bars;
    public Text barsText;
    public int numOfBarsLeft;
    public GameObject currentBar;
    public int barNum;
    public float bossTime;
    public Text timerText;
    public Vector3 bossPos;
    public Vector3 healthPos;
    public Vector3[] spawnSpot;

    public GameObject item;
    public Vector3[] itemSpot;

    int ran;
    Vector3 left, center, right;
    public GameObject bullet;
    public GameObject firePos;

    public SpriteRenderer spriteRenderer;
    public Color originalColor;

    protected virtual void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("you(Clone)").GetComponent<DragonFlyMove>();
        healthBar = GameObject.Find("HealthBar");
        bars = new GameObject[5]; //GameObject.Find("Bar");
        numOfBarsLeft = 5;
        barsText = GameObject.Find("barsLeftText").GetComponent<Text>();
        timerText = GameObject.Find("TimeText").GetComponent<Text>();
        barsText.text = "x" + numOfBarsLeft;
        barNum = 0;
        interval = 1f;
        for (int i = barNum; i < bars.Length; i++)
        {
            bars[i] = GameObject.Find("Bar" + i);
        }

        currentBar = bars[barNum];

        bossPos = new Vector3(this.transform.position.x, 2.5f, -1f);
        healthPos = new Vector3(0, 3.5f, 0f);
        spawnSpot = new Vector3[5];
        spawnSpot[0] = new Vector3(-1.8f, 4.5f, -1);
        spawnSpot[1] = new Vector3(-0.9f, 4.5f, -1);
        spawnSpot[2] = new Vector3(0, 4.5f, -1);
        spawnSpot[3] = new Vector3(0.9f, 4.5f, -1);
        spawnSpot[4] = new Vector3(1.8f, 4.5f, -1);

        itemSpot = new Vector3[2];
        itemSpot[0] = new Vector3(-0.5f, 2.5f, -1);
        itemSpot[1] = new Vector3(0.5f, 2.5f, -1);

        ran = Random.Range(0, 3);
        left = new Vector3(-1.3f, 2.5f, -1);
        center = new Vector3(0, 2.5f, -1);
        right = new Vector3(1.3f, 2.5f, -1);

        spriteRenderer = this.GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    protected virtual void Update()
    {
        bossTime += Time.deltaTime;
        BossMoveDown();
        timerText.text = bossTime.ToString("#.00");

    }

    public virtual void BossMoveDown()
    {//enemy enters screen
        float step = 1f * Time.deltaTime;
        if (bossTime <= 3)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, bossPos, step);
        }
    }

    public virtual void PhaseTwoAction()
    {//starts additional action when enemy's health goes below 65%

    }

    public virtual void Fire()
    {//enemy will fire bullets every provided time

    }

    public virtual void HealthBar()
    {//alters enemy's healthbar on screen
        float percentage = (float)health / (float)maxHealth;

        if (percentage >= 0.01f)
        {
            currentBar.transform.localScale = new Vector3(percentage, 1);
        }
        else
        {
            currentBar.transform.localScale = new Vector3(0, 1);
        }
    }

    public virtual void LeftRight()
    {
        float step = 1f * Time.deltaTime;

        if ((int)bossTime % 4 == 0)
        {
            if (ran == 0)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, left, step);
                if (this.transform.position == left) { ran = Random.Range(0, 3); }
            }
            if (ran == 1)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, center, step);
                if (this.transform.position == center) { ran = Random.Range(0, 3); }
            }
            if (ran == 2)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, right, step);
                if (this.transform.position == right) { ran = Random.Range(0, 3); }
            }
        }
    }

    public virtual void SpawnItem()
    {
        GameObject _item = Instantiate(item, GameObject.Find("GameManager").transform);
        _item.transform.position = itemSpot[(int)Random.Range(0, 2)];
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {//gets hit by player's bullet. Set up new enemy when dead
        if (collision.gameObject.tag == "bullet" || collision.gameObject.tag == "bomb")
        {
            gm.score += 15;
            health--;
            spriteRenderer.color = Color.white;
            if (health >= 0)
            {
                spriteRenderer.color = originalColor;
                percentage = (float)health / (float)maxHealth;
                HealthBar();
            }

            if (health <= 0 && barNum != 4)
            {
                gm.score += 80;
                HealthBar();
                spriteRenderer.color = Color.red;
                interval -= 0.07f;
                barNum++;
                numOfBarsLeft--;
                barsText.text = "x" + numOfBarsLeft;
                currentBar = bars[barNum];
                health = maxHealth * 2;
                maxHealth = health;
                SpawnItem();
            }
            else if (barNum == 4 && health <= 0)
            {
                gm.score += 80;
                //popup on
                gm.score += (player.life * 170);
                gm.score += (gm.bombLeft * 170);
                gm.score += (gm.score / (int)bossTime);
                gm.GameStart = false;
                GameObject _popup = Instantiate(popup, GameObject.Find("GameCanvas").transform);
                _popup.GetComponent<Popup>().wonText.text = "You Won!";
                if (gm.score > gm.highScore)
                {
                    gm.highScore = gm.score;
                    PlayerPrefs.SetInt(gm.stage.ToString(), gm.highScore);
                }
                PlayerPrefs.SetInt("countDown", gm.adCountDown--);
                if (PlayerPrefs.GetInt("countDown") <= 0)
                {
                    gm.ShowAds();
                    gm.adCountDown = 2;
                    PlayerPrefs.SetInt("countDown", gm.adCountDown);
                }
            }
        }
    }
}
