using PlayerSystems.ObstacleAvoiding;
using SignalsFramework;
using TapticFeedback;
using UniRx;
using UnityEngine;

namespace ObstacleSystem
{
    public abstract class Obstacle : MonoBehaviour
    {
        #region "Signals"

        public static readonly Subject<Unit> OnObstacleTriggeredByPlayer = new Subject<Unit>();

        #endregion

        protected virtual void PlayerTriggered(PlayerObstacleAvoiding playerObstacleAvoiding)
        {
            Obstacle.OnObstacleTriggeredByPlayer.Fire();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerObstacleAvoiding playerObstacleAvoiding))
            {
                Taptic.Light();
                PlayerTriggered(playerObstacleAvoiding);
            }
        }
    }
}