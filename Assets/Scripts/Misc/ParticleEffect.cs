using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    [field: SerializeField] public ParticleSystem particles {  get; private set; }

    void Start()
    {
        if(particles == null) particles = GetComponent<ParticleSystem>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.15f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
