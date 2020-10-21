using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableClipMixerStateMachine
{
    private PlayableClipStateCrossFade _crossFade;
        private bool _isCrossfading;
        private Dictionary<int,PlayableClipMixerState> _states;
        
        private PlayableClipMixerState _currentState;
        private PlayableClipMixerState _nextState;

        public PlayableClipMixerStateMachine()
        {
            _states = new Dictionary<int, PlayableClipMixerState>();
            _crossFade = new PlayableClipStateCrossFade();
        }

        public Dictionary<int, PlayableClipMixerState> States
        {
            get { return _states; }
        }

        public void AddState(PlayableClipMixerState mixerState)
        {
            mixerState.StateMachine = this;
            _states.Add(mixerState.Id,mixerState);
        }

        public void ChangeState(int stateId)
        {
            PlayableClipMixerState newState = GetState(stateId);
            if (newState == null)
            {
                return;
            }

            if (_currentState == newState)
            {
                return;
            }
            
            Debug.Log("change state:"+stateId);

            if (_currentState == null)
            {
                newState.Enter();
                _currentState = newState;
            }
            else
            {
                _nextState = newState;
                _crossFade.SetData(_currentState, newState, 0.5f);
                _isCrossfading = true;    
            }
            
        }

        public PlayableClipMixerState GetState(int stateId)
        {
            PlayableClipMixerState rlt = null;
            _states.TryGetValue(stateId, out rlt);
            return rlt;
        }

        public void Update(float dt)
        {
            if (_isCrossfading)
            {
                _crossFade.Update(dt);
                if (_crossFade.IsComplete())
                {
                    _isCrossfading = false;
                    _currentState = _nextState;
                    _nextState.Enter();
                    _nextState = null;
                }
                return;
            }
            _currentState.Update(dt);
        }
}
