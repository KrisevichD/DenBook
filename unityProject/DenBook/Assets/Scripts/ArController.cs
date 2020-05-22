using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ArController : MonoBehaviour
{
    public Transform target;
    public GameObject user;
    float time;
    float currTime;
    // Start is called before the first frame update
    void Start()
    {
        time = 0.7f;
        currTime = 0f;
        user = GameObject.Find("User");
        transform.GetComponent<VideoPlayer>().url = user.GetComponent<User>().link;
    }

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine(rotateCube());
        
    }

    IEnumerator rotateCube()
    {
        Vector3 relativePos = target.position - transform.position;

        // the second argument, upwards, defaults to Vector3.up
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        float time = 0.1f;
        float currTime = 0f;
        do
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, currTime / time);
            currTime += Time.deltaTime;
            yield return null;
        } while (currTime <= time);
    }


}
