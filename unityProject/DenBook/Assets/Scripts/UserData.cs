using UnityEngine;

[System.Serializable]
public class UserData
{

    public int id;
    public string name;
    public string created_at;
    public string image_url;

    public static UserData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<UserData>(jsonString);
    }

    // Given JSON input:
    // {"name":"Dr Charles","lives":3,"health":0.8}
    // this example will return a PlayerInfo object with
    // name == "Dr Charles", lives == 3, and health == 0.8f.
}