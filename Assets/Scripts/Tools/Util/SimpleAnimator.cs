using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SimpleAnimator : MonoBehaviour
{
    [SerializeField, Range(0, 10f)]
    private float _speed = 1;
    [SerializeField]
    protected List<AnimationData> _normalizedAnimations;
    [SerializeField, Tooltip("Call Play() once again to loop")]
    private UnityEvent _onAnimationEnd;
    [SerializeField]
    bool _useUnscaledTime = false;

    public void PlayAndThen(UnityAction a)
    {
        _onAnimationEnd.AddListener(a);
        UnityAction autoRemove = () => _onAnimationEnd.RemoveListener(a);
        _onAnimationEnd.AddListener(autoRemove);
        _onAnimationEnd.AddListener(() => _onAnimationEnd.RemoveListener(autoRemove));
        Play();
    }

    protected virtual void OnValidate()
    {
        /*if (_normalizedAnimations.Count == 0)
        {
            _normalizedAnimations.Add(new AnimationData(ReferenceHolderAsset.Instance.EmptyAnimation, gameObject));
        }*/
        foreach (var anim in _normalizedAnimations)
        {
            if (anim.Targets.Count == 0)
                anim.Targets.Add(gameObject);
        }
        if (TryGetComponent<Animation>(out var a))
            a.enabled = false;
    }
    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
    internal void Retarget(int index, params GameObject[] targets)
    {
        Retarget(index, targets);
        //_normalizedAnimations[index].Targets = new List<GameObject>(GetComponentsInChildren<MeshRenderer>().Select((m)=>m.gameObject));
    }
    internal void Retarget(int index, IEnumerable<GameObject> targets)
    {
        _normalizedAnimations[index].Targets = new List<GameObject>(targets);
        //_normalizedAnimations[index].Targets = new List<GameObject>(GetComponentsInChildren<MeshRenderer>().Select((m)=>m.gameObject));
    }
    public virtual void Play()
    {
        //Debug.Log(gameObject.activeSelf+" On simple animator go state " + gameObject.name);
        gameObject.SetActive(true);
        StartCoroutine(PlayAtSpeed());
    }
    [Serializable]
    public class AnimationData
    {
        [SerializeField]
        AnimationClip _anim;
        [SerializeField]
        float _speedMultiplier = 1;
        [SerializeField]
        List<GameObject> _targets;
        public AnimationData(AnimationClip anim, GameObject defaultTar)
        {
            _anim = anim;
            _targets = new List<GameObject>() { defaultTar };
            _speedMultiplier = 1f;
        }

        public AnimationClip Anim { get => _anim; set => _anim = value; }
        public float SpeedMultiplier { get => _speedMultiplier; set => _speedMultiplier = value; }
        public List<GameObject> Targets { get => _targets; set => _targets = value; }
    }
    public void PlayAtSpeed(params (int, float)[] newSpeedForGivenIndex)
    {
        foreach (var ind in newSpeedForGivenIndex)
            _normalizedAnimations[ind.Item1].SpeedMultiplier = ind.Item2;
        Play();
    }
    public IEnumerator PlayAtSpeed()
    {
        //To implement negative just check _progress in absolute value in while check and add Time.DeltaTime*_speed
        //Check when passing a negative time the sample animation effectively sample the curve repeated so the end of the curve at -.001
#if false
        float _progress = 0;
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        while (_progress < _animation[0].length)
        {
            foreach (var anim in _animation)
            {
                anim.SampleAnimation(gameObject, _progress);
                _progress += Time.deltaTime * _speed;
            }
            yield return waitForEndOfFrame;
        } 
#else
        float progress = 0;
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        float duration = _normalizedAnimations.Max((a) => Mathf.Abs(1f / a.SpeedMultiplier))/_speed;
        while (progress < duration)
        {
            foreach (var anim in _normalizedAnimations)
            {
                int start = anim.SpeedMultiplier < 0 ? 1 : 0;
                float time = (start) + progress * anim.SpeedMultiplier * _speed;
                //float time = progress * anim.SpeedMultiplier * _speed + 1;
                //Debug.Log(anim.Anim.name + " ; " + time + " pro " + progress+" mult "+ anim.SpeedMultiplier);
                if (Mathf.Abs(time) < duration)
                {
                    foreach (var go in anim.Targets)
                        anim.Anim.SampleAnimation(go, time);
                }
            }
            progress += _useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            yield return waitForEndOfFrame;
        }
#endif
        _onAnimationEnd.Invoke();
        //gameObject.SetActive(true);
    }
}
