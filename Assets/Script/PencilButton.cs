using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PencilButton : MonoBehaviour
{
    Button thisButton;
    float start = 0;
    public GameObject pencil;
    public bool pencilUse;
    // Start is called before the first frame update
    void Start()
    {
        pencilUse = true;

        thisButton = this.GetComponent<Button>();
        thisButton.onClick.AddListener(UsePencil);
    }

    // Update is called once per frame
    void Update()
    {
        PencilCooldown();

    }
    public void UsePencil()
    {
        if (pencilUse)
        {
            GameObject _pencil = Instantiate(pencil, GameObject.Find("GameManager").transform);
            thisButton.GetComponent<Image>().color = Color.grey;
            pencilUse = false;
        }
    }
    public void PencilCooldown()
    {
        if (pencilUse == false)
        {
            start += Time.deltaTime;
            if (start >= 15)
            {
                pencilUse = true;
                thisButton.GetComponent<Image>().color = Color.white;
                start = 0;
            }
        }
    }

}
