using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZGame.Event;

public class Blink : MonoBehaviour
{
    public SpriteRenderer sr;

    bool isGameOver = false;
    void Start()
    {
        EventDispatcher.Instance.AddListener(EventID.OnGameStart, onGameStart);
        EventDispatcher.Instance.AddListener(EventID.OnGameOver, onGameOver);
    }

    private void onGameOver(string evtId, object[] paras)
    {
        isGameOver = true;
        sr.color = new Color(1, 1, 1, 1);
    }

    private void onGameStart(string evtId, object[] paras)
    {
        isGameOver = false;
    }

    float duration = 1f;
    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
        {
            return;
        }
        duration -= Time.deltaTime;
        sr.color = new Color(1, 1, 1, duration);
        if (duration < 0)
        {
            duration = 1f;
        }
    }


}
