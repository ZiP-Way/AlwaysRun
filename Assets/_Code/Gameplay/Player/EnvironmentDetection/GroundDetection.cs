using SignalsFramework;
using UniRx;
using UnityEngine;
using UpdateSys;

public class GroundDetection : MonoBehaviour, ILateUpdatable
{
    [SerializeField] private DetectionRay[] _detectionRays = default;

    #region "Signals"

    public readonly Subject<bool> IsOnGround = new Subject<bool>();

    #endregion

    private void OnEnable()
    {
        this.StartLateUpdate();
    }

    private void OnDisable()
    {
        this.StopLateUpdate();
    }

    public void OnSystemLateUpdate(float deltaTime)
    {
        Check();
    }

    private void Check()
    {
        if (IsGroundDetected())
        {
            IsOnGround.Fire(true);
        }
        else
        {
            IsOnGround.Fire(false);
        }
    }

    private bool IsGroundDetected()
    {
        foreach (DetectionRay ray in _detectionRays)
        {
            if (ray.CheckDetection() == false)
            {
                return false;
            }
        }

        return true;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        _detectionRays = GetComponentsInChildren<DetectionRay>();
    }
#endif
}
