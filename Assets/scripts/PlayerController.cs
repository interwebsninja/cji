﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    public float acceleration;

    private PlayerActions actions;
    private Rigidbody rigid;

    private ConstellationManager constManager;

    //Star tracking
    private StarController lastStar;

    public GameObject PlayerModel;

    public Material NormalMat;
    public Material PinkMat;
    public Material BlueMat;
    public Material GreenMat;
    public Material YellowMat;

    void Start()
    {
        actions = PlayerActions.BindAll();
        rigid = GetComponent<Rigidbody>();
        constManager = ConstellationManager.Instance;
    }

    void Update()
    {
        CompleteConstellationListener();
    }

    void FixedUpdate()
    {
        Movement();
    }

    void CompleteConstellationListener()
    {
        if(actions.PrimaryAction.WasPressed)
        {
            constManager.CompleteConstellation();
        }
    }
    void Movement()
    {
        Vector2 dir = actions.Move.Value * speed;
        Vector2 newSpeed = Vector2.Lerp(rigid.velocity, dir, acceleration);

        rigid.velocity = newSpeed;
    }

    void OnTriggerEnter(Collider hit)
    {
        StarController isStar = hit.GetComponent<StarController>();

        if(isStar)
        {
            if(lastStar != null && lastStar.theStarType != isStar.theStarType)
            {
                //destroy the star you hit to break the constellation
                //isStar.GetComponent<StarController>().StopMovement();
                //isStar.GetComponent<StarController>().DeactivateCollider();
                //isStar.GetComponent<StarController>().Shrinkle();

                constManager.BreakConstellation();
                lastStar = null;
            }
            else
            {
                lastStar = isStar;
                isStar.StopMovement();
                isStar.starData.Position = isStar.transform.position;
                if (isStar.theStarType == GameData.StarType.Circle)//blue
                {
                    PlayerModel.GetComponent<Renderer>().material = BlueMat;
                }
                else if (isStar.theStarType == GameData.StarType.Square)//Pink
                {
                    PlayerModel.GetComponent<Renderer>().material = PinkMat;
                }
                else if (isStar.theStarType == GameData.StarType.Star)//Yellow
                {
                    PlayerModel.GetComponent<Renderer>().material = YellowMat;
                }
                else if (isStar.theStarType == GameData.StarType.Triangle)//Green
                {
                    PlayerModel.GetComponent<Renderer>().material = GreenMat;
                }

                constManager.AddStar(isStar.starData);
            }
        }
    }
}
