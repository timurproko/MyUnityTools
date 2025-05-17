#if ANIMANCER
using System;
using Animancer;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AnimancerComponent))]
[AddComponentMenu("My Tools/Animation/" + "Play Animation")]
public class PlayAnimation : MonoBehaviour
{
    [SerializeField] private AnimationClip _Animation;
    [SerializeField] private PlayModes _playMode;
    [SerializeField] private bool _loop;
    [SerializeField] private bool _randomStartTime;

    [ShowIf("_randomStartTime")]
    [SerializeField] private int _randomSeed;

    private AnimancerComponent _Animancer;
    private AnimancerState _State;

    private void Awake()
    {
        _Animancer = GetComponent<AnimancerComponent>();
    }

    private void Start()
    {
        if (_playMode == PlayModes.OnStart)
        {
            PlayAnimationClip();
        }
    }

    private void OnEnable()
    {
        if (_playMode == PlayModes.OnEnable && _Animancer != null)
        {
            PlayAnimationClip();
        }

        if (_State != null)
            _State.IsPlaying = true;
    }

    private void OnDisable()
    {
        if (_State != null)
            _State.IsPlaying = false;
    }

    private void PlayAnimationClip()
    {
        _State = _Animancer.Play(_Animation);
        _State.IsPlaying = enabled;

        if (_randomStartTime)
        {
            if (_randomSeed != 0)
                Random.InitState(_randomSeed);

            _State.NormalizedTime = Random.value;
        }

        if (_loop)
        {
            _State.Events(this).OnEnd = () =>
            {
                _Animancer.Play(_Animation).NormalizedTime = 0;
            };
        }
        else
        {
            _State.Events(this).OnEnd = null;
        }
    }
}

public enum PlayModes
{
    OnStart,
    OnEnable
}
#endif