using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform Target;
    private const float MoveSpeed = 50;
    private const float MaxDistance = 6f;

    public AnimationCurve FollowSpeed;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        float normalized_distance = Mathf.Clamp01(Vector2.Distance(transform.position, Target.position) / MaxDistance);
        if (Vector2.Distance(transform.position, Target.position) > 0.1)
        {
            transform.Translate((Target.position - transform.position).normalized * (MoveSpeed * FollowSpeed.Evaluate(normalized_distance)) * Time.deltaTime);
        }
        else
        {
            transform.position = Target.position;
        }

        Vector3 t = transform.position;
        t.z = -10;
        transform.position = t;
    }
}
