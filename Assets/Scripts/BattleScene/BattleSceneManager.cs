using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Contest
{
    public class BattleSceneManager : SingletonBehavior<BattleSceneManager>, IManager
    {
        [SerializeField] private Transform canvas;
        private Turn currentTurn = Turn.Friend;
        private bool isRunning = false;
        private IFactoryHolders _factoryHolders;

        private List<GameObject> units = new List<GameObject>();
        private List<UnitBase> unitBases = new List<UnitBase>();

        private int friendCount = 0;
        private int enemyCount = 0;

        public virtual bool IsRunning
        {
            get => isRunning;
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;
                }
            }
        }

        public bool InAction => unitBases.Count > 0 && unitBases[0].InAction;
        public bool InSelecting => SkillSelectManager.instance.InSelecting;
        public Turn Turn => currentTurn;
        public List<UnitBase> AllUnit => unitBases;
        public int Friend => friendCount;
        public int Enemy => enemyCount;

        public IFactoryHolders FactoryHolders => _factoryHolders;

        /// <summary>
        /// バトルの実行を開始します。
        /// </summary>
        /// <param name="datas">バトルに参加するユニットのGameObjectリスト</param>
        public void Execute(List<GameObject> datas, InBattleEvent battleEvent, IFactoryHolders factoryHolders = null)
        {
            _factoryHolders = factoryHolders ?? new InBattleFactoryHolder(unitBases);
            StartCoroutine(InitializeBattle(datas));
        }

        /// <summary>
        /// バトルの初期化を行うコルーチン。
        /// </summary>
        /// <param name="datas">バトルに参加するユニットのGameObjectリスト</param>
        /// <returns></returns>
        private IEnumerator InitializeBattle(List<GameObject> datas)
        {
            Notify_StartInitialize();

            foreach (var obj in datas)
            {
                if (obj.TryGetComponent<UnitBase>(out var unitBase))
                {
                    Instantiate(obj, new Vector3(0, 0, 0), Quaternion.identity, canvas);
                    units.Add(obj);
                    unitBases.Add(unitBase);
                    ChangeUnitCount(unitBase);


                }
                yield return null; // 各ユニットの初期化間でフレームを分ける場合
            }
            yield return new WaitUntil(() => _factoryHolders.FinishedInit);
            Notify_FinishInitialize();
            // ターン管理を開始
            if (!isRunning)
            {
                StartCoroutine(TurnFlow());
            }
        }
        public void SortObjects()
        {
            foreach (var obj in unitBases)
            {

            }
        }
        /// <summary>
        /// ターンフローを管理するコルーチン。
        /// </summary>
        /// <returns></returns>
        private IEnumerator TurnFlow()
        {
            IsRunning = true;

            while (friendCount > 0 && enemyCount > 0)
            {
                // 各ユニットの行動を順番に処理
                foreach (var unit in unitBases.ToArray()) // ToArray() でコレクション変更時のエラーを防ぐ
                {
                    if (unit == null || !FLG.FLGCheck((uint)unit.MyUnitType, (uint)currentTurn))
                    {
                        continue;
                    }

                    yield return StartCoroutine(HandleUnitAction(unit));

                    // バトル終了の可能性をチェック
                    if (friendCount <= 0 || enemyCount <= 0)
                    {
                        break;
                    }
                }

                // ターン終了後にターンを切り替え
                TurnChange();
                yield return null; // 次のターンへ移行する前にフレームを分ける
            }

            IsRunning = false;
            EndBattle();
        }

        /// <summary>
        /// ユニットのアクションを処理するコルーチン。
        /// </summary>
        /// <param name="unit">処理するユニット</param>
        /// <returns></returns>
        private IEnumerator HandleUnitAction(UnitBase unit)
        {
            if (unit == null)
            {
                yield break;
            }

            bool actionCompleted = false;

            // イベントハンドラーの定義
            void OnActionCompleteHandler()
            {
                actionCompleted = true;
            }

            // イベントにハンドラーを登録
            unit.OnActionComplete += OnActionCompleteHandler;

            // ユニットのターンを開始
            unit.EnterTurn();

            // アクションが完了するまで待機
            while (!actionCompleted)
            {
                yield return null;
            }

            // イベントハンドラーを解除
            unit.OnActionComplete -= OnActionCompleteHandler;

            // ユニットのターンを終了
            unit.ExitTurn();
        }

        /// <summary>
        /// ターンを切り替えます（友軍 ⇄ 敵軍）。
        /// </summary>
        private void TurnChange()
        {
            currentTurn ^= Turn.All; // 味方と敵のターンを切り替え
            Debug.Log($"Turn changed to: {currentTurn}");
        }

        /// <summary>
        /// バトル終了時の処理を行います。
        /// </summary>
        private void EndBattle()
        {
            if (friendCount > 0 && enemyCount <= 0)
            {
                Debug.Log("Friends Win!");
            }
            else if (enemyCount > 0 && friendCount <= 0)
            {
                Debug.Log("Enemies Win!");
            }
            else
            {
                Debug.Log("Battle Ended");
            }
            Debug.Log($"\nfriend:{friendCount}\nenemy:{enemyCount}");
            // 必要に応じてバトル終了後の処理を追加
            // 例: 結果画面の表示、リトライオプションの提供など
        }

        /// <summary>
        /// ユニットのカウントを変更します。
        /// </summary>
        /// <param name="unitBase">カウントを変更するユニット</param>
        /// <param name="count">変更する数（デフォルトは1）</param>
        private void ChangeUnitCount(UnitBase unitBase, int count = 1)
        {
            if (FLG.FLGCheck((uint)unitBase.unitData.UnitType, (uint)UnitType.Enemy | (uint)UnitType.EnemyAI))
            {
                enemyCount += count;
            }
            else if (FLG.FLGCheck((uint)unitBase.unitData.UnitType, (uint)UnitType.Friend | (uint)UnitType.FriendAI))
            {
                friendCount += count;
            }
        }

        /// <summary>
        /// バトルが終了しているかをチェックし、必要な処理を行います。
        /// </summary>
        private void IsBattleEnd()
        {
            if (friendCount <= 0 || enemyCount <= 0)
            {
                IsRunning = false;
                StopAllCoroutines();
                EndBattle();
            }
        }

        /// <summary>
        /// ユニットのアクションが終了した際に呼び出されます。
        /// </summary>
        public void Notify_EndUnitAction()
        {
            // 現在の実装では不要ですが、将来的な拡張のために残しておきます。
            // 必要に応じて実装を追加してください。
        }

        /// <summary>
        /// バトルの初期化が開始された際に呼び出されます。
        /// </summary>
        protected virtual void Notify_StartInitialize()
        {
            // 初期化開始時の処理をここに追加
            Debug.Log("Battle Initialization Started.");
        }

        /// <summary>
        /// バトルの初期化が完了した際に呼び出されます。
        /// </summary>
        protected virtual void Notify_FinishInitialize()
        {
            // 初期化完了時の処理をここに追加
            Debug.Log("Battle Initialization Finished.");
        }

        /// <summary>
        /// ユニットをバトルから削除します。
        /// </summary>
        /// <param name="id">削除するユニットのID</param>
        public virtual void RemoveUnit(Guid id)
        {
            for (int i = 0; i < unitBases.Count; i++)
            {
                if (unitBases[i].ID == id)
                {
                    // カウントを減らす
                    ChangeUnitCount(unitBases[i], -1);

                    // ユニットとGameObjectをリストから削除
                    unitBases.RemoveAt(i);
                    Destroy(units[i]);
                    units.RemoveAt(i);

                    Debug.Log($"Unit {id} removed from battle.");

                    // バトル終了の可能性をチェック
                    IsBattleEnd();
                    break; // 一致するユニットが見つかったらループを抜ける
                }
            }
        }
    }

    public class SortHelper
    {
        public List<List<GameObject>> units = new List<List<GameObject>>()
        {
            new List<GameObject>(), // 味方
            new List<GameObject>()  // 敵
        };
        public float spacing = 2f;      // オブジェクト間の距離
        public Vector2 startPoint = new Vector2(-10, -10); // 配置開始位置（左下の座標）
        public Vector2 areaSize = new Vector2(20, 20); // 配置エリアのサイズ

        void SortingUnits(int num)
        {
            for (int i = 0; i < units[num].Count; i++)
            {
                // 配置する位置を計算
                Vector2 position = new Vector2(
                    startPoint.x,
                    startPoint.y + (i * spacing)  // Y軸の位置
                );
            }
        }
    }
}
