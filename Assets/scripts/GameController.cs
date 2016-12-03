﻿using UnityEngine;
using System.Collections;
using DG.Tweening;
public class GameController : MonoBehaviour
{
    public enum State
    {
        Setup,
        Start,
        InGame,
        Pause,
        End
    }
    public State currentState;

    public int score;

    [Header("Player")]
    public GameObject player;
    public Transform playerSpawn;

    [Header("Comet Properties")]
    public GameObject comet;
    public Transform cometSpawn;
    private Rigidbody cometRigid;

    private float currentDistance;
    private float currentTarget;

    //Current tween
    private Tween currentTween;
    private bool hit;

    [Header("Debug")]
    public bool spawnComet;

    void Awake()
    {

    }

    void Update()
    {
        RunStates();
    }

    void SpawnObjects()
    {
        Instantiate(player, playerSpawn.position, playerSpawn.rotation);
        if (spawnComet)
        {
            GameObject newComet = (GameObject)Instantiate(comet, cometSpawn.position, cometSpawn.rotation);
            cometRigid = newComet.GetComponent<Rigidbody>();
        }
    }

    void SetupLevel()
    {
        currentDistance = GameData.levelSize;
    }

    void RunStates()
    {
        switch(currentState)
        {
            case State.Setup:
                SpawnObjects();
                SetupLevel();
                currentState = State.Start;
                break;
            case State.Start:
                currentState = State.InGame;
                break;
            case State.InGame:
                UpdateComet();
                break;
            case State.Pause:
                break;
            case State.End:
                break;
        }
    }

    void UpdateComet()
    {
        if(!hit)
        {
            currentDistance = Mathf.MoveTowards(currentDistance, 0, .01f);
        }
        cometRigid.transform.position = new Vector2(0, currentDistance);
    }

    [ContextMenu("Do it bitch")]
    void AddDistanceToComet()
    {
        hit = true;
        if(currentTween != null)
        {
            currentTween.Kill();
        }
        currentTween = DOTween.To(() => currentDistance, x => currentDistance = x, currentDistance + 5, 1f);
        currentTween.OnComplete(() => hit = false);
    }
}
