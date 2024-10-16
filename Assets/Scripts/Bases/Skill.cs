using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Contest
{
    /// <summary>
    /// スキルの動作を管理するクラス。
    /// スキルの発動、アニメーションの管理、ダメージ計算などを行う。
    /// </summary>
    public class Skill : IDoAction, IUniqueThing
    {
        // スキルデータ
        public SkillData skillData;
        // スキルを管理する親ハンドラー
        public SkillHandler parent;
        // ユニークID
        private Guid id;
        // スキルが実行中かどうかを管理
        private bool inAction;
        // スキルのフラグ (攻撃、防御、バフなど)
        private SkillTypes skillTypes;
        // スキルがダメージか回復かを示すフラグ
        private bool isBad;
        // スキルが攻撃かどうかを示すフラグ
        private bool isAttack;
        //
        public List<DamageInfo> currentResult = new List<DamageInfo>();

        public MiniGameResult currentMiniGameResult = MiniGameResult.Normal;

        // スキルのユニークIDを返すプロパティ
        public Guid ID => id;

        /// <summary>
        /// スキルがアクション中 (エフェクト再生中) かどうかを示すプロパティ。
        /// </summary>
        public bool InAction => inAction;

        /// <summary>
        /// スキルのフラグ (攻撃、防御、バフなど) を取得するプロパティ。
        /// </summary>
        public SkillTypes SkillTypes => skillData.SkillTypes;

        /// <summary>
        /// スキルが使用可能かどうかを示すプロパティ。
        /// MPが必要コスト以上あれば使用可能。
        /// </summary>
        public virtual bool CanUse
        {
            get
            {
                return parent.Parent.StatusTracker.CurrentMP.CurrentAmount >= skillData.Cost;
            }
        }

        /// <summary>
        /// スキルの基本的な効果量 (ダメージ) を計算するプロパティ。
        /// </summary>
        public virtual int DamageAmount
        {
            get
            {
                float variation = UnityEngine.Random.Range(0.9f, 1.1f); // 90%～110%の範囲でダメージが変動
                int amount = (int)((skillData.Amount + parent.Parent.StatusTracker.Atk.CurrentAmount * skillData.Magnification) * variation);
                return FLG.FLGCheckHaving((uint)skillData.DamageOptions ,(uint)DamageOptions.IsHeal) ? amount *= -1 : amount;
            }
        }


        // コルーチン管理のための参照
        private MonoBehaviour coroutineRunner;

        /// <summary>
        /// スキルのコンストラクタ。スキルデータとスキルハンドラーを受け取って初期化する。
        /// </summary>
        /// <param name="skillData">スキルデータ。</param>
        /// <param name="skillHandler">スキルハンドラー。</param>
        /// <param name="runner">コルーチンを実行するための MonoBehaviour インスタンス。</param>
        public Skill(SkillData skillData, IHandler skillHandler)
        {
            id = Guid.NewGuid();
            inAction = false;
            this.skillData = skillData;
            parent = skillHandler as SkillHandler;
            coroutineRunner = skillHandler as SkillHandler;
        }

        /// <summary>
        /// スキルのエフェクトやアニメーションを設定するメソッド。
        /// アクション中でなければアクション状態に移行し、エフェクトの処理を開始。
        /// </summary>
        /// <param name="targets">スキルの対象となるユニットのリスト。</param>
        public virtual void InvokeSkill(List<UnitBase> targets)
        {
            if (!inAction && CanUse)
            {
                coroutineRunner.StartCoroutine(ExecuteSkillCoroutine(targets));
            }
        }

        /// <summary>
        /// スキルのアクションを終了するメソッド。
        /// </summary>
        public virtual void EndAction()
        {
            if (inAction)
            {
                inAction = false;
            }
        }

        /// <summary>
        /// スキルを実行するコルーチン。
        /// エフェクトの再生、ダメージの適用を行い、完了後にアクション完了を通知。
        /// </summary>
        /// <param name="targets">スキルの対象となるユニットのリスト。</param>
        /// <returns></returns>
        private IEnumerator ExecuteSkillCoroutine(List<UnitBase> targets)
        {
            inAction = true;
            currentResult = null;

            // MPを消費
            StatusBase mp = parent.Parent.StatusTracker.CurrentMP;
            mp.CurrentAmount -= skillData.Cost;
            Debug.Log($"{parent.Parent.Name} : MPを消費：{mp.DefaultAmount}/{mp.CurrentAmount}");

            // スキル発動時のアニメーションを実行
            AnimationHandler.InvokeAnimation(parent.Parent.gameObject, skillData.AnimationType);

            // アニメーションの再生時間を待機（例: スキルデータに持たせる）
            yield return new WaitForSeconds(2.0f);

            //yield return ExecuteSkillCoroutine(targets);
            // スキルの効果を適用
            currentResult = ApplySkill(targets, currentMiniGameResult);

            // エフェクト処理が必要な場合、ここに追加
            // 例: エフェクトの再生、パーティクルの生成など

            // アクションを終了
            EndAction();

            // アクション完了を通知
            parent.Parent.NotifyActionComplete();

            yield break;
        }

        /// <summary>
        /// ここにミニゲームとの連絡を記述する
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator MiniGameActivator()
        {
            yield return null;

            // currentMiniGameResult;
        }
        /// <summary>
        /// スキルを対象のユニットに対して発動するメソッド。
        /// </summary>
        /// <param name="units">対象となるユニットのリスト。</param>
        public virtual List<DamageInfo> ApplySkill(List<UnitBase> units, MiniGameResult result)
        {
            if (units == null || units.Count == 0) { return null; }
            List<DamageInfo> infos = new List<DamageInfo>();

            foreach (UnitBase unit in units)
            {
                if (unit != null)
                {
                    DamageInfo info = new DamageInfo(parent.Parent, unit, skillData.DamageOptions);
                    CalculationDamage(unit, ref info);
                    // 各ユニットに対してダメージを与える
                    infos.Add(info);
                    unit.TakeDamage(info);
                }
            }
            return infos;
        }

        /// <summary>
        /// ダメージ計算用
        /// </summary>
        /// <param name="target">ターゲットのユニット</param>
        /// <returns>最終的なダメージ(回復量など)</returns>
        protected virtual void CalculationDamage(UnitBase target, ref DamageInfo info)
        {
            DamageOptions options = skillData.DamageOptions;
            float finalDamage = DamageAmount;// この時点では正の数

            if (!FLG.FLGCheck((uint)options, (uint)DamageOptions.IsFix))
            {
                // 会心の判定
                if (!FLG.FLGCheckHaving((uint)options, (uint)DamageOptions.ForceCrit) || UnityEngine.Random.Range(0, 1) < 0.15f)
                {
                    finalDamage *= 2;
                    info.isCrit = true;
                }

                // 固定ダメージや防御貫通スキルではない場合、防御力による減衰を計算
                if (!FLG.FLGCheck((uint)options, (uint)DamageOptions.IsPanetraitDefance))
                {
                    finalDamage = Mathf.Clamp(finalDamage - target.StatusTracker.Def.CurrentAmount / 2, DamageAmount / 7, 99999);
                }
            }
            // このスキルがダメージスキルの場合、負の値へ反転
            if (FLG.FLGCheck((uint)options, (uint)DamageOptions.IsDamage)) finalDamage *= -1;
            info.amount = (int)finalDamage;
        }
    }
}
