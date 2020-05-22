using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordingController : MonoBehaviour
{
    public Sprite type1;
    public Sprite type2;
    public Sprite type3;
    public int type = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void typePlus()
    {
        type++;
    }

    // Update is called once per frame
    void Update()
    {
        if (type > 2)
        {
            type = 0;
        }
        switch (type)
        {
            case 0:
                {
                    transform.GetComponent<Image>().color = Color.red;
                    transform.GetComponent<Image>().sprite = type1;
                    break;
                }
            case 1: {

                    transform.GetComponent<Image>().color = Color.black;
                    transform.GetComponent<Image>().sprite = type2;
                    break;
                }
            case 2:
                {
                    transform.GetComponent<Image>().color = Color.black;
                    transform.GetComponent<Image>().sprite = type3;
                    break;
                }

        }
    }
}
