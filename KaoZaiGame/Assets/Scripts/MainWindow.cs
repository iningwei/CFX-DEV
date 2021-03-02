using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZGame.Event;

public class MainWindow : MonoBehaviour
{
    public Transform root;


    [NonSerialized]
    public Text scoreTxt;
    [NonSerialized]
    public Text totalScoreTxt;


    void Start()
    {
        scoreTxt = root.Find("ScoreTxt").GetComponent<Text>();
        totalScoreTxt = root.Find("TotalScoreArea/TotalScoreTxt").GetComponent<Text>();


        EventDispatcher.Instance.AddListener(EventID.OnScoreChange, onScoreChange);
    }

    public void Show()
    {
        this.root.gameObject.SetActive(true);
        this.scoreTxt.text = "0";
        this.totalScoreTxt.text = Archive.Instance.GetTotalScore().ToString();
    }
    public void Hide()
    {
        this.root.gameObject.SetActive(false);
    }




    private void onScoreChange(string evtId, object[] paras)
    {
        int curScore = int.Parse(paras[0].ToString());
        int scoreOffset = int.Parse(paras[1].ToString());
        this.scoreTxt.text = (curScore + scoreOffset).ToString();
    }


}
