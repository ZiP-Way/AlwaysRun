using PlayerSystems.ObstacleAvoiding;

namespace ObstacleSystem
{
    public class SpringBoardObstacle : Obstacle
    {
        protected override void PlayerTriggered(PlayerObstacleAvoiding playerObstacleAvoiding)
        {
            base.PlayerTriggered(playerObstacleAvoiding);
            playerObstacleAvoiding.JumpOnSpringBoard();
        }
    }
}