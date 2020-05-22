
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
public class AccountFiller : MonoBehaviour
{
    public GameObject user;
    public Text txtName;
    public Text txtCreated;
    public RawImage myImg;
    public string jsonData = "";
    public int myId;
    // Start is called before the first frame update
    void Start()
    {
        user = GameObject.FindGameObjectWithTag("user");
        
        myId = user.GetComponent<User>().getId();
        txtName.text = "Имя: " + user.GetComponent<User>().getName();
        string date = getDate(user.GetComponent<User>().getCreated());
        txtCreated.text = "Создан: " + date;
        StartCoroutine(setImage(myImg, user.GetComponent<User>().getImg()));
    }

    // Update is called once per frame
    string getDate(string date)
    {
        string res = "";
        res += date.Substring(0, 4) + "."
            + date.Substring(5, 2) + "."
            + date.Substring(8, 2) + " "
            + date.Substring(11,5);
        return res;
    }

    IEnumerator setImage(RawImage img, string url)
    {
        if (url != "")
        {

            Texture2D tex;
            tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
            using (WWW www = new WWW(url))
            {
                yield return www;
                www.LoadImageIntoTexture(tex);
                img.GetComponent<CanvasRenderer>().SetTexture(tex);
            }
        }

    }
}
