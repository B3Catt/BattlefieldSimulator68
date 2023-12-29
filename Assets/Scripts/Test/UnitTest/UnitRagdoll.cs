using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{

    [SerializeField] private Transform ragdoll;
    private float Timer = 2f;
    private void Update()
    {
        Timer -= Time.deltaTime;

        if (Timer <= 0f)
        {
            Destroy(gameObject);
        }
    }

}
