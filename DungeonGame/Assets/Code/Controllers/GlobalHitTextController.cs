using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalHitTextController : MonoBehaviour
{
    private static GlobalHitTextController Instance;
    public GameObject HitTextPrefab;

    public Color HitColor, XpColor, GoldColor;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static void SpawnHitText(Vector2 location, float damage, float speed = 1f)
    {
        SpawnColorText(location, damage.ToString(), 1, 1.25f, Instance.HitColor);
    }

    public static void SpawnXpText(Vector2 location, int xp)
    {
        SpawnColorText(location, "+" + xp.ToString(), 1, 0.75f, Instance.XpColor);
    }

    public static void SpawnGoldText(Vector2 location, int gold)
    {
        SpawnColorText(location, "+" + gold.ToString(), 1, 0.75f, Instance.GoldColor);
    }

    public static void SpawnColorText(Vector2 location, string text, float life, float speed, Color color)
    {
        GameObject go = Instantiate(Instance.HitTextPrefab);
        go.transform.position = location;
        HitTextController ctr = go.GetComponent<HitTextController>();
        ctr.Activate(text, life, speed, color);
    }
}
