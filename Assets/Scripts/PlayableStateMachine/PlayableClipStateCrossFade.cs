//=================================================
//
//    Created by jianzhou.yao
//
//=================================================

using System;

public class PlayableClipStateCrossFade
{
    private float _duration = 0.5f;
    private float _currentTime;

    private PlayableClipMixerState _prevMixerState;
    private PlayableClipMixerState _nextMixerState;

    public void SetData(PlayableClipMixerState prevMixerState, PlayableClipMixerState nextMixerState,
        float? duration = null)
    {
        _currentTime = 0;
        _prevMixerState = prevMixerState;
        _nextMixerState = nextMixerState;
        if (duration != null)
        {
            _duration = duration.GetValueOrDefault();    
        }
    }

    public void SetDuration(float value)
    {
        _duration = value;
    }

    public bool IsComplete()
    {
        return _duration - _currentTime <= 0;
    }

    public void Update(float dt)
    {
        if (_duration > 0)
        {
            // Debug.Log("crossfading!!");
            _currentTime += dt;
            _currentTime = Math.Min(_currentTime, _duration);
            float weight = _currentTime / _duration;
            _prevMixerState.SetWeight(1 - weight);
            _nextMixerState.SetWeight(weight);    
        }
        else
        {
            _prevMixerState.SetWeight(0);
            _nextMixerState.SetWeight(1);
        }
    }
}