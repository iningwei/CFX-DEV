using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCenter : Singleton<GameCenter>
{
    public Camera cam;
    public Transform bgTrans;
    public Transform leftTrans;
    public Transform rightTrans;
    public Transform bottomTrans;
    public Transform warningLineTrans;
    public SpriteRenderer nextShow;

    GameLogic logic;



    public MainWindow mainWindow;
    public GameOverWindow gameOverWindow;
    public MaxMergeEffect maxMergeEffect;
    public WelcomeArea welcomeArea;

    public void Init()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        bgTrans = GameObject.Find("GameArea/bg").GetComponent<Transform>();
        leftTrans = GameObject.Find("GameArea/Left").GetComponent<Transform>();
        rightTrans = GameObject.Find("GameArea/Right").GetComponent<Transform>();
        bottomTrans = GameObject.Find("GameArea/Bottom").GetComponent<Transform>();
        nextShow = GameObject.Find("GameArea/NextShow").GetComponent<SpriteRenderer>();
        warningLineTrans = GameObject.Find("GameArea/WarningLine").GetComponent<Transform>();


        mainWindow = GameObject.Find("GameArea/Canvas/MainWindow").GetComponent<MainWindow>();
        gameOverWindow = GameObject.Find("GameArea/Canvas/GameOverWindow").GetComponent<GameOverWindow>();

        maxMergeEffect = GameObject.Find("GameArea/MaxMergeEffect").GetComponent<MaxMergeEffect>();

        welcomeArea = GameObject.Find("GameArea/WelcomeArea").GetComponent<WelcomeArea>();


        AdjustGameArea.Init(cam, bgTrans, leftTrans, rightTrans, bottomTrans);
        logic = new GameLogic(cam, nextShow, warningLineTrans);
    }




    public override void Update()
    {
        logic.Update();
    }

    public override void FixedUpdate()
    {
        logic.FixedUpdate();
    }

}
