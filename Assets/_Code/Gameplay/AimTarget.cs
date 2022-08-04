using System;
using System.Collections;
using UnityEngine;
using UpdateSys;

[Serializable]
public struct HumanBone
{
    [SerializeField] private HumanBodyBones _bone;
    [SerializeField, Range(0f, 1f)] private float _weight;

    #region "Properties"

    public HumanBodyBones Bone => _bone;
    public float Weight => _weight;

    #endregion
}

public class AimTarget : MonoBehaviour, ILateUpdatable
{
    [Header("Settings")]
    [SerializeField] private Transform _aimTransform = default;
    [SerializeField, Range(0f, 1f)] private float _weight = 1f;
    [SerializeField] private Vector3 _offset = Vector3.zero;

    [SerializeField] private HumanBone[] _humanBones = default;

    [Header("Limits")]
    [SerializeField] private float _angleLimit = 90.0f;
    [SerializeField] private float _distanceLimit = 1.5f;

    [SerializeField] private int _iterations = 10;

    [Header("Other Components")]
    [SerializeField] private Animator _animator = default;

    #region "Fields"

    private Transform[] _boneTransforms = default;
    private Transform _targetTransform = default;
    private Coroutine _coroutine = default;

    #endregion

    private void Awake()
    {
        _boneTransforms = GetBoneTransform();
    }

    private void OnDisable()
    {
        this.StopLateUpdate();
    }

    public void OnSystemLateUpdate(float deltaTime)
    {
        Vector3 targetPosition = GetTargetPosition() + _offset;
        for (int i = 0; i < _iterations; i++)
        {
            for (int b = 0; b < _boneTransforms.Length; b++)
            {
                Transform bone = _boneTransforms[b];
                float boneWeight = _humanBones[b].Weight * _weight;
                AimAtTarget(bone, targetPosition, boneWeight);
            }
        }
    }

    public void SetTarget(Transform target)
    {
        _animator.SetFloat("FiringBlend", 1f);
        _targetTransform = target;

        this.StartLateUpdate();

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        _coroutine = StartCoroutine(RemovingTargetDelay());
    }

    public void RemoveTarget()
    {
        _animator.SetFloat("FiringBlend", 0f);
        _targetTransform = null;

        this.StopLateUpdate();
    }

    private IEnumerator RemovingTargetDelay()
    {
        yield return new WaitForSeconds(0.4f);
        RemoveTarget();

        _coroutine = null;
    }

    private Transform[] GetBoneTransform()
    {
        Transform[] boneTransforms = new Transform[_humanBones.Length];
        for (int i = 0; i < boneTransforms.Length; i++)
        {
            boneTransforms[i] = _animator.GetBoneTransform(_humanBones[i].Bone);
        }

        return boneTransforms;
    }

    private Vector3 GetTargetPosition()
    {
        Vector3 targetDirection = _targetTransform.position - _aimTransform.position;
        Vector3 aimDirection = _aimTransform.forward;
        float blendOut = 0.0f;

        //float targetAngle = Vector3.Angle(targetDirection, aimDirection);
        //if (targetAngle > _angleLimit)
        //{
        //    blendOut += (targetAngle - _angleLimit) / 50.0f;
        //}

        //float targetDistance = targetDirection.magnitude;
        //if (targetDistance < _distanceLimit)
        //{
        //    blendOut += _distanceLimit - targetDistance;
        //}

        Vector3 direction = Vector3.Slerp(targetDirection, aimDirection, blendOut);
        return _aimTransform.position + direction;
    }

    private void AimAtTarget(Transform bone, Vector3 targetPosition, float weight)
    {
        Vector3 aimDirection = _aimTransform.forward;
        Vector3 targetDirection = targetPosition - _aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);
        Quaternion blendRotation = Quaternion.Slerp(Quaternion.identity, aimTowards, weight);
        bone.rotation = blendRotation * bone.rotation;
    }
}