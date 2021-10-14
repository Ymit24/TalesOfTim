using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSpawner : MonoBehaviour
{
    public string NpcTrackingId;
    public Npc Target;

    void Start()
    {
        if (GlobalContinuityController.CanSpawn(NpcTrackingId))
        {
            Target.Spawn(NpcTrackingId, transform.position);
        }
        Destroy(gameObject);
    }
}
