﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class DistanceMeter : MonoBehaviour
{
    public RectTransform comet;
    public float cometStart;

    public void ChangeDistance(float distance)
    {
        float currentDistance = (distance + Mathf.Abs(GameData.cometDest)) / (GameData.cometStartY + Mathf.Abs(GameData.cometDest));
        currentDistance = Mathf.Clamp(currentDistance, 0, 1) * cometStart;
        
        comet.anchoredPosition = new Vector2(0, currentDistance);
    }
}
