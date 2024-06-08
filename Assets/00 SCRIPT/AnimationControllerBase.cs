using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerState = PlayerController.PlayerState;

public abstract class AnimationControllerBase : MonoBehaviour
{
    public abstract void UpdateAnimation(PlayerState playerState);
}
