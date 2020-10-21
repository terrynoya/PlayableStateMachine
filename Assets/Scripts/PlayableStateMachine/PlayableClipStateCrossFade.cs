//=================================================
//
//    Created by jianzhou.yao
//
//=================================================

using System;

public class PlayableClipStateCrossFade
{
    private float _duration;
    private float _currentTime;

    private PlayableClipMixerState _prevMixerState;
    private PlayableClipMixerState _nextMixerState;

    public void SetData(PlayableClipMixerState prevMixerState, PlayableClipMixerState nextMixerState,
        float duration)
    {
        _currentTime = 0;
        _prevMixerState = prevMixerState;
        _nextMixerState = nextMixerState;
        _duration = duration;
    }

    public bool IsComplete()
    {
        return _duration - _currentTime <= 0;
    }

    public void Update(float dt)
    {
        // Debug.Log("crossfading!!");
        _currentTime += dt;
        _currentTime = Math.Min(_currentTime, _duration);
        float weight = _currentTime / _duration;
        _prevMixerState.SetWeight(1 - weight);
        _nextMixerState.SetWeight(weight);
    }
}