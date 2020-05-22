using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cubeBtn : MonoBehaviour
{
    public GameObject update;
    // Start is called before the first frame update
    void Start()
    {
        update = GameObject.Find("Update");
        
    }

    private void OnMouseDown()
    {
        
            //update.GetComponent<UpdateVideo>().stopVideo();
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}