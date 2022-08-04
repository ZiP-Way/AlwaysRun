using Data;
using PlayerSystems.MovementSystem;
using SignalsFramework;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement _player))
        {
            Hub.LevelFailed.Fire();
        }
    }
}