using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZGame.cc;
using ZGame.TimerTween;

public class MaxMergeEffect : MonoBehaviour
{
    public Transform root;
    public Transform effTongGuan;
    public Transform effCaiDai;

    public void Show()
    {
        Time.timeScale = 0;
        root.gameObject.SetActive(true);
        effTongGuan.localPosition = Vector3.zero;
        effTongGuan.gameObject.SetActive(true);
        effCaiDai.localPosition = Vector3.zero;
        effCaiDai.gameObject.SetActive(true);


        TimerTween.Delay(2f, () =>
        {
            this.Hide();
        }).SetUseRealTime(true).Start();
    }

    public void Hide()
    {

        effTongGuan.gameObject.RunTween(new MoveTo(0.4f, new Vector3(0, Screen.width / 100f / 2f + 4f), Space.Self).OnComplete((x) =>
        {
            this.root.gameObject.SetActive(false);
            this.effTongGuan.gameObject.SetActive(false);
            this.effCaiDai.gameObject.SetActive(false);
            Time.timeScale = 1;
        }).IgnoreTimeScale(true));



    }
}
