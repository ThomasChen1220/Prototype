 using UnityEngine;
 using System.Collections;
 
[RequireComponent(typeof(ParticleSystem))]
 public class ParticleSystemAutoDestroy : MonoBehaviour 
 {
     private ParticleSystem ps;
 
 
     public void Start() 
     {
         ps = GetComponent<ParticleSystem>();
     }
 
     public void Update() 
     {
         if(ps)
         {
             if(!ps.IsAlive())
             {
                 Destroy(gameObject);
             }
         }
     }
 }