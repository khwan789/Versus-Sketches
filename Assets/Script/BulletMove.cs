using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public bool player;
    public bool missile;
    public bool web;
    public bool cut;

    GameManager gm;
    //missile
    GameObject boss;
    Quaternion rotateToBoss;
    Vector3 dir;
    Rigidbody2D rb;

    //web
    Vector3 endPos;
    float stay = 3.5f;
    float start = 0;

    //cut
    float speed = 0;
    float maxSpeed = 10;
    float accel = 10;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (missile)
        {
            boss = GameObject.FindGameObjectWithTag("boss");
            rb = this.GetComponent<Rigidbody2D>();
        }

        endPos = new Vector3(this.transform.position.x, -2.4f, -1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            this.transform.position += transform.up * Time.deltaTime * 3;
        }
        else if (missile)
        {
            MissileAttack();
        }
        else if (cut)
        {
            CutAttack();
        }
        else if (web)
        {
            MoveDown();
            GetTime();
        }
        else
        {
            this.transform.position += -transform.up * Time.deltaTime * 3;
        }
        DestroyOutofBoundary();
    }

    void MissileAttack()
    {
        dir = (boss.transform.position - this.transform.position).normalized;
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //rotateToBoss = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.right = boss.transform.position - this.transform.position;
        rb.velocity = new Vector2(dir.x * 2, dir.y * 2);
    }

    void CutAttack()
    {
        if (speed < maxSpeed)
        {
            speed = speed + accel * Time.deltaTime;
        }
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - speed * Time.deltaTime, this.transform.position.z);
        /*this.transform.position -= transform.up * Time.deltaTime * i;
        if (i <= 5)
        {
            i += 0.05f;
        }*/
    }
    void MoveDown()
    {
        float step = 4f * Time.deltaTime;
        this.transform.position = Vector3.MoveTowards(this.transform.position, endPos, step);
    }

    void GetTime()
    {
        start += Time.deltaTime;
        if (start >= stay)
        {
            Destroy(this.gameObject);
        }
    }

    void DestroyOutofBoundary()
    {
        if (this.transform.position.y >= 5 || this.transform.position.y <= -5)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!player && !missile && collision.gameObject.tag == "bomb")
        {
            gm.score += 14;
            Destroy(this.gameObject);
        }
        if ((player || missile) && collision.gameObject.tag == "boss")
        {
            Destroy(this.gameObject);
        }
    }

}
