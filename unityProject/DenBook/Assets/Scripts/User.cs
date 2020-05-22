using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    public string userJson;
    public int n = 0;
    public string link;
    public int typeMain;
    public bool isAr;
    public string idDialog;
    public string friendsJson;
    // Start is called before the first frame update
    void Start()
    {
        isAr = false;
        typeMain = 1;
        GameObject[] list = GameObject.FindGameObjectsWithTag("user");
        if (list.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(transform.gameObject);

        }
    }
    public Root getFriendsIds()
    {
        
        Root root = JsonUtility.FromJson<Root>("{\"dialogs\":" + friendsJson + "}");
        return root;
    }
    // Update is called once per frame
    public string getName()
    {
        UserData userData = JsonUtility.FromJson<UserData>(userJson);
        return userData.name;
    }
    public int getId()
    {
        UserData userData = JsonUtility.FromJson<UserData>(userJson);
        return userData.id;
    }
    public string getCreated()
    {
        UserData userData = JsonUtility.FromJson<UserData>(userJson);
        return userData.created_at;
    }

    public string getImg()
    {
        UserData userData = JsonUtility.FromJson<UserData>(userJson);
        return userData.image_url;
    }

}
