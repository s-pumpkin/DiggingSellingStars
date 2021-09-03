using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectUISize : MonoBehaviour
{
    public RectTransform CheckChildCountRectTr;
    public RectTransform rectTransform;
    public float Length;

    public void Update()
    {
        int childsCount = CheckChildCountRectTr.transform.childCount;
        rectTransform.sizeDelta = new Vector2(childsCount * Length, rectTransform.sizeDelta.y);
    }
}
