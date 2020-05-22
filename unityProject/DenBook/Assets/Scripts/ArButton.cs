using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArButton : MonoBehaviour
{
    private GameObject user;
    public Sprite ar;
    public Sprite norm;
    // Start is called before the first frame update
    void Start()
    {
        user = GameObject.Find("User");
    }

    // Update is called once per frame
    void Update()
    {
        if (user.GetComponent<User>().isAr)
        {
            transform.GetComponent<Image>().sprite = ar;
        } else
        {
            transform.GetComponent<Image>().sprite = norm;

        }
    }
}
