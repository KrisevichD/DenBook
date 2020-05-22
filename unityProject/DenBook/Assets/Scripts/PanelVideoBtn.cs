using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class PanelVideoBtn : MonoBehaviour
{
    
    public GameObject update;
    public GameObject user;
    // Start is called before the first frame update
    void Start()
    {
        update = GameObject.Find("Update");

        user = GameObject.Find("User");
        transform.GetComponent<Button>().onClick.AddListener(delegate () {
            if (!update.GetComponent<UpdateVideo>().cube.GetComponent<VideoPlayer>().isPlaying)
            {
                if (user.GetComponent<User>().isAr)
                {
                    setAR();
                } else
                {
                    playVideo();
                }
            }
        });
    }

    public void setAR()
    {
        update.GetComponent<UpdateVideo>().user.GetComponent<User>().link = transform.name;
        SceneManager.LoadScene("AR");
    }

    

   

    public void playVideo()
    {

        update.GetComponent<UpdateVideo>().showVideo(transform.name);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
