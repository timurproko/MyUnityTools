using Animancer;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AnimancerComponent))]
public class PlayAnimation : MonoBehaviour
{
    [SerializeField] private AnimationClip _Animation;
    [SerializeField] private PlayModes _playMode;
    [SerializeField] private bool _randomStartTime;
    
    [ShowIf("_randomStartTime")]
    [SerializeField] private int _randomSeed;

    private AnimancerComponent _Animancer;

    private void Awake()
    {
        _Animancer = GetComponent<AnimancerComponent>();
    }

    protected virtual void OnEnable()
    {
        if (_playMode == PlayModes.OnEnable)
        {
            var state = _Animancer.Play(_Animation);

            if (_randomStartTime)
            {
                if (_randomSeed != 0) 
                {
                    Random.InitState(_randomSeed); 
                }
                
                state.NormalizedTime = Random.value;
            }
        }
    }
}

public enum PlayModes
{
    OnEnable
}