using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendButton : MonoBehaviour
{
    public GameObject update;
    public GameObject user;
    // Start is called before the first frame update
    void Start()
    {
        update = GameObject.Find("Update");

        user = GameObject.Find("User");
        transform.GetComponent<Button>().onClick.AddListener(delegate () {
            update.GetComponent<UpdateVideo>().SendMessege(transform.parent.name);
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
