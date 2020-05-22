using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackBtn : MonoBehaviour
{
    public string scene;
    public int typeMain;
    private GameObject user;
    // Start is called before the first frame update
    void Start()
    {
        user = GameObject.Find("User");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            user.GetComponent<User>().typeMain = typeMain;
            SceneManager.LoadScene(scene);
        }
    }
}
