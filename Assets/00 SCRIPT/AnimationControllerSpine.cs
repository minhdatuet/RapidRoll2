using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerState = PlayerController.PlayerState;
using Spine.Unity;
using Spine;

public class AnimationControllerSpine : AnimationControllerBase
{
    [SerializeField] SkeletonAnimation _anim;
    PlayerState _currentState;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<SkeletonAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UpdateAnimation(PlayerState playerState)
    {
        if (playerState != _currentState)
        {
            _currentState = playerState;
            ChangeAnimation();

        }

        float moveDirection = Input.GetAxisRaw("Horizontal");
        if (moveDirection * this.transform.localScale.x < 0)
        {
            Vector3 tmpScale = this.transform.localScale;
            tmpScale.x = -tmpScale.x;
            this.transform.localScale = tmpScale;
        }
    }

    public void ChangeAnimation()
    {
        Debug.Log(_currentState);
        switch (_currentState)
        {
            case PlayerState.IDLE:
                _anim.state.SetAnimation(0, "idle", true);
                break;
            case PlayerState.RUN:
                _anim.state.SetAnimation(0, "run", true);
                break;
            case PlayerState.SHOOT:
                _anim.state.SetAnimation(0, "shoot", true);
                break;
            case PlayerState.DEATH:
                _anim.state.SetAnimation(0, "death", true);
                break;
            case PlayerState.PORTAL:
                Debug.LogError("PORTAL");
                _anim.state.SetAnimation(0, "portal", true);
                break;
            case PlayerState.JUMP:
                _anim.state.SetAnimation(0, "jump", true);
                break;
        }
        
    }
}
