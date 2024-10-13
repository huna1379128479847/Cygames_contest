using System;
using System.Collections.Generic;
using UnityEngine;

namespace Contest
{
    public class ParticleManager : SingletonBehavior<ParticleManager>
    {
        [SerializeField] Dictionary<ParticleType, GameObject> particleSets = new Dictionary<ParticleType, GameObject>();
        // エフェクトを再生させるオブジェクト / エフェクトのリスト
        private Dictionary<GameObject, List<GameObject>> acivationParticles = new Dictionary<GameObject, List<GameObject>>();
        public void MakeParticle(ParticleType particle, AnimationOptions options, GameObject target)
        {
            // 未実装
        }
        public void DeleteParticle(GameObject target)
        {
            if (!acivationParticles.ContainsKey(target)) { return; }
            foreach(var particle in acivationParticles[target])
            {
                Destroy(particle);
            }
            acivationParticles.Remove(target);
        }
        public void DeleteParticle(GameObject target, ParticleType particleType)
        {
            particleSets.TryGetValue(particleType, out GameObject particle);
            if (acivationParticles.ContainsKey(target) && acivationParticles[target].Contains(particle))
            {
                int idx = acivationParticles[target].IndexOf(particle);
                Destroy(acivationParticles[target][idx]);
                acivationParticles[target].Remove(particle);
            }
        }
    }
}
