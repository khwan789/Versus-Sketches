using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMantis : Enemy
{
    public GameObject cut;
    bool cutting;
    bool cutting2;
    float cutSpeed;
    //fall
    Vector3 endPos;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        cutting = false;
        cutting2 = false;
        cutSpeed = 1f;
        gm.highScore = PlayerPrefs.GetInt(gm.stage.ToString());
        health = 15;
        maxHealth = health;
        bossTime = 0;
        percentage = 1;
        fireTime = 28;
        interval = 1.5f;
        trapAngle = 23;
        down = true;
        Debug.Log("health " + health + " max health " + maxHealth);

        endPos = new Vector3(this.transform.position.x, -3f, -1f);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (!cutting)
        {
            cutting = true;
            StartCoroutine(StartCut());
            StartCoroutine(Faster());
        }
        if(barNum > 1)
        {
            if (bossTime >= fireTime)
            {
                Fire();
                fireTime += interval;
            }
        }
        if(barNum > 3 && !cutting2)
        {
            cutting2 = true;
            StartCoroutine(StartCut());
        }

        LeftRight();
    }

    public override void BossMoveDown()
    {
        base.BossMoveDown();
    }

    public override void HealthBar()
    {
        base.HealthBar();
    }

    IEnumerator StartCut()
    {
        while (cutting)
        {
            yield return new WaitForSeconds(cutSpeed);
            int i = Random.Range(0, spawnSpot.Length);
            GameObject _cut = Instantiate(cut, spawnSpot[i], Quaternion.identity);
            Debug.Log("cut speed " + cutSpeed);
        }
    }
    IEnumerator Faster()
    {
        while (cutting)
        {
            yield return new WaitForSeconds(12);
            cutSpeed -= 0.02f;
        }
    }

    public override void Fire()
    {
        GameObject _bullet = Instantiate(bullet);
        _bullet.transform.position = firePos.transform.position;
        _bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, trapAngle));

        GameObject _bullet2 = Instantiate(bullet);
        _bullet2.transform.position = firePos.transform.position;
        _bullet2.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -trapAngle));

        if (down)
        {
            trapAngle -= 5;
        }
        else
        {
            trapAngle += 5;
        }

        if (trapAngle >= 23) { down = true; }
        if (trapAngle <= -23) { down = false; }
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
