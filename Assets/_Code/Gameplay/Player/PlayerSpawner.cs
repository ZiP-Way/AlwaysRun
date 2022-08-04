using PlayerSystems;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utility;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Player _player = default;
    [SerializeField, HideInInspector] private ObservableDestroyTrigger _ltt = default;

    private void Awake()
    {
        Level.SpawnPlayer.Subscribe(position => DoSpawn(position)).AddTo(_ltt);
    }

    private void DoSpawn(Vector3 position)
    {
        _player.SetPosition(position);
    }

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        if (_ltt == null) _ltt = gameObject.SetupDestroyTrigger();
    }
#endif
}
