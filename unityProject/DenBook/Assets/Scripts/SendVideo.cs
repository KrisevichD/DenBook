using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SendVideo : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        StartCoroutine(Send());
    }

    private IEnumerator Send()
    {

        int id = 1;

        WWWForm form = new WWWForm();
        form.AddBinaryData("image", File.ReadAllBytes("Assets/Sprites/sMekDV4xUBg.jpg"), "bu.jpg", "image/jpeg");
       

        WWW www = new WWW("http://161.35.79.190/api/users/"+id, form);
        yield return www;


        Debug.Log("Server said: " + www.text);
    }
}
