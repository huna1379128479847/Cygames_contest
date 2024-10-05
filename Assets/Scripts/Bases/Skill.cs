using System;
using System.Collections;
using System.Collections.Generic;
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
        private SkillFlgs skillFlgs;
        // スキルがダメージか回復かを示すフラグ
        private bool isBad;
        // スキルが攻撃かどうかを示すフラグ
        private bool isAttack;

        // スキルのユニークIDを返すプロパティ
        public Guid ID => id;

        /// <summary>
        /// スキルがアクション中 (エフェクト再生中) かどうかを示すプロパティ。
        /// </summary>
        public bool InAction => inAction;

        /// <summary>
        /// スキルのフラグ (攻撃、防御、バフなど) を取得するプロパティ。
        /// </summary>
        public SkillFlgs SkillFlgs => skillData.SkillFlgs;

        /// <summary>
        /// スキルが使用可能かどうかを示すプロパティ。
        /// MPが必要コスト以上あれば使用可能。
        /// </summary>
        public virtual bool CanUse
        {
            get
            {
                return parent.parent.statusTracker.CurrentMP.CurrentAmount >= skillData.Cost;
            }
        }

        /// <summary>
        /// スキルの基本的な効果量 (ダメージ) を計算するプロパティ。
        /// </summary>
        public virtual int DamageAmount
        {
            get
            {
                return (int)(skillData.Amount + parent.parent.statusTracker.Atk.CurrentAmount * skillData.Magnification);
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
        public Skill(SkillData skillData, SkillHandler skillHandler, MonoBehaviour runner)
        {
            id = Guid.NewGuid();
            this.skillData = skillData;
            parent = skillHandler;
            coroutineRunner = runner;
            inAction = false;
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

            // MPを消費
            parent.parent.statusTracker.CurrentMP.CurrentAmount -= skillData.Cost;

            // スキル発動時のアニメーションを実行
            AnimationHandler.InvokeAnimation(parent.parent.gameObject, skillData.AnimationType);

            // アニメーションの再生時間を待機（例: スキルデータに持たせる）
            yield return new WaitForSeconds(2.0f);

            // スキルの効果を適用
            ApplySkill(targets);

            // エフェクト処理が必要な場合、ここに追加
            // 例: エフェクトの再生、パーティクルの生成など

            // アクションを終了
            EndAction();

            // アクション完了を通知
            parent.parent.Notify_ActionComplete();

            yield break;
        }

        /// <summary>
        /// スキルを対象のユニットに対して発動するメソッド。
        /// ダメージを与える。
        /// </summary>
        /// <param name="units">対象となるユニットのリスト。</param>
        public virtual void ApplySkill(List<UnitBase> units)
        {
            if (units == null || units.Count == 0) { return; }

            foreach (UnitBase unit in units)
            {
                if (unit != null)
                {
                    // 各ユニットに対してダメージを与える
                    unit.TakeDamage(new DamageInfo(this, parent.parent, unit), false);
                }
            }
        }
    }
}
