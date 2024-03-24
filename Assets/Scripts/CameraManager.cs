using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    public Camera MainCamera { get; private set; }

    private void Awake()
    {
        Instance = this;

        MainCamera = Camera.main;
    }
}