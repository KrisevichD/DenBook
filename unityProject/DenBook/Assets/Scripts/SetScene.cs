using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetScene : MonoBehaviour
{
    public string scene;
    public int typeMain;
    public GameObject user;
    // Start is called before the first frame update
    public void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setScene()
    {
        if (typeMain != 0)
        {
            user = GameObject.Find("User");
            user.GetComponent<User>().typeMain = typeMain;
        }
        SceneManager.LoadScene(scene);
;    }
}
