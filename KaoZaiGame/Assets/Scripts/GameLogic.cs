using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZGame.cc;
using ZGame.Event;
using ZGame.TimerTween;

public class Entity
{
    //Id和预制件唯一对应
    public int Id;
    public float Size;
    //name和游戏中entity的index唯一对应，递增
    public string name;

    public GameObject obj;

    public bool boomFlag = false;

    //是否曾落入到地面
    public bool isDropedFloor = false;

    //是否参加到redline check
    public bool checkRedLine = false;

    Timer checkTimer;
    public Entity(int id, float size, string name, GameObject obj)
    {
        this.Id = id;
        this.Size = size;
        this.name = name;
        this.obj = obj;

        checkTimer = TimerTween.Delay(1.5f, () =>
         {
             checkRedLine = true;
         });
        checkTimer.Start();
    }

    public void Destroy()
    {
        checkTimer?.Cancel();
        GameObject.DestroyImmediate(obj);
    }
}


public class BoomPair
{
    public Entity candidate;
    public Entity target;
    public BoomPair(Entity candidate, Entity target)
    {
        this.candidate = candidate;
        this.target = target;
    }
}


public class GameLogic
{
    Camera cam;
    SpriteRenderer nextShow;
    Transform warningLine;
    Dictionary<int, float> sizeDic = new Dictionary<int, float>();
    List<Entity> entities = new List<Entity>();
    List<BoomPair> boomPairs = new List<BoomPair>();


    float nextShowYPos;
    float warningLineYPos;
    int nameIndex = 0;
    int ranMaxId = 6;
    int nextId;
    int fillShowIndex = 1;
    bool isGameOver = false;
    float floorYPos;



    float scaleRatio;
    public GameLogic(Camera cam, SpriteRenderer nextShow, Transform warningLine)
    {
        this.cam = cam;
        this.nextShow = nextShow;
        this.warningLine = warningLine;

        scaleRatio = Screen.height / 1280f;

        floorYPos = -Screen.height / 2f / 100f + 120 / 100f;



        //////sizeDic.Add(1, 52 / 100f * scaleRatio);
        //////sizeDic.Add(2, 80 / 100f * scaleRatio);
        //////sizeDic.Add(3, 108 / 100f * scaleRatio);
        //////sizeDic.Add(4, 119 / 100f * scaleRatio);
        //////sizeDic.Add(5, 153 / 100f * scaleRatio);///
        //////sizeDic.Add(6, 183 / 100f * scaleRatio);
        //////sizeDic.Add(7, 193 / 100f * scaleRatio);
        //////sizeDic.Add(8, 258 / 100f * scaleRatio);
        //////sizeDic.Add(9, 308 / 100f * scaleRatio);
        //////sizeDic.Add(10, 308 / 100f * scaleRatio);
        //////sizeDic.Add(11, 408 / 100f * scaleRatio);

        sizeDic.Add(1, 64 / 100f * scaleRatio);
        sizeDic.Add(2, 80 / 100f * scaleRatio);
        sizeDic.Add(3, 110 / 100f * scaleRatio);
        sizeDic.Add(4, 120 / 100f * scaleRatio);
        sizeDic.Add(5, 150 / 100f * scaleRatio);///
        sizeDic.Add(6, 190 / 100f * scaleRatio);
        sizeDic.Add(7, 200 / 100f * scaleRatio);
        sizeDic.Add(8, 284 / 100f * scaleRatio);
        sizeDic.Add(9, 320 / 100f * scaleRatio);
        sizeDic.Add(10, 350 / 100f * scaleRatio);
        sizeDic.Add(11, 449 / 100f * scaleRatio);



        //////nextShowYPos = Screen.height / 2f / 100f - 153 / 2f / 100f * scaleRatio - 0.15f;
        //////warningLineYPos = Screen.height / 2f / 100f - 153f / 100f * scaleRatio - 0.15f;

        nextShowYPos = Screen.height / 2f / 100f - 150f / 2f / 100f * scaleRatio - 0.15f;
        warningLineYPos = Screen.height / 2f / 100f - 150f / 100f * scaleRatio - 0.15f;



        EventDispatcher.Instance.AddListener(EventID.OnGameStart, onGameStart);
        EventDispatcher.Instance.AddListener(EventID.OnGameOver, onGameOver);
        EventDispatcher.Instance.AddListener(EventID.OnReplayBtnClicked, onReplayBtnClicked);
        EventDispatcher.Instance.AddListener(EventID.OnScoreChange, onScoreChange);
        EventDispatcher.Instance.DispatchEvent(EventID.OnGameStart);
    }

    private void onScoreChange(string evtId, object[] paras)
    {
        int curScore = int.Parse(paras[0].ToString());
        int scoreOffset = int.Parse(paras[1].ToString());
        this.singleScore = curScore + scoreOffset;
    }

    private void onReplayBtnClicked(string evtId, object[] paras)
    {
        GameCenter.Instance.gameOverWindow.FlyCoinsAndHide();




    }

    private void onGameOver(string evtId, object[] paras)
    {
        Debug.LogError("game over!!!");
        isGameOver = true;
        Entity targetEntity = paras[0] as Entity;
        targetEntity.obj.GetComponent<SpriteRenderer>().color = new Color(120 / 255f, 120 / 255f, 120 / 255f, 1);

        entities.Remove(targetEntity);
        entities.Sort((a, b) =>
        {
            if (a.obj.transform.position.y > b.obj.transform.position.y)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        });
        entities.Insert(0, targetEntity);

        for (int i = 0; i < entities.Count; i++)
        {
            int tmpI = i;
            int x = i + 1;
            float y = Mathf.Log(x, 3);
            float delayTime = y + 0.2f;
            //Debug.LogError("delayTime:" + delayTime);
            TimerTween.Delay(delayTime, () =>
                  {
                      doGameOverBoom(entities[tmpI]);
                      if (tmpI == entities.Count - 1)
                      {
                          entities.Clear();


                          GameCenter.Instance.gameOverWindow.Show(singleScore);
                          GameCenter.Instance.mainWindow.Hide();
                      }
                  }).Start();
        }
    }


    void doGameOverBoom(Entity entity)
    {
        AudioManager.Instance.PlayBoom();
        string effName = "";
        if (entity.Id < 10)
        {
            effName = "Effects/effect_hecheng_0" + entity.Id;
        }
        else
        {
            effName = "Effects/effect_hecheng_" + entity.Id;
        }
        GameObject effObj = ResHelper.Instance.LoadModel(effName);
        effObj.transform.position = entity.obj.transform.position;
        effObj.transform.localScale = Vector3.one * scaleRatio;
        TimerTween.Delay(1.2f, () =>
        {
            GameObject.DestroyImmediate(effObj);
        }).Start();


        TimerTween.Delay(0.2f, () =>
        {
            int score = entity.Id;
            if (entity.Id == 11)
            {
                score = 500;
            }

            EventDispatcher.Instance.DispatchEvent(EventID.OnScoreChange, singleScore, score);
            entity.Destroy();
        }).Start();
    }


    /// <summary>
    /// 第一阶段的类型
    /// </summary>
    int firstPhaseType = 1;
    /// <summary>
    /// 第一阶段的步数
    /// </summary>
    int firstPhaseSteps = 0;
    int[] firstPhaseBalls1 = { 1, 1, 1, 2, 3, 3, 4 };
    int[] firstPhaseBalls2 = { 1, 1, 2, 2, 2, 4, 1 };
    int[] firstPhaseBalls3 = { 1, 1, 2, 2, 3, 3, 3, 3, 4 };

    bool isWelcome = true;
    private void onGameStart(string evtId, object[] paras)
    {

        GameCenter.Instance.mainWindow.Show();
        GameCenter.Instance.gameOverWindow.Hide();


        isGameOver = false;
        singleScore = 0;
        this.nextShow.transform.position = new Vector3(0, nextShowYPos, 0);
        this.warningLine.position = new Vector3(0, warningLineYPos, 0);
        fillShowIndex = 1;

        //第一阶段分为三种情况
        //情况一 权重40   ； 1，1，1，2，3，3，4
        //情况二 权重30 ；  1，1，2，2，2，4，1
        //情况三 权重30   ； 1，1，2，2，3，3，3，3，4
        var r = UnityEngine.Random.Range(1, 101);
        if (r <= 40)
        {
            firstPhaseSteps = 7;
            firstPhaseType = 1;
        }
        else if (r > 40 && r <= 70)
        {
            firstPhaseSteps = 7;
            firstPhaseType = 2;
        }
        else
        {
            firstPhaseSteps = 9;
            firstPhaseType = 3;
        }
        Debug.LogError("firstPhaseType:" + firstPhaseType);
        genNextBall();
    }

    void genNextBall()
    {

        if (fillShowIndex < firstPhaseSteps)
        {
            if (firstPhaseType == 1)
            {
                nextId = firstPhaseBalls1[fillShowIndex - 1];
            }
            else if (firstPhaseType == 2)
            {
                nextId = firstPhaseBalls2[fillShowIndex - 1];
            }
            else if (firstPhaseType == 3)
            {
                nextId = firstPhaseBalls3[fillShowIndex - 1];
            }

        }
        else
        {
            //第二阶段，根据分数，分配不同权重
            var r = UnityEngine.Random.Range(1, 201);
            //Debug.LogError("r value:" + r);
            if (singleScore <= 500)
            {
                if (r <= 75)
                {
                    nextId = 1;
                }
                else if (r > 75 && r <= 135)
                {
                    nextId = 2;
                }
                else if (r > 135 && r <= 170)
                {
                    nextId = 3;
                }
                else if (r > 170 && r <= 190)
                {
                    nextId = 4;
                }
                else
                {
                    nextId = 5;
                }
            }
            else if (singleScore > 500 && singleScore <= 1000)
            {
                if (r <= 65)
                {
                    nextId = 1;
                }
                else if (r > 65 && r <= 120)
                {
                    nextId = 2;
                }
                else if (r > 120 && r <= 160)
                {
                    nextId = 3;
                }
                else if (r > 160 && r <= 185)
                {
                    nextId = 4;
                }
                else
                {
                    nextId = 5;
                }
            }
            else
            {
                if (r <= 55)
                {
                    nextId = 1;
                }
                else if (r > 55 && r <= 100)
                {
                    nextId = 2;
                }
                else if (r > 100 && r <= 145)
                {
                    nextId = 3;
                }
                else if (r > 145 && r <= 175)
                {
                    nextId = 4;
                }
                else
                {
                    nextId = 5;
                }
            }
        }
        fillShowIndex++;
        fillNextShow(nextId);
    }

    void fillNextShow(int id)
    {
        nextShow.sprite = Resources.Load("UI/demon_" + id, typeof(Sprite)) as Sprite;
        scaleFinished = false;
        nextShow.gameObject.transform.localScale = Vector3.zero;
        nextShow.gameObject.RunTween(new ScaleTo(0.3f, Vector3.one * scaleRatio).Easing(ZGame.Ease.OutBack).OnComplete((x) =>
         {

             nextShow.transform.localScale = Vector3.one * scaleRatio;
             scaleFinished = true;
         }));
    }

    bool interaction = false;
    bool moveFinished = false;
    bool scaleFinished = false;

    float targetXPos = 0f;
    //单局的得分
    int singleScore = 0;


    public void FixedUpdate()
    {
        if (isGameOver)
        {
            return;
        }

        checkDropFloor();
        checkCollision();
        doBoom();
        checkRedLine();
    }


    public void Update()
    {
        if (isGameOver)
        {
            return;
        }
        if (nextShow.sprite == null || scaleFinished == false)
        {

        }
        else
        {

            if (Input.GetMouseButtonDown(0))
            {
                interaction = true;

                targetXPos = restrictXPos(ScreenXPosTo2DPos(Input.mousePosition.x));

                nextShow.gameObject.RunTween(new MoveTo(0.1f, new Vector3(targetXPos, nextShowYPos, 0), Space.World).OnComplete(
                    (a) =>
                    {
                        this.moveFinished = true;
                    }
                    ));

            }
            if (interaction && Input.GetMouseButtonUp(0))
            {
                this.moveFinished = false;
                this.interaction = false;
                if (isWelcome)
                {
                    GameCenter.Instance.welcomeArea.Hide();
                    isWelcome = false;
                }
                this.genEntityBy2DPos(nextId, new Vector3(targetXPos, nextShowYPos, 0), false);
                nextShow.sprite = null;
                nextShow.transform.position = new Vector3(0, nextShowYPos, 0);

                TimerTween.Delay(0.8f, () =>
                {

                    nextShow.transform.position = new Vector3(0, nextShowYPos, 0);

                    this.genNextBall();
                }).Start();
            }

            if (interaction && Input.GetMouseButton(0))
            {
                if (this.moveFinished)
                {
                    float tmpXPos = ScreenXPosTo2DPos(Input.mousePosition.x);
                    targetXPos = restrictXPos(tmpXPos);
                    nextShow.transform.position = new Vector3(targetXPos, nextShowYPos, 0);
                }

            }
        }


        //checkDropFloor();
        //checkCollision();
        //doBoom();
        //checkRedLine();
    }
    void checkDropFloor()
    {


        for (int i = 0; i < entities.Count; i++)
        {
            if (entities[i].isDropedFloor == false)
            {
                if (entities[i].obj.transform.position.y - sizeDic[entities[i].Id] * 0.5f - 0.01 <= floorYPos)
                {
                    entities[i].isDropedFloor = true;
                    //////AudioManager.Instance.PlayDropFloor();//暂时屏蔽该音效
                }
            }
        }
    }
    void checkRedLine()
    {
        for (int i = 0; i < entities.Count; i++)
        {
            if (entities[i].checkRedLine)
            {
                if (entities[i].obj.transform.position.y + sizeDic[entities[i].Id] * 0.5f > warningLineYPos)
                {

                    EventDispatcher.Instance.DispatchEvent(EventID.OnGameOver, entities[i]);
                    return;
                }
            }
        }



        for (int i = 0; i < entities.Count; i++)
        {
            if (entities[i].checkRedLine)
            {
                if (entities[i].obj.transform.position.y + sizeDic[entities[i].Id] * 0.5f + 2f > warningLineYPos)
                {
                    this.warningLine.gameObject.SetActive(true);
                    return;
                }
            }
        }

        this.warningLine.gameObject.SetActive(false);
    }

    float restrictXPos(float tmpXPos)
    {
        float targetXPos;
        if (tmpXPos + sizeDic[nextId] * 0.5f > Screen.width / 2f / 100f)
        {
            targetXPos = Screen.width / 2f / 100f - sizeDic[nextId] * 0.5f;
        }
        else if (tmpXPos - sizeDic[nextId] * 0.5f < -Screen.width / 2f / 100f)
        {
            targetXPos = -Screen.width / 2f / 100f + sizeDic[nextId] * 0.5f;
        }
        else
        {
            targetXPos = tmpXPos;
        }
        return targetXPos;
    }

    void genEntityByMousePos(int id, Vector3 mousePos)
    {

        Vector3 pos = ScreenPosTo2DPos(mousePos);
        GameObject obj = ResHelper.Instance.LoadModel("demon_" + id.ToString());
        obj.transform.position = pos;
        obj.name = nameIndex.ToString();
        nameIndex++;
        entities.Add(new Entity(id, sizeDic[id], nameIndex.ToString(), obj));
    }

    void genEntityBy2DPos(int id, Vector3 pos, bool isMerge)
    {
        GameObject obj = ResHelper.Instance.LoadModel("demon_" + id.ToString());
        obj.transform.position = pos;
        obj.name = nameIndex.ToString();
        obj.transform.localScale = Vector3.one * scaleRatio;

        nameIndex++;
        entities.Add(new Entity(id, sizeDic[id], nameIndex.ToString(), obj));


    }


    void checkCollision()
    {

        Entity candidate;
        Entity target;
        float dis;
        for (int i = 0; i < entities.Count; i++)
        {
            candidate = entities[i];
            for (int j = 0; j < entities.Count; j++)
            {
                target = entities[j];
                if (i == j || candidate.Id == 11 || target.Id == 11 || candidate.Id != target.Id || candidate.boomFlag || target.boomFlag || candidate.obj.transform.position.y < target.obj.transform.position.y)
                {
                    continue;
                }

                dis = Vector3.Distance(candidate.obj.transform.position, target.obj.transform.position);
                //Debug.LogError("dis:" + dis);
                if (dis - candidate.Size * 0.5f - target.Size * 0.5f < 0f)
                {
                    //candidate和target标记boomFlag
                    candidate.boomFlag = true;
                    target.boomFlag = true;

                    //添加我BoomPair
                    BoomPair pair = new BoomPair(candidate, target);
                    boomPairs.Add(pair);
                }
            }
        }

        if (entities.Count > 0)
        {
            //移除标记了boomFlag的Entity
            for (int i = entities.Count - 1; i >= 0; i--)
            {
                if (entities[i].boomFlag)
                {
                    entities.RemoveAt(i);
                }
            }
        }

    }


    void doBoom()
    {
        if (this.boomPairs.Count > 0)
        {
            for (int i = 0; i < boomPairs.Count; i++)
            {
                doSingleBoom(boomPairs[i]);
            }
        }
        this.boomPairs.Clear();
    }

    void doSingleBoom(BoomPair boomPair)
    {
        var candidateObj = boomPair.candidate.obj;
        var targetObj = boomPair.target.obj;

        int pairId = boomPair.candidate.Id;

        Vector3 fromPos = candidateObj.transform.position;
        Vector3 toPos = targetObj.transform.position;
        bool flag = false;
        TimerTween.Value(0, 1, 0.2f, (v) =>
        {
            if (v > 0.3f)
            {
                targetObj.GetComponent<Rigidbody2D>().simulated = false;//不再受影响
            }
            if (v > 0.6f && !flag)
            {
                flag = true;
                if (pairId == 10)
                {
                    AudioManager.Instance.PlayAudioMaxMerge();
                }
                else
                {
                    AudioManager.Instance.PlayAudioMerge();
                }


                AudioManager.Instance.PlayBoom();
                string effName = "";
                if (pairId < 9)
                {
                    effName = "Effects/effect_hecheng_0" + (pairId + 1);
                }
                else
                {
                    effName = "Effects/effect_hecheng_" + (pairId + 1);
                }
                GameObject effObj = ResHelper.Instance.LoadModel(effName);
                effObj.transform.position = boomPair.target.obj.transform.position;
                effObj.transform.localScale = Vector3.one * scaleRatio;
                TimerTween.Delay(1.2f, () =>
                {
                    GameObject.DestroyImmediate(effObj);
                    if (pairId == 10)
                    {
                        GameCenter.Instance.maxMergeEffect.Show();
                    }
                }).Start();
            }
            candidateObj.transform.position = fromPos + (toPos - fromPos) * v;
        }, () =>
        {
            //在targetObj的位置生成新的合成的球
            if (boomPair.candidate.Id < 11)
            {
                int score = boomPair.candidate.Id;
                if (boomPair.candidate.Id == 10)
                {
                    score = 500;
                }
                EventDispatcher.Instance.DispatchEvent(EventID.OnScoreChange, singleScore, score);


                //Debug.LogError("score:" + singleScore);
                this.genEntityBy2DPos(boomPair.candidate.Id + 1, targetObj.transform.position, true);
            }
            boomPair.candidate.Destroy();
            boomPair.target.Destroy();


        }).Start();
    }


    float ScreenXPosTo2DPos(float screenXPos)
    {
        return (screenXPos - 0.5f * Screen.width) / 100f;
    }
    Vector3 ScreenPosTo2DPos(Vector3 screenPos)
    {
        float x = screenPos.x;
        float y = screenPos.y;
        x = x - 0.5f * Screen.width;
        y = y - 0.5f * Screen.height;

        x = x / 100f;
        y = y / 100f;
        return new Vector3(x, y, 0);
    }
}
