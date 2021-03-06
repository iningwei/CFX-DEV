using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZGame.Event;
using ZGame.TimerTween;

public class GameOverWindow : MonoBehaviour
{
    public Transform root;

    [NonSerialized]
    public Text scoreTxt;
    [NonSerialized]
    public Text maxScoreTxt;
    [NonSerialized]
    public Text totalScoreTxt;
    [NonSerialized]
    public Transform coinIconTrans;

    [NonSerialized]
    public Button replayBtn;

    GameObject coinPrefab;

    void Start()
    {
        scoreTxt = root.Find("ScoreArea/ScoreTxt").GetComponent<Text>();
        maxScoreTxt = root.Find("MaxScoreArea/MaxScoreTxt").GetComponent<Text>();
        totalScoreTxt = root.Find("TotalScoreArea/TotalScoreTxt").GetComponent<Text>();

        coinIconTrans = root.Find("TotalScoreArea/CoinIcon").GetComponent<Transform>();

        coinPrefab = root.Find("CoinPrefab").gameObject;

        replayBtn = root.Find("ReplayBtn").GetComponent<Button>();
        replayBtn.onClick.AddListener(this.onReplayBtnClicked);
    }


    int singleScore;
    int curTotalScore;
    public void Show(int score)
    {
        this.replayBtn.gameObject.SetActive(true);
        singleScore = score;
        this.root.gameObject.SetActive(true);
        this.scoreTxt.text = score.ToString();


        int tmpMaxScore = Archive.Instance.GetMaxScore();
        if (score > tmpMaxScore)
        {
            Archive.Instance.SetMaxScore(score);
            tmpMaxScore = score;
        }
        this.maxScoreTxt.text = tmpMaxScore.ToString();


        curTotalScore = Archive.Instance.GetTotalScore();
        totalScoreTxt.text = curTotalScore.ToString();
    }

    public void Hide()
    {

        this.root.gameObject.SetActive(false);
    }


    public void FlyCoinsAndHide()
    {
        //////this.replayBtn.gameObject.SetActive(false);
        //////Vector3 t_pos1 = coinIconTrans.transform.TransformPoint(Vector3.zero);
        //////Vector3 t_pos2 = this.root.InverseTransformPoint(t_pos1);

        //////for (int i = 0; i < 10; i++)
        //////{
        //////    int tmpI = i;
        //////    GameObject coinObj = GameObject.Instantiate(coinPrefab) as GameObject;
        //////    coinObj.transform.SetParent(this.root);
        //////    coinObj.transform.localScale = Vector3.one;
        //////    coinObj.SetActive(true);

        //////    Vector3 pos1 = scoreTxt.transform.TransformPoint(Vector3.zero);
        //////    Vector3 pos2 = this.root.InverseTransformPoint(pos1);
        //////    coinObj.transform.localPosition = pos2;

        //////    TimerTween.Delay(i * 0.1f, () =>
        //////    {
        //////        TimerTween.Value(0, 1, 0.5f, (v) =>
        //////        {
        //////            coinObj.transform.localPosition = pos2 + v * (t_pos2 - pos2);
        //////        }, () =>
        //////        {
        //////            GameObject.DestroyImmediate(coinObj);
        //////            if (tmpI == 9)
        //////            {
        //////                this.Hide();
        //////                GameCenter.Instance.mainWindow.Show();
        //////                EventDispatcher.Instance.DispatchEvent(EventID.OnGameStart);
        //////            }
        //////        }).Start();
        //////    }).Start();
        //////}

        //////TimerTween.Value(0, 1, 1.3f, (v) =>
        //////{
        //////    this.totalScoreTxt.text = ((int)(curTotalScore + v * singleScore)).ToString();
        //////    this.scoreTxt.text = ((int)(singleScore - v * singleScore)).ToString();
        //////}, () =>
        //////{
        //////    this.totalScoreTxt.text = (curTotalScore + singleScore).ToString();
        //////}).Start();
        ///

        this.Hide();
        GameCenter.Instance.mainWindow.Show();
        EventDispatcher.Instance.DispatchEvent(EventID.OnGameStart);
    }

    private void onReplayBtnClicked()
    {
        AudioManager.Instance.PlayReplay();
        Archive.Instance.SetTotalScore(curTotalScore + singleScore);

        EventDispatcher.Instance.DispatchEvent(EventID.OnReplayBtnClicked);
    }




}
