using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Contest
{
    public interface IUnit // ���j�b�g�p
    {
        string Name { get; }
        UnitType MyUnitType { get; }
        UnitBase MyUnitBase { get; }
        SkillTracker SkillTracker { get; }
        void TurnBehavior();
    }

    public interface IUniqueThing
    {
        string ID { get; }
    }
    public interface IPlayerHandler // �v���C���[�p�̑I��������
    {
        // �v���C���[�̑I�����𐶐����郁�\�b�h�̗�
        void GeneratePlayerOptions();

        // �v���C���[�̑I�������I�΂ꂽ���̏���
        void HandlePlayerSelection(int selectionId);
    }

    public interface IEnemy//�G���j�b�g�p
    {
        BehaviorPattern BehaviorPattern { get; }
    }

    public interface IStats//���j�b�g�p
    {
        int MaxHP { get; }
        int CurrentHP { get; }//current = ����
        int MaxMP { get; }
        int CurrentMP { get; }
        int MaxSpeed { get; }
        int CurrentSpeed { get; }
        int Atk { get; }
        int Def { get; }
    }

    public interface IManager
    {
        /// <summary>
        /// ���̃}�l�[�W���[�܂��̓V�[�����ғ������ǂ������Ǘ����܂��B
        /// �ғ����łȂ��ꍇ�A���ׂĂ̓��삪�ꎞ��~���܂��B
        /// </summary>
        bool IsRunning { get; set; }

        /// <summary>
        /// ���̃}�l�[�W���[���C���X�^���X�����ꂽ�Ƃ����߂ɌĂ΂�郁�\�b�h�ł��B
        /// ��������Z�b�g�A�b�v�����������ōs���܂��B
        /// </summary>
        void Execute(List<object> Data);
    }
    public interface ISceneChanger
    {
        string FromSceneName { get;}
        string ToSceneName { get;}
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

}
