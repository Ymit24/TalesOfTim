using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitTextController : MonoBehaviour
{
    public TMPro.TMP_Text HitText;
    public float Life = 1;
    public float Speed = 1;
    private float _lifeTimer = 0;

    public void Activate(string message, float life, float speed, Color color)
    {
        HitText.text = message;
        Life = life;
        Speed = speed;
        HitText.color = color;
    }

    private void Update()
    {
        _lifeTimer += Time.deltaTime;
        transform.Translate(Vector2.up * Speed * Time.deltaTime);
        if (_lifeTimer >= Life)
        {
            Destroy(gameObject);
        }
    }
}
