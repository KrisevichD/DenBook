using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconBtn : MonoBehaviour
{
    public GameObject update;
    public GameObject user;
    // Start is called before the first frame update
    void Start()
    {
        update = GameObject.Find("AddIconButton");

        user = GameObject.Find("User");
        transform.GetComponent<Button>().onClick.AddListener(delegate () {
            update.GetComponent<AddIconButton>().SendImage(transform.name);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
