using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class PlayableStateMachineTestMain : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private List<AnimationClip> _anims;

    private AnimationMixerPlayable _mixer;
    private PlayableGraph _graph;
    
    private PlayableClipMixerStateMachine _stateMachine = new PlayableClipMixerStateMachine();
    private List<int> _stateIds = new List<int>();
    // Start is called before the first frame update
    void Awake()
    {
        PlayableGraph graph = PlayableGraph.Create("test_play_queue_graph");
        _graph = graph;
        graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

        AnimationPlayableOutput animOutput = AnimationPlayableOutput.Create(graph, "anim", _animator);
        AnimationMixerPlayable mixer = AnimationMixerPlayable.Create(graph, _anims.Count);
        _mixer = mixer;
        animOutput.SetSourcePlayable(mixer);

        for (int i = 0; i < _anims.Count; i++)
        {
            AnimationClipPlayable clipPlayable = AnimationClipPlayable.Create(graph, _anims[i]);
            graph.Connect(clipPlayable, 0, mixer, i);
            // _animPlayables.Add(clipPlayable);
                
            PlayableClipMixerState mixerState = new PlayableClipMixerState(i,clipPlayable,mixer,i);
            _stateMachine.AddState(mixerState);
            _stateIds.Add(mixerState.Id);
            // _states.Add(mixerState);
        }
            
        _stateMachine.ChangeState(0);
            
        graph.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
