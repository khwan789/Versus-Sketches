using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpider : Enemy
{
    public GameObject web;
    float webTime;
    float webSpawnTime;
    float webInterval;
    int numBullets;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        health = 15;
        maxHealth = health;
        bossTime = 0;
        percentage = 1;
        fireTime = 0;
        interval = 1f;
        trapAngle = 20;
        down = true;
        numBullets = 2;
        Debug.Log("health " + health + " max health " + maxHealth);
        gm.highScore = PlayerPrefs.GetInt(gm.stage.ToString());
        webTime = 0f;
        webSpawnTime = 2f;
        webInterval = 5;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (bossTime >= fireTime)
        {
            Fire();
            fireTime += interval;
        }

        if (barNum > 0)
        {
            LeftRight();
        }
        if (barNum > 1)
        {
            numBullets = 3;
        }
        if (barNum > 2)
        {
            PhaseTwoAction();
        }
        if (barNum > 3)
        {
            webInterval = 3;
        }

    }

    public override void BossMoveDown()
    {
        base.BossMoveDown();
    }

    public override void HealthBar()
    {
        base.HealthBar();
    }

    public override void PhaseTwoAction()
    {
        webTime += Time.deltaTime;
        if (webTime >= webSpawnTime)
        {
            GameObject _web = Instantiate(web, GameObject.Find("GameManager").transform);
            _web.transform.position = spawnSpot[(int)Random.Range(0, 5)];
            webSpawnTime += webInterval;
        }
    }

    public override void Fire()
    {
        float nowAngle = trapAngle + 7;

        for (int i = 0; i < numBullets; i++)
        {
            GameObject _bullet = Instantiate(bullet);
            _bullet.transform.position = firePos.transform.position;
            _bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, nowAngle));
            nowAngle -= 10;
        }

        if (down)
        {
            trapAngle -= 8;
        }
        else
        {
            trapAngle += 8;
        }

        if (trapAngle >= 20) { down = true; }
        if (trapAngle <= -20) { down = false; }
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
