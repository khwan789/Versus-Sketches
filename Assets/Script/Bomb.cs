using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour
{
    Vector3 newScale;

    // Start is called before the first frame update
    void Start()
    {
        newScale = new Vector3(1f, 1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.localScale.x <= 20)
        {
            this.transform.localScale += newScale * Time.deltaTime * 35;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
