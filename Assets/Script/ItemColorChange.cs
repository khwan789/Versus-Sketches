using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemColorChange : MonoBehaviour
{
    public Gradient myGradient;
    public float strobeDuration = 2f;
    float left = -1.3f;
    float right = 1.3f;
    float current;
    bool moveRight;
    // Start is called before the first frame update
    void Start()
    {
        current = this.transform.position.x;
        if(Random.Range(0,2) > 0)
        {
            moveRight = false;
        }
        else { moveRight = true; }
    }

    // Update is called once per frame
    void Update()
    {
        float t = Mathf.PingPong(Time.time / strobeDuration, 1f);
        this.GetComponent<SpriteRenderer>().color = myGradient.Evaluate(t);
        current = this.transform.position.x;
        this.transform.position += -transform.up * Time.deltaTime * 2;
        LeftRight();

    }

    public virtual void LeftRight()
    {
        float step = 1f * Time.deltaTime;

        if (moveRight)
        {
            this.transform.position += transform.right * step;
            if (current >= right)
            {
                moveRight = false;
            }
        }
        else
        {
            this.transform.position += -transform.right * step;
            if (current <= left)
            {
                moveRight = true;
            }
        }
    }

    void DestroyOutofBoundary()
    {
        if (this.transform.position.y <= -5)
        {
            Destroy(this.gameObject);
        }
    }
}
