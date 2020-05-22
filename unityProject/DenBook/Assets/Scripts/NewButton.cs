using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewButton : MonoBehaviour
{
    public Camera mainCamera;
    public InputField loginField;
    public InputField passwordField;
    float time = 0.1f;
    public Text txt;
    public GameObject user;


    // Start is called before the first frame update
    public void Start()
    {

        user = GameObject.FindGameObjectWithTag("user");
        StartCoroutine(Register());
    }

    // Update is called once per frame
    void Update()
    {


    }

    public IEnumerator Register()
    {
        string name = loginField.text;
        string password = passwordField.text;
        if (name == "" && password == "")
        {
            ErrorMsg(passwordField, "Введите данные");
            ErrorMsg(loginField, "Введите данные");

        }
        else
            if (name == "")
        {
            GrayField(passwordField);
            ErrorMsg(loginField, "Введите логин");
        }
        else
            if (password == "")
        {
            ErrorMsg(passwordField, "Введите пароль");
            GrayField(loginField);
        }

        else
        {

            WWWForm form = new WWWForm();
            form.AddField("name", name);
            form.AddField("password", password);
            WWW www = new WWW("https://denbook.me/api/register", form);
            yield return www;
            switch (www.text)
            {
                case "0":
                    Debug.Log("Logged in");
                    break;
                case "1":
                    Debug.Log("Incorrect name");
                    ErrorMsg(loginField, "Логин уже используется");
                    GreenField(passwordField);
                    break;
                case "2":
                    Debug.Log("Incorrect password");
                    GreenField(loginField);
                    ErrorMsg(passwordField, "Неверный пароль");
                    break;
                default:
                    Debug.Log("Unknown msg");
                    user.GetComponent<User>().userJson = www.text;
                    GreenField(loginField);
                    GreenField(passwordField);
                    StartCoroutine(BGgreen());
                    break;

            }
            Debug.Log("Server said: " + www.text);
        }
    }

    void GreenField(InputField ip)
    {
        ip.image.color = new Color(0.7f, 1f, 0.4f);
    }
    void GrayField(InputField ip)
    {
        ip.image.color = new Color(0.88f, 0.88f, 0.88f);
    }
    void ErrorMsg(InputField ip, string msg)
    {

        StartCoroutine(BGred());
        txt.color = new Color(1f, 0.4f, 0.4f);
        txt.text = msg;
        ip.image.color = new Color(1f, 0.4f, 0.4f);
    }

    public IEnumerator BGred()
    {
        Color bgColor = new Color(1f, 0.4f, 0.4f);
        float time = 0.7f;
        float currTime = 0f;
        do
        {
            mainCamera.backgroundColor = Color.Lerp(bgColor, Color.white, currTime / time);
            currTime += Time.deltaTime;
            yield return null;
        } while (currTime <= time);
    }
    public IEnumerator BGgreen()
    {
        Color bgColor = new Color(0.7f, 1f, 0.4f);
        float time = 0.7f;
        float currTime = 0f;
        do
        {
            mainCamera.backgroundColor = Color.Lerp(bgColor, Color.white, currTime / time);
            currTime += Time.deltaTime;
            yield return null;
        } while (currTime <= time);
        SceneManager.LoadScene("Main");
    }

    public void setActivePassword()
    {
        passwordField.Select();
    }

}
