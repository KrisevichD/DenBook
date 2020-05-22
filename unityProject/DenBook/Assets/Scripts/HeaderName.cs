using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeaderName : MonoBehaviour
{
    public GameObject user;
    public Text txt;
    // Start is called before the first frame update
    private void Awake()
    {
        user = GameObject.Find("User");
        txt.text += "Привет, " + user.GetComponent<User>().getName();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
