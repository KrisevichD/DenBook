using UnityEngine;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public class GetPermission : MonoBehaviour
{

    public void Start()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
        }
#endif
    }

    
}