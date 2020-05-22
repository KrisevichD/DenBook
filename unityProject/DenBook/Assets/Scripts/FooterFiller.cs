using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FooterFiller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int scale = Screen.width/4;
        transform.GetChild(0).GetComponent<RectTransform>().position.Set(scale, 75f, 0f);
        transform.GetChild(2).GetComponent<RectTransform>().position.Set(-scale, 75f,0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
