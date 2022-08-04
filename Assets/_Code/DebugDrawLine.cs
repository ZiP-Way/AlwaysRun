using UnityEngine;

public class DebugDrawLine : MonoBehaviour
{
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 50);
    }
#endif
}
