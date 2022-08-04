using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    protected void Disable()
    {
        gameObject.SetActive(false);
    }
}
