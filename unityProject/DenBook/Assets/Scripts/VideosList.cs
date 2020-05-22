using UnityEngine;

[System.Serializable]
public class VideosList
{

    public int id;
    public int from_id;
    public int to_id;
    public string description;
    public string created_at;
    public string url;
    public string preview_url;
    public UserData from_user;
    public UserData to_user;

    public static VideosList[] CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<VideosList[]>(jsonString);
    }

    // Given JSON input:
    // {"name":"Dr Charles","lives":3,"health":0.8}
    // this example will return a PlayerInfo object with
    // name == "Dr Charles", lives == 3, and health == 0.8f.
}

[System.Serializable]
public class Dialog
{
    public int user_id;
    public int peer_id;
    public int last_video_id;
    public VideosList last_video;
    public UserData peer_user;

}


[System.Serializable]
public class Root
{
    public VideosList[] videos;
    public Dialog[] dialogs;
    public UserData[] users;
}

