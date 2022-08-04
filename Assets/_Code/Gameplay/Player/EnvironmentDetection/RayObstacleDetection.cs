using SignalsFramework;
using UniRx;
using UnityEngine;
using UpdateSys;

public class RayObstacleDetection : MonoBehaviour, ILateUpdatable
{
    [SerializeField] private DetectionRay _detectionRay = default;

    #region "Signals"

    public readonly Subject<bool> IsDetected = new Subject<bool>();

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
        IsDetected.Fire(_detectionRay.CheckDetection());
    }

    public bool Check()
    {
        return _detectionRay.CheckDetection();
    }
}
