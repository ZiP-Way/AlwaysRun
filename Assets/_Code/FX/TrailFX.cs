using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Utility;
using PlayerSystems.States;

public class TrailFX : MonoBehaviour
{
    [SerializeField, HideInInspector] private ParticleSystem _trailParticle = default;
    [SerializeField, HideInInspector] private ObservableDestroyTrigger _ltt = default;

    private void Awake()
    {
        OnAirState.OnActiveStateChanged.Subscribe(isActive => Toggle(isActive)).AddTo(_ltt);
    }

    private void Toggle(bool state)
    {
        if (state)
        {
            _trailParticle.Play();
        }
        else
        {
            _trailParticle.Stop();
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_trailParticle == null) _trailParticle = GetComponent<ParticleSystem>();
        if (_ltt == null) _ltt = gameObject.SetupDestroyTrigger();
    }
#endif
}
