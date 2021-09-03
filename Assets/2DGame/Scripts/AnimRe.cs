using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimRe : MonoBehaviour
{
    private PlayerControl player;
    private void Awake()
    {
        player = GetComponentInParent<PlayerControl>();
    }
    public void Reset()
    {
        player.isDig = false;
    }
}
