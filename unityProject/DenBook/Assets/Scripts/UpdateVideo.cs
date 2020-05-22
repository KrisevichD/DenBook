
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Video;
using System.Linq;
using UnityEngine.SceneManagement;

public class UpdateVideo : MonoBehaviour
{
    public GameObject user;
    public GameObject update;
    public RectTransform content;
    public RectTransform prefab;
    public RectTransform[] prefabVideo;
    public RectTransform prefabAvatar;
    public RectTransform prefabUser;
    public RawImage txtUpdate;
    public string jsonData="";
    public int myId;
    public RawImage player;
    public RawImage playerBack;
    public InputField dirName;
    public RawImage updatingAnim;
    public VideoPlayer vp;
    public GameObject cube;
    public VideoClip hello;
    public string idDialog;
    public Text headTxt;
    private DirectoryInfo dirInfo = new DirectoryInfo("/storage/emulated/0/DCIM/Camera");
    //private DirectoryInfo dirInfo = new DirectoryInfo("d:\\Downloads\\");
    private FileInfo[] files;
    public Button backBtn, arBtn, sendBtn;
    public int tp;
    // Start is called before the first frame update
    public void Start()
    {
        user = GameObject.FindGameObjectWithTag("user");
        tp = user.GetComponent<User>().typeMain;
        idDialog = user.GetComponent<User>().idDialog;
        myId = user.GetComponent<User>().getId();
        switch (tp)
        {
            case 1:
                {
                    StartCoroutine(getVideos(myId));
                    break;
                }
            case 2:
                {
                    StartCoroutine(getDialogs(idDialog));
                    break;
                }
            case 3:
                {
                    LoadAvatarList();
                    break;
                }
            case 4:
                {
                    LoadFriends();
                    break;
                }
        }
    }

    private void Update()
    {

        tp = user.GetComponent<User>().typeMain;
        switch (tp)
        {
            case 1:
                {
                    headTxt.text = "Диалоги";
                    backBtn.GetComponent<RectTransform>().localScale = new Vector3(0f,0f,0f);
                    sendBtn.GetComponent<RectTransform>().localScale = new Vector3(0f,0f,0f);
                    break;
                }
            case 2:
                {
                    backBtn.GetComponent<RectTransform>().localScale = new Vector3(-1f, 1f, 1f);
                    sendBtn.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                    break;
                }
            case 3:
                {
                    headTxt.text = "Видеозаписи";
                    backBtn.GetComponent<RectTransform>().localScale = new Vector3(-1f, 1f, 1f);
                    sendBtn.GetComponent<RectTransform>().localScale = new Vector3(0f, 0f, 0f);
                    break;
                }
            case 4:
                {
                    headTxt.text = "Люди";
                    backBtn.GetComponent<RectTransform>().localScale = new Vector3(-1f, 1f, 1f);
                    sendBtn.GetComponent<RectTransform>().localScale = new Vector3(0f, 0f, 0f);
                    break;
                }
        }
        float pos = content.GetComponent<RectTransform>().position.y;
        
        txtUpdate.transform.Rotate(0f,0f,-5f,Space.World);
        if (pos < 0.55 && Input.GetMouseButtonUp(0))
        {
            if (tp == 1)
            {

                
                StartCoroutine(getVideos(myId));
            } else if (tp == 2)
            {

                StartCoroutine(getDialogs(idDialog));
            }
        }
        if(tp < 3)
        if (pos < 0.55)
        {
            txtUpdate.color = new Color(0f,0f,0f,1f);
        } else
        {
            if (pos > 0.75)
            {

                txtUpdate.color = new Color(0f, 0f, 0f, 0f);
            } else
            {

                
                txtUpdate.color = new Color(0f, 0f, 0f, 4f*(0.75f-pos));
            }

        }
        if (cube.GetComponent<VideoPlayer>().isPlaying)
        {
            updatingAnim.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 0f);
        }
    }

    public void getVideo()
    {
        user.GetComponent<User>().typeMain = 1;      
        myId = user.GetComponent<User>().getId();
        StartCoroutine(getVideos(myId));
    }

    public IEnumerator getVideos(int myId)
    {

        Debug.Log("Give me videos of " + myId);
        WWW www = new WWW("https://denbook.me/api/dialogs?user_id=" + myId);
        yield return www;
        jsonData = www.text;
        user.GetComponent<User>().friendsJson = www.text;
        Debug.Log("Videos: " + www.text);
        Root root = JsonUtility.FromJson<Root>("{\"dialogs\":" + jsonData + "}");
        Dialog[] dialogs = root.dialogs;    
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        foreach (Dialog vl in dialogs)
        {

            var instance = GameObject.Instantiate(prefab.gameObject) as GameObject;
            instance.transform.SetParent(content, false);
            instance.transform.name = vl.peer_id.ToString();
            Text name = instance.transform.Find("AuthorName").GetComponent<Text>();
            RawImage img = instance.transform.Find("VideoMask").Find("ImgVideo").GetComponent<RawImage>();
            RawImage imgUser = instance.transform.Find("UserMask").Find("ImgUser").GetComponent<RawImage>();

            name.text = vl.peer_user.name;
            StartCoroutine(setImage(imgUser, vl.peer_user.image_url));
            StartCoroutine(setImage(img, vl.last_video.preview_url));
        }
    }
    public void getDialog(string panel)
    {
        user.GetComponent<User>().typeMain = 2;
        StartCoroutine(getDialogs(panel));
    }
    public IEnumerator getDialogs(string panel)
    {
        user.GetComponent<User>().idDialog = panel;
        RectTransform pref;
        string peerId = panel;
        Debug.Log("Give me videos of " + myId);
        WWW www = new WWW("https://denbook.me/api/videos?user_id=" + myId + "&peer_id=" + peerId);
        yield return www;
        jsonData = www.text;

        Debug.Log("Videos: " + www.text);
        Root root = JsonUtility.FromJson<Root>("{\"videos\":" + jsonData + "}");
        VideosList[] videosList = root.videos;
        if (peerId == videosList[0].from_id.ToString())
        {
            headTxt.text = videosList[0].from_user.name;
        } else
        {
            headTxt.text = videosList[0].to_user.name;

        }
        int i = Random.Range(0,4) ;
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        foreach (VideosList vl in videosList)
        {
            if (i > 4) i = 0;
            pref = prefabVideo[i];
            var instance = GameObject.Instantiate(pref.gameObject) as GameObject;
            instance.transform.SetParent(content, false);
            instance.transform.name = vl.url.ToString();
            Text name = instance.transform.Find("AuthorName").GetComponent<Text>();
            Text description = instance.transform.Find("Description").GetComponent<Text>();
            RawImage img = instance.transform.Find("VideoMask").Find("ImgVideo").GetComponent<RawImage>();
            RawImage imgUser = instance.transform.Find("UserMask").Find("ImgUser").GetComponent<RawImage>();

            name.text = vl.from_user.name;
            description.text = getDate(vl.created_at);
            StartCoroutine(setImage(imgUser, vl.from_user.image_url));
            StartCoroutine(setImage(img, vl.preview_url));

            i++;
        }
    }
    public void setDirectory()
    {
        string path = dirName.text;
        dirInfo = new DirectoryInfo(path);
        LoadAvatarList();
    }

    public void stopVideo()
    {
        cube.transform.localScale -= new Vector3(cube.transform.localScale.x, cube.transform.localScale.y, cube.transform.localScale.z);
        cube.GetComponent<VideoPlayer>().Stop();

        //    player.GetComponent<RectTransform>().sizeDelta = new Vector2(0f,0f);
           playerBack.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 0f);
        updatingAnim.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 0f);
        //    vp.Stop();
    }
    public void showVideo(string url)
    {
        //vp.url = url;
        //vp.Stop();
        //StartCoroutine(playVideo(url));
        cube.transform.localScale += new Vector3(Screen.width, Screen.height,0f);
        
        playerBack.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        updatingAnim.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 200f);
        cube.GetComponent<VideoPlayer>().url = url;
    }

    IEnumerator playVideo(string url)
    {
        vp.Prepare();
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        while (!vp.isPrepared)
        {
            yield return waitForSeconds;
            break;
        }
        player.texture = vp.texture;
        playerBack.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width,Screen.height);
        player.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.width*vp.texture.height/vp.texture.width);
        vp.Play();
    }

   
    

    IEnumerator setImage(RawImage img, string url)
    {
      
        if (url != null && url != "")
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
    }
    IEnumerator setImageFile(RawImage img, string url)
    {

        if (url != null && url != "")
        {

            Texture2D tex;
            tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
            using (WWW www = new WWW("file:///"+url))
            {
                yield return www;
                www.LoadImageIntoTexture(tex);
                img.GetComponent<CanvasRenderer>().SetTexture(tex);
            }
        }
    }

    public void LoadAvatarList()
    {
        user.GetComponent<User>().typeMain = 3;
        files = dirInfo.GetFiles("*.mp4", SearchOption.AllDirectories);
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        foreach (FileInfo vl in files)
        {
            var instance = GameObject.Instantiate(prefabAvatar.gameObject) as GameObject; instance.transform.SetParent(content, false);
            instance.transform.name = vl.FullName;
            Text name = instance.transform.Find("AuthorName").GetComponent<Text>();
            Text description = instance.transform.Find("Description").GetComponent<Text>();
            
            name.text = vl.Name;
            description.text = vl.FullName;
        }
        
    }

    public void changeAr()
    {
        bool isAr = user.GetComponent<User>().isAr;
        if (isAr)
        {
            user.GetComponent<User>().isAr = false;
        } else
        {
            user.GetComponent<User>().isAr = true;
        }
    }

    public void SendMessege(string path)
    {
        user.GetComponent<User>().typeMain = 2;
        StartCoroutine(SendVideofile(path));

        user.GetComponent<User>().typeMain = 2;
        StartCoroutine(getDialogs(idDialog));
    }
    public void AddFriend(string id)
    {
        user.GetComponent<User>().typeMain = 1;
        StartCoroutine(SendPresent(id));
        StartCoroutine(getVideos(myId));
    }
    private IEnumerator SendPresent(string id)
    {


        Debug.Log("Send present to "+id);
        WWWForm form = new WWWForm();
        form.AddBinaryData("video", File.ReadAllBytes(Application.dataPath+"/Hello.mp4"), "bu.mp4", "video/mp4");
        form.AddField("from_id", myId);
        form.AddField("to_id", id);

        WWW www = new WWW("http://161.35.79.190/api/videos", form);
        yield return www;
        user.GetComponent<User>().typeMain = 2;

        Debug.Log("Server said: " + www.text);
    }
    private IEnumerator SendVideofile(string path)
    {


        Debug.Log("Send this: "+path);
        WWWForm form = new WWWForm();
        form.AddBinaryData("video", File.ReadAllBytes(path), "bu.mp4", "video/mp4");
        form.AddField("from_id", myId);
        form.AddField("to_id", idDialog);

        WWW www = new WWW("http://161.35.79.190/api/videos", form);
        yield return www;
        Debug.Log("Server said: " + www.text);
    }
    
    public void LoadFriends()
    {
        user.GetComponent<User>().typeMain = 4;
        StartCoroutine(GetUsers());
    }
    

    public IEnumerator GetUsers()
    {

        Debug.Log("Give me users");
        WWW www = new WWW("http://161.35.79.190/api/users");
        yield return www;
        Debug.Log("Users: " + www.text);
        jsonData = www.text;

        Debug.Log("Videos: " + www.text);
        Root root = JsonUtility.FromJson<Root>("{\"users\":" + jsonData + "}");
        UserData[] videosList = root.users;
        

        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        bool isFriend;
        Root root2 = user.GetComponent<User>().getFriendsIds();
        foreach (UserData vl in videosList)
        {
            isFriend = false;
            for (int i = 0; i < root2.dialogs.Length; i++)
            {
                if (root2.dialogs[i].peer_id == vl.id || myId == vl.id)
                {
                    isFriend = true;
                    break;
                }
            }

            if (!isFriend)
            {
                var instance = GameObject.Instantiate(prefabUser.gameObject) as GameObject;
                instance.transform.SetParent(content, false);
                instance.transform.name = vl.id.ToString();
                Text name = instance.transform.Find("AuthorName").GetComponent<Text>();
                Text description = instance.transform.Find("Description").GetComponent<Text>();
                RawImage imgUser = instance.transform.Find("UserMask").GetChild(0).GetComponent<RawImage>();

                name.text = vl.name;
                description.text = "В Denbook с " + getFullDate(vl.created_at);
                StartCoroutine(setImage(imgUser, vl.image_url));
            }
        }
    }

    string getFullDate(string date)
    {
        string res = "";
        res += date.Substring(8, 2) + "."
            + date.Substring(5, 2) + "."
            + date.Substring(0, 4);
        return res;
    }
    string getDate(string date)
    {
        string res = "";
        res += date.Substring(11, 5);
        return res;
    }
}
