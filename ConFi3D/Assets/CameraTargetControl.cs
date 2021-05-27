using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetControl : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        //Messenger.AddListener<string, GameObject>(MessengerEventType.ET_SELECT_TIP, onSelectTip);
    }

    private void onSelectTip(string tipName, GameObject tipObj)
    {
        //Debug.LogError("tipObj:" + tipName + ", 的世界坐标为：" + tipObj.transform.position);

        transform.position = tipObj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
