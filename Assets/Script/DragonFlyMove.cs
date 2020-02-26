using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragonFlyMove : MonoBehaviour
{
    GameManager gm;
    Enemy enemyBoss;
    private Vector3 mousePosi;
    public float moveSpeed = 2f;
    public GameObject bullet, beellet;
    public GameObject pencilButton;
    public GameObject bulletPos, bulletPos2, missilePos, hurt;
    public GameObject[] lives;
    public int life = 3;
    float interval = 0.3f;
    float nextTime = 0;
    bool fireSecond;
    public GameObject itemText;
    public GameObject popup;
    public float playerTime;
    bool invincible = false;

    List<int> cases;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        itemText = GameObject.Find("itemEat");
        mousePosi = transform.position;

        fireSecond = false;

        lives = new GameObject[3];
        for (int i = 0; i < life; i++)
        {
            lives[i] = GameObject.Find("life" + (i + 1));
        }

        PlayerHealth();
        hurt.SetActive(false);
        itemText.SetActive(false);
        itemText.transform.position = new Vector3(0.5f, -1, 0);
        cases = new List<int>
        {
            1,
            2,
            3,
            4,
            5,
            6
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.GameStart == true)
        {
            playerTime += Time.deltaTime;

            enemyBoss = GameObject.FindGameObjectWithTag("boss").GetComponent<Enemy>();

            if (Input.GetMouseButton(0))
            {
                if (Input.touchCount > 1)
                {
                }
                else
                {
                    mousePosi = Input.mousePosition;
                    mousePosi = Camera.main.ScreenToWorldPoint(mousePosi);
                    if (mousePosi.y <= -0.8)
                    {
                        this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(mousePosi.x, mousePosi.y + 0.8f, -1), moveSpeed);
                    }
                }
            }
            if (playerTime >= nextTime)
            {
                FireBullet(bulletPos.transform.position);
                if (fireSecond)
                {
                    FireBullet(bulletPos2.transform.position);
                }
                nextTime += interval;
            }
        }
    }
    void FireBullet(Vector3 bulletPosition)
    {
        GameObject _bullet = Instantiate(bullet, bulletPosition, Quaternion.identity);
    }

    void ItemEffect()
    {
        gm.score += 35;
        if (life < 3 && !cases.Contains(3)) { cases.Add(3); } else { cases.Remove(3); }
        if (gm.bombLeft < 4 && !cases.Contains(4)) { cases.Add(4); } else { cases.Remove(4); }
        int randomVal;

        if (cases.Count <= 1)
        {
            randomVal = 1;
        }
        else
        {
            randomVal = Random.Range(0, cases.Count - 1);
        }
        Debug.Log(randomVal + " " + cases.Count + " " + cases[1]);
        switch (cases[randomVal])
        {
            case 1:
                interval -= 0.05f;
                if (interval == 0.2) { cases.Remove(1); }
                itemText.GetComponent<Text>().text = "Speed Up!";
                Debug.Log("Speed Up " + interval);
                break;
            case 2:
                fireSecond = true;
                itemText.GetComponent<Text>().text = "Double!";
                cases.Remove(2);
                break;
            case 3:
                life++;
                itemText.GetComponent<Text>().text = "Life Up!";
                PlayerHealth();
                Debug.Log("Life Up");
                break;
            case 4:
                gm.bombLeft++;
                gm.bombText.GetComponent<Text>().text = gm.bombLeft.ToString();
                itemText.GetComponent<Text>().text = "One more bomb!";
                Debug.Log("+1 Bomb");
                break;
            case 5:
                StartCoroutine(FireMissile());
                itemText.GetComponent<Text>().text = "Fire Missile!";
                cases.Remove(5);
                break;
            case 6:
                GameObject _pencilButton = Instantiate(pencilButton, GameObject.Find("GameCanvas").transform);
                itemText.GetComponent<Text>().text = "Pencil Shield!";
                cases.Remove(6);
                break;
            default:
                break;
        }
        itemText.SetActive(true);
        itemText.GetComponent<Animator>().Play("itemTextAnim", -1, 0);

    }

    IEnumerator FireMissile()
    {
        while (true)
        {
            GameObject _missile = Instantiate(beellet, missilePos.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(3f);
        }
    }

    void PlayerHealth()
    {
        for (int i = 0; i < 3; i++)
        {
            lives[i].GetComponent<Image>().color = Color.clear;
        }

        for (int i = 0; i < life; i++)
        {
            lives[i].GetComponent<Image>().color = Color.red;
        }
    }

    void InvincibleReset()
    {
        invincible = false;
        hurt.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemyBullet")
        {
            if (!invincible)
            {
                life--;
                hurt.SetActive(true);
                PlayerHealth();
                invincible = true;
                Invoke("InvincibleReset",1f);
            }
        }
        if (collision.gameObject.tag == "web")
        {
            moveSpeed = 0.01f;
        }
        if (collision.gameObject.tag == "item")
        {
            ItemEffect();
            Destroy(collision.gameObject);
        }
        if (life < 1)
        {
            gm.score += (gm.bombLeft * 170);
            gm.score += (gm.score / (int)enemyBoss.bossTime);
            gm.GameStart = false;
            GameObject _popup = Instantiate(popup, GameObject.Find("GameCanvas").transform);
            _popup.GetComponent<Popup>().wonText.text = "You Lost :(";
            if (gm.score > gm.highScore)
            {
                gm.highScore = gm.score;
                PlayerPrefs.SetInt(gm.stage.ToString(), gm.highScore);
            }
            gm.adCountDown--;
            PlayerPrefs.SetInt("countDown", gm.adCountDown--);
            if (PlayerPrefs.GetInt("countDown") <= 0)
            {
                gm.ShowAds();
                gm.adCountDown = 2;
                PlayerPrefs.SetInt("countDown", gm.adCountDown);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "web")
        {
            moveSpeed = 2f;
        }
    }

}
