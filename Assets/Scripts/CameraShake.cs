using DG.Tweening;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] float duration = 0.5f;
    [SerializeField] float strength = 0.2f;
    [SerializeField] int vibrato = 12;

    Camera cam;
    Vector3 startPosition;

    void Start()
    {
        cam = GetComponent<Camera>();
        startPosition = transform.position;
    }

    public void SmallShake()
    {
        cam.DOShakePosition(0.3f, 0.15f, 20).OnComplete(() =>
        {
            cam.transform.position = startPosition;
        });
    }

    public void Shake()
    {
        cam.DOShakePosition(duration, strength, vibrato).OnComplete(() =>
        {
            cam.transform.position = startPosition;
        });
    }
}
