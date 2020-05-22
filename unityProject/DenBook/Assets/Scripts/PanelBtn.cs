using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelBtn : MonoBehaviour
{
    public GameObject update;
    // Start is called before the first frame update
    void Start()
    {
        update = GameObject.Find("Update");
        transform.GetComponent<Button>().onClick.AddListener(delegate () {
            
            update.GetComponent<UpdateVideo>().getDialog(transform.name);
        });
    }
    public void setDialogId()
    {

        update.GetComponent<UpdateVideo>().idDialog = transform.name;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
