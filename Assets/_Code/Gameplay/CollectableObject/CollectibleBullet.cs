using PlayerSystems.MovementSystem;
using SignalsFramework;
using UniRx;
using UnityEngine;

public class CollectibleBullet : Bullet
{
    #region "Signals"

    public static readonly Subject<Unit> OnBulletCollected = new Subject<Unit>();

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement player))
        {
            CollectibleBullet.OnBulletCollected.Fire();
            Disable();
        }
    }
}
