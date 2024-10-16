using Contest;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Contest
{
    /// <summary>
    /// パーティクルの生成と管理を行うクラス。
    /// </summary>
    public class ParticleManager : SingletonBehavior<ParticleManager>
    {
        [SerializeField] private List<ParticleSet> particleSetsList = new List<ParticleSet>();
        private Dictionary<ParticleType, GameObject> particleSets = new Dictionary<ParticleType, GameObject>();

        // ターゲットオブジェクトとそのアクティブなパーティクルのリスト
        private Dictionary<GameObject, List<GameObject>> activationParticles = new Dictionary<GameObject, List<GameObject>>();

        /// <summary>
        /// 初期化処理。シリアライズされたリストから辞書を構築します。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            foreach (var set in particleSetsList)
            {
                if (!particleSets.ContainsKey(set.ParticleType))
                {
                    particleSets.Add(set.ParticleType, set.ParticlePrefab);
                }
                else
                {
                    Debug.LogWarning($"ParticleType {set.ParticleType} is already added. Skipping duplicate.");
                }
            }
        }

        /// <summary>
        /// 指定されたパーティクルタイプのパーティクルをターゲットオブジェクトに生成します。
        /// </summary>
        /// <param name="particleType">生成するパーティクルのタイプ。</param>
        /// <param name="options">アニメーションオプション。</param>
        /// <param name="target">パーティクルを適用するターゲットオブジェクト。</param>
        public void MakeParticle(ParticleType particleType, AnimationOptions options, GameObject target)
        {
            if (!particleSets.TryGetValue(particleType, out GameObject particlePrefab))
            {
                Debug.LogError($"ParticleType {particleType} is not registered in particleSets.");
                return;
            }

            if (target == null)
            {
                Debug.LogError("Target GameObject is null.");
                return;
            }

            GameObject particleInstance = Instantiate(particlePrefab, target.transform);
            if (particleInstance == null)
            {
                Debug.LogError($"Failed to instantiate particle for ParticleType {particleType}.");
                return;
            }

            if (!activationParticles.ContainsKey(target))
            {
                activationParticles[target] = new List<GameObject>();
            }
            particleInstance.GetComponent<RectTransform>().anchoredPosition = target.transform.position;
            activationParticles[target].Add(particleInstance);

            // アニメーションオプションに基づいてパーティクルを設定
            ConfigureParticle(particleInstance, options);
        }

        /// <summary>
        /// パーティクルのアニメーションオプションを設定します。
        /// </summary>
        /// <param name="particle">設定するパーティクルオブジェクト。</param>
        /// <param name="options">アニメーションオプション。</param>
        private void ConfigureParticle(GameObject particle, AnimationOptions options)
        {
            // AnimationOptionsに基づいた設定をここに実装
            // 例: パーティクルの再生速度や持続時間の設定など
            ParticleSystem particleSystem = particle.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Play();
            }
            else
            {
                Debug.LogWarning("ParticleSystem component is missing on the particle prefab.");
            }
        }

        /// <summary>
        /// ターゲットオブジェクトからすべてのパーティクルを削除します。
        /// </summary>
        /// <param name="target">パーティクルを削除するターゲットオブジェクト。</param>
        public void DeleteParticle(GameObject target)
        {
            if (target == null)
            {
                Debug.LogError("Target GameObject is null.");
                return;
            }

            if (!activationParticles.ContainsKey(target))
            {
                return;
            }

            foreach (var particle in activationParticles[target])
            {
                if (particle != null)
                {
                    Destroy(particle);
                }
            }

            activationParticles.Remove(target);
        }

        /// <summary>
        /// ターゲットオブジェクトから指定されたパーティクルタイプのパーティクルを削除します。
        /// </summary>
        /// <param name="target">パーティクルを削除するターゲットオブジェクト。</param>
        /// <param name="particleType">削除するパーティクルのタイプ。</param>
        public void DeleteParticle(GameObject target, ParticleType particleType)
        {
            if (target == null)
            {
                Debug.LogError("Target GameObject is null.");
                return;
            }

            if (!particleSets.TryGetValue(particleType, out GameObject particlePrefab))
            {
                Debug.LogError($"ParticleType {particleType} is not registered in particleSets.");
                return;
            }

            if (!activationParticles.ContainsKey(target))
            {
                return;
            }

            // 該当するパーティクルを検索
            GameObject particleToRemove = activationParticles[target].FirstOrDefault(p => p.name.Contains(particlePrefab.name));
            if (particleToRemove != null)
            {
                activationParticles[target].Remove(particleToRemove);
                Destroy(particleToRemove);
            }
            else
            {
                Debug.LogWarning($"No active particle of type {particleType} found on target {target.name}.");
            }

            // ターゲットに関連付けられたパーティクルがなくなったら辞書から削除
            if (activationParticles[target].Count == 0)
            {
                activationParticles.Remove(target);
            }
        }

        /// <summary>
        /// ターゲットオブジェクトに関連付けられたすべてのパーティクルを削除します。
        /// </summary>
        /// <param name="target">パーティクルを削除するターゲットオブジェクト。</param>
        public void RemoveAllParticles(GameObject target)
        {
            DeleteParticle(target);
        }
    }

    /// <summary>
    /// パーティクルタイプと対応するパーティクルプレハブのペアを表します。
    /// </summary>
    [Serializable]
    public class ParticleSet
    {
        public ParticleType ParticleType;
        public GameObject ParticlePrefab;
    }

}