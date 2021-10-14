using UnityEngine;

public class Npc : MonoBehaviour
{
    public string NpcTrackingId;
    [SerializeField] private GameObject Prefab;

    public void Spawn(string trackingId, Vector2 location)
    {
        NpcTrackingId = trackingId;
        GameObject go = Instantiate(Prefab);
        go.transform.position = location;
    }
}
