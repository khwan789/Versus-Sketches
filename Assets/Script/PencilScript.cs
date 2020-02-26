using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PencilScript : MonoBehaviour
{
    float stay = 5f;
    float start = 0;

    // Update is called once per frame
    void Update()
    {
        start += Time.deltaTime;
        if (start >= stay)
        {
            Destroy(this.gameObject);
        }
    }
}
