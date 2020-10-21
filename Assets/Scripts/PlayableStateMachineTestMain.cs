using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.UI;

public class PlayableStateMachineTestMain : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private List<AnimationClip> _anims;
    
    private PlayableGraph _graph;
    
    private PlayableClipMixerStateMachine _stateMachine = new PlayableClipMixerStateMachine();
    private List<int> _stateIds = new List<int>();
    // Start is called before the first frame update
    
    [SerializeField]
    private int _stateId;
    
    [SerializeField]
    private Button _btnChangeState;
    
    void Awake()
    {
        _btnChangeState.onClick.AddListener(OnBtnChangeStateClick);
        
        PlayableGraph graph = PlayableGraph.Create("test_play_queue_graph");
        _graph = graph;
        graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

        AnimationPlayableOutput animOutput = AnimationPlayableOutput.Create(graph, "anim", _animator);
        AnimationMixerPlayable mixer = AnimationMixerPlayable.Create(graph, _anims.Count);
        animOutput.SetSourcePlayable(mixer);

        for (int i = 0; i < _anims.Count; i++)
        {
            AnimationClipPlayable clipPlayable = AnimationClipPlayable.Create(graph, _anims[i]);
            graph.Connect(clipPlayable, 0, mixer, i);
            PlayableClipMixerState mixerState = new PlayableClipMixerState(i,clipPlayable,mixer,i);
            _stateMachine.AddState(mixerState);
            _stateIds.Add(mixerState.Id);
        }

        //attack->idle when anim finished
        _stateMachine.GetState(1).OnUpdate += OnAttackUpdate;
        
        _stateMachine.ChangeState(0);
            
        graph.Play();

        // for (int i = 0; i < mixer.GetInputCount(); i++)
        // {
        //     Playable playable = mixer.GetInput(i);
        //     Debug.Log(playable.GetPlayableType());
        // }
    }

    private void OnAttackUpdate(PlayableClipMixerState state)
    {
        if (state.IsComplete())
        {
            _stateMachine.ChangeState(0);
        }
    }

    private void OnBtnChangeStateClick()
    {
        _stateMachine.ChangeState(_stateId);
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        _stateMachine.Update(dt);
    }
}
