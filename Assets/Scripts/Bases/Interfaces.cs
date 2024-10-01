using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Contest
{
    public interface IUnit // ���j�b�g�p
    {
        string Name { get; }
        UnitType MyUnitType { get; }
        SkillHandler SkillHandler { get; }
        void TurnBehavior();
    }

    public interface IUniqueThing
    {
        Guid ID { get; }
    }
    public interface IPlayerHandler // �v���C���[�p�̑I��������
    {
        // �v���C���[�̑I�����𐶐����郁�\�b�h�̗�
        void GeneratePlayerOptions();

        // �v���C���[�̑I�������I�΂ꂽ���̏���
        void HandlePlayerSelection(int selectionId);
    }

    public interface IDoAction
    {
        bool InAction { get; }
    }

    public interface IEnemy//�G���j�b�g�p
    {
        BehaviorPattern BehaviorPattern { get; }
    }

    public interface IStatus//���j�b�g�p
    {
        StatusBase MaxHP { get;}
        StatusBase CurrentHP { get; }//current = ����
        StatusBase MaxMP { get; }
        StatusBase CurrentMP { get; }
        StatusBase MaxSpeed { get; }
        StatusBase CurrentSpeed { get; }
        StatusBase Atk { get; }
        StatusBase Def { get; }
    }

    public interface IManager
    {
        /// <summary>
        /// ���̃}�l�[�W���[�܂��̓V�[�����ғ������ǂ������Ǘ����܂��B
        /// �ғ����łȂ��ꍇ�A���ׂĂ̓��삪�ꎞ��~���܂��B
        /// </summary>
        bool IsRunning { get; set; }
    }
    public interface ISceneChanger
    {
        string FromSceneName { get; }
        string ToSceneName { get; }
        void Execute(string sceneName);
    }

    public interface IDataContainer
    {
        // �q�f�[�^�R���e�i��A�֘A����R���e�i�����ꍇ
        IDataContainer Parent { get; }

        // �R���e�i���̃f�[�^���擾���邽�߂̃��\�b�h
        object GetData();

        // �R���e�i�Ƀf�[�^���Z�b�g���邽�߂̃��\�b�h
        void SetData(object data);
    }
    public interface IEffect
    {
        string Name { get; }
        int Duration { get; }
        EffectTiming Timing { get; }
        EffectFlgs Flgs { get; }
        void Apply();
        void Remove();
        void UpdateStatsEffect();
        void DecreaseDuration(int time = 1);
        void ExecuteEffect(Action action = null);
    }

    public interface IHandler
    {

    }
}
