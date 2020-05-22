using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    private bool camAvailable;
    private WebCamTexture backCam;
    private Texture defaultBackground;

    public bool isFront = true;
    public RawImage background;
    public AspectRatioFitter fit;

    // Use this for initialization
    private void Start()
    {
        defaultBackground = background.texture;
        setCam();

       
        
        

    }
    public void swapCam()
    {
        isFront = isFront ? false : true;
        setCam();
    }
    public void setCam()
    {

        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length == 0)
        {
            Debug.Log("No devices");
            camAvailable = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++)
        {
            if (isFront == true)
            {
                if (devices[i].isFrontFacing)
                {
                    backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);

                }
            }
            else
            {
                if (!devices[i].isFrontFacing)
                {
                    backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);

                }
            }

        }
        if (backCam == null)
        {
            Debug.Log("Unable to find back cam");
            return;
        }
        backCam.Stop();

        backCam.Play();
        if (backCam.isPlaying)
        {

            Debug.Log("Camera playing");
        }
        else
        {

            Debug.Log("Camera not playing");
        }
        background.texture = backCam;

        camAvailable = true;
    }
    public void stopCamera()
    {
        backCam.Stop();
        if (backCam.isPlaying)
        {

            Debug.Log("Camera playing");
        }
        else
        {

            Debug.Log("Camera not playing");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!camAvailable)
        {
            return;
        }
        float ratio = (float)backCam.width / (float)backCam.height;
        fit.aspectRatio = ratio;

        float scaleX = backCam.videoVerticallyMirrored ? 1f : -1f;
        if (isFront)
        {
            background.rectTransform.localScale = new Vector3(scaleX, 1f, 1f);

        }
        

        int orient = backCam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0,0,orient);
        
    }
}