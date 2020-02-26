using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDragonfly : Enemy
{
    public GameObject net;
    float netTime;
    float netSpawnTime;
    float netInterval;

    float fireAngle = 2.5f;
    float bulletsFired3 = 6;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        health = 15;
        maxHealth = health;
        bossTime = 0;
        percentage = 1;
        fireTime = 2;
        interval = 1f;
        fireAngle = 1.5f;
        bulletsFired3 = 4;
        Debug.Log("health " + health + " max health " + maxHealth);

        netTime = 0f;
        netSpawnTime = 2f;
        netInterval = 4;

        gm.highScore = PlayerPrefs.GetInt(gm.stage.ToString());
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if(barNum > 3)
        {
            fireAngle = 2.5f;
            bulletsFired3 = 6;
        }
        if(barNum > 2)
        {
            netInterval = 3;
        }
        if (barNum > 1)
        {
            PhaseTwoAction();
        }
        if (barNum > 0)
        {
            LeftRight();
        }
        
        if (bossTime >= fireTime)
        {
            Fire();
            fireTime += interval;
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
        netTime += Time.deltaTime;
        if (netTime >= netSpawnTime)
        {
            Debug.Log("netnet");
            GameObject _obstacle = Instantiate(net, GameObject.Find("GameManager").transform);
            _obstacle.transform.position = spawnSpot[(int)Random.Range(0, 5)];
            netSpawnTime += netInterval;
        }
    }

    public override void Fire()
    {
        int ranPattern = Random.Range(1, 4);
        float angle = Random.Range(20, 30);
        float minus = 0;
        float numOfBullets = 0;
        if (ranPattern == 1)
        {
            minus = angle / fireAngle;
            numOfBullets = bulletsFired3;
        }
        else
        {
            minus = angle / 2;
            numOfBullets = 5;
        }

        for (int i = 0; i < numOfBullets; i++)
        {
            GameObject _bullet = Instantiate(bullet);
            _bullet.transform.position = firePos.transform.position;
            _bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            angle -= minus;
        }
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
