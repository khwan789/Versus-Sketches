using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    Toggle dontShowToggle;
    Button okayButton;
    public bool dontShow;
    int boolNum;
    // Start is called before the first frame update
    void Start()
    {
        dontShowToggle = this.GetComponentInChildren<Toggle>();
        boolNum = PlayerPrefs.GetInt(this.gameObject.name,0);

        Debug.Log("boolNum " + boolNum);
        Debug.Log(dontShow);


        if (boolNum == 1)
        {
            Destroy(this.gameObject);
        }
    }

    public void OkayButton()
    {
        Destroy(this.gameObject);
    }

    public void ToggleTrueFalse()
    {
        if (dontShowToggle.isOn)
        {
            dontShow = true;
            PlayerPrefs.SetInt(this.gameObject.name, 1);
        }
        else
        {
            dontShow = false;
            PlayerPrefs.SetInt(this.gameObject.name, 0);
        }
        Debug.Log(dontShow);
    }
}
