using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Vuforia;

public class ImageTargeting : MonoBehaviour,
                                            ITrackableEventHandler
{
    private TrackableBehaviour mTrackableBehaviour;
    public RectTransform panel;
    public Text txt;
    public Text txt2;
    public RawImage img;
    public VideoPlayer vp;
    public bool loaded;
    void Start()
    {
        loaded = false;
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }

        vp.Pause();
    }
    public void Update()
    {
        if (loaded)
        {
            vp.Play();
        } else {
            vp.Pause();
        }
       
        
    }
    public void OnTrackableStateChanged(
                                    TrackableBehaviour.Status previousStatus,
                                    TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            txt.color = Color.clear;
            txt2.color = Color.clear;

            img.color = Color.clear;
            panel.GetComponent<UnityEngine.UI.Image>().color = Color.clear;
            loaded = true;
            Debug.Log("TARGET IS HERE!!!");
        }
        else
        {
            loaded = false;
            txt.color = Color.black;
            txt2.color = Color.white;

            img.color = Color.white;
            panel.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            Debug.Log("WHERE IS TARGET?");
        }
    }
}
