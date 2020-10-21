﻿//=================================================
//
//    Created by jianzhou.yao
//
//=================================================

using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class PlayableClipMixerState
{
    private AnimationClipPlayable _playable;
    private double _duration;
    private AnimationMixerPlayable _mixer;
    private float _weight;
    private int _id;
    private int _inputIndex;

    private PlayableClipMixerStateMachine _stateMachine;

    public Action<PlayableClipMixerState> OnUpdate;

    public PlayableClipMixerState(int inputIndex,AnimationClipPlayable playable,AnimationMixerPlayable mixer,int id)
    {
        _playable = playable;
        _duration = _playable.GetAnimationClip().length;
        _mixer = mixer;
        _inputIndex = inputIndex;
        _id = id;
    }

    public PlayableClipMixerStateMachine StateMachine
    {
        get { return _stateMachine; }
        set { _stateMachine = value; }
    }

    public int Id
    {
        get { return _id; }
    }

    public bool IsComplete()
    {
        //Debug.Log(_playable.GetTime()+" duration:"+_duration);
        double timeLeft = _duration - _playable.GetTime();
        return timeLeft <= 0;
    }

    public void Update(float dt)
    {
        // if (IsComplete() && _id != 0)
        // {
        //     _stateMachine.ChangeState(0);
        // }
        OnUpdate?.Invoke(this);
    }

    public void Enter(float weight = 1)
    {
        _weight = weight;
        UpdateWeight();
        _playable.SetTime(0);
    }

    private void UpdateWeight()
    {
        _mixer.SetInputWeight(_inputIndex,_weight);
    }

    public float GetWeight()
    {
        return _weight;
    }
        
    public void SetWeight(float value)
    {
        _weight = value;
        UpdateWeight();
    }

    public void Exit()
    {
        _mixer.SetInputWeight(_inputIndex,0);
    }
        
    public enum PlayableClipStateType
    {
        FadeIn,
        FadeOut,
        Normal,
    }
}