using PlayerSystems.ObstacleAvoiding;

namespace ObstacleSystem
{
    public class LowObstacle : Obstacle
    {
        protected override void PlayerTriggered(PlayerObstacleAvoiding playerObstacleAvoiding)
        {
            base.PlayerTriggered(playerObstacleAvoiding);
            playerObstacleAvoiding.AvoidLowObstacle();
        }
    }
}