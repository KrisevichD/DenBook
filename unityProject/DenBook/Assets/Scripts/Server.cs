using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;


public class Server : MonoBehaviour
{
    public GameObject cube;

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(SendVideo());
        StartCoroutine(GetUsers());

    }

    // Update is called once per frame
    private IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("name","DenisKA");
        form.AddField("password", "1234");
        WWW www = new WWW("http://161.35.79.190/api/register", form);
        yield return www;
        switch (www.text)
        {
            case "0":
                Debug.Log("User successfully added");
                break;
            case "1":
                Debug.Log("User already exists");
                break;
            case "2":
                Debug.Log("Password error");
                break;
            default:
                Debug.Log("Unknown server msg");
                break;
                
        }
        Debug.Log("Server said: " + www.text);
    }

    private IEnumerator Login()
    {
        string name = "Denis";
        string password = "12345";
        WWW www = new WWW("http://161.35.79.190/api/login?name="+name+"&password="+password );
        yield return www;
        switch (www.text)
        {
            case "0":
                Debug.Log("Logged in");
                break;
            case "1":
                Debug.Log("Incorrect name");
                break;
            case "2":
                Debug.Log("Incorrect password");
                break;
            default:
                Debug.Log("Unknown server msg");
                break;

        }
        Debug.Log("Server said: " + www.text);
    }

    private IEnumerator GetUsers()
    {
     
        WWW www = new WWW("http://161.35.79.190/api/users");
        yield return www;
        Debug.Log("Users: " + www.text);
    }

    private IEnumerator SendVideo()
    {   
        
      

        WWWForm form = new WWWForm();
        form.AddBinaryData("video", File.ReadAllBytes("Assets/IDUBBBZ I'M GAY MEME.mp4"), "bu.mp4", "video/mp4");
        form.AddField("from_id","1");
        form.AddField("to_id",3);
        
        WWW www = new WWW("http://161.35.79.190/api/videos", form);
        yield return www;

        cube.GetComponent<UnityEngine.Video.VideoPlayer>().url = www.text;


        Debug.Log("Server said: "+ www.text );
    }
}
