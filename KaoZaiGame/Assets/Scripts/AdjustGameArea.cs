using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustGameArea
{
    public static void Init(Camera cam, Transform bg, Transform left, Transform right, Transform bottom)
    {
        int width = Screen.width;
        int height = Screen.height;
        Debug.LogError($"width:{width}, height:{height}");

        //bg的设计分辨率为1080*1920,这里缩放一下
        float xRatio = width / 1080f;
        float yRatio = height / 1920f;
        bg.transform.localScale = new Vector3(xRatio, yRatio, 1);




        cam.orthographicSize = height / 2f / 100f;

        left.transform.localPosition = new Vector3(-width / 2f / 100f, 0, 0);
        right.transform.localPosition = new Vector3(width / 2f / 100f, 0, 0);

        bottom.transform.localPosition = new Vector3(0, -height / 2f / 100f + 120 / 2f / 100f, 0f);
    }
}
