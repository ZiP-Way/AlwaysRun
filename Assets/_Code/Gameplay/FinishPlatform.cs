using UnityEngine;

public class FinishPlatform : MonoBehaviour
{
    [SerializeField] private Transform _body = default;

    public void SetPositionUnderPlayer(Vector3 playerPosition)
    {
        _body.gameObject.SetActive(true);
        _body.position = new Vector3(playerPosition.x, playerPosition.y - 2, playerPosition.z);
    }
}