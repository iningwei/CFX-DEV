using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Launcher : MonoBehaviour
{

    void Start()
    {
        Application.targetFrameRate = 60;


        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
#if UNITY_IOS
            UnityEngine.iOS.Device.SetNoBackupFlag(Application.persistentDataPath);
#endif
        }


        GameCenter.Instance.Init();
    }


    private void Update()
    {
        GameCenter.Instance.Update();
    }


    private void FixedUpdate()
    {
        GameCenter.Instance.FixedUpdate();
    }
}
