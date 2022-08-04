using UnityEngine;

public class DetectionRay : MonoBehaviour
{
    [SerializeField] private float _rayDistance = 0f;
    [SerializeField] private LayerMask _layerMask = default;

    public bool CheckDetection()
    {
        if(Physics.Raycast(transform.position, transform.forward, _rayDistance, _layerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * _rayDistance);
    }
#endif
}
