using UnityEngine;

public class FPS : MonoBehaviour
{
#if UNITY_ANDROID
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
#endif
}
