using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AddIconButton : MonoBehaviour
{
    public GameObject scrollView, user;
    public RectTransform prefab, content,canvas;
    private DirectoryInfo dirInfo = new DirectoryInfo("/storage/emulated/0/DCIM/.thumbnails/");
    //private DirectoryInfo dirInfo = new DirectoryInfo("d:\\Downloads\\");
    private FileInfo[] files;
    // Start is called before the first frame update
    void Start()
    {
        user = GameObject.Find("User");
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    IEnumerator setImage(RawImage img, string url)
    {

            Texture2D tex;
            tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
        using (WWW www = new WWW(url))
        {
            yield return www;
            www.LoadImageIntoTexture(tex);
            if (img)
                img.GetComponent<CanvasRenderer>().SetTexture(tex);
        }
    }

    public void LoadAvatarList()
    {
        var inst = Instantiate(scrollView, canvas);
        content = inst.transform.Find("Viewport").Find("Content") as RectTransform;
        content.GetComponent<GridLayoutGroup>().cellSize = new Vector2((Screen.width - 120) / 3, (Screen.width - 120) / 3);


        files = dirInfo.GetFiles("*.jpg", SearchOption.AllDirectories);
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        int size = 0;
        if(files.Length > size+30)
        {
            for (int i =size; i<size+30; i++)
            {
                var instance = GameObject.Instantiate(prefab.gameObject) as GameObject;
                instance.transform.SetParent(content, false);
                instance.transform.name = files[i].FullName;
                RawImage icon = instance.transform.Find("Mask").GetChild(0).GetComponent<RawImage>();


                StartCoroutine(setImage(icon, files[i].FullName));
            }
        } else
        {
            for (int i = size; i < files.Length; i++)
            {
                Debug.Log("hi");
                var instance = GameObject.Instantiate(prefab.gameObject) as GameObject;
                instance.transform.SetParent(content, false);
                instance.transform.name = files[i].FullName;
                RawImage icon = instance.transform.Find("Mask").Find("IconImg").GetComponent<RawImage>();

                Debug.Log(files[i].FullName);
                StartCoroutine(setImage(icon, files[i].FullName));
            }
        }
    }

    public void SendImage(string path)
    {
        StartCoroutine(SendImg(path));
    }

    private IEnumerator SendImg(string path)
    {


        WWWForm form = new WWWForm();
        form.AddBinaryData("image", File.ReadAllBytes(path), "bu.jpg", "image/jpg");
        
        string id = user.GetComponent<User>().getId().ToString();
        Debug.Log("id: " + id + ", path: " + path);
        WWW www = new WWW("https://denbook.me/api/users/" + id, form);
        yield return www;
        user.GetComponent<User>().userJson = www.text;
        Debug.Log("Server said: " + www.text);
        SceneManager.LoadScene("Account");
    }
}
