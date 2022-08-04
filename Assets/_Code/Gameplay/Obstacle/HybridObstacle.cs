using PlayerSystems.ObstacleAvoiding;

namespace ObstacleSystem
{
    public class HybridObstacle : Obstacle
    {
        protected override void PlayerTriggered(PlayerObstacleAvoiding playerObstacleAvoiding)
        {
            base.PlayerTriggered(playerObstacleAvoiding);
            playerObstacleAvoiding.AvoidHybridObstacle();
        }
    }
}