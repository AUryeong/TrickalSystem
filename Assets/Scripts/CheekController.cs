using System.Collections;
using Spine;
using Spine.Unity;
using UnityEngine;

public class CheekController : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation skeletonAnimation;

    [Header("Collider")]
    [SerializeField] private BoxCollider2D cheekAbleCollider;
    [SerializeField] private BoxCollider2D cheekMaxCollider;

    private Bounds cheekMaxBounds;

    private bool isCheekPulling;

    private Bone cheekBone;
    private Vector2 cheekBoneStartPos;

    private Vector3 inputStartPosition;

    private Coroutine cheekBackCoroutine;
    private readonly WaitForSeconds cheekReturnFaceWait = new(0.2f);

    private void Awake()
    {
        cheekBone = skeletonAnimation.Skeleton.FindBone("Cheek Bone");
        cheekBoneStartPos = cheekBone.GetLocalPosition();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            inputStartPosition = CameraManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
            inputStartPosition.z = 0;

            if (!cheekAbleCollider.bounds.Contains(inputStartPosition)) return;

            if (cheekBackCoroutine != null)
                StopCoroutine(cheekBackCoroutine);
            
            cheekMaxBounds = cheekMaxCollider.bounds;
            cheekMaxBounds.center += cheekAbleCollider.bounds.center - inputStartPosition;

            skeletonAnimation.state.SetAnimation(0, "animation2", false);
            isCheekPulling = true;
        }

        if (!isCheekPulling) return;

        if (Input.GetMouseButtonUp(0))
        {
            cheekBackCoroutine = StartCoroutine(CheekBackCoroutine());
            isCheekPulling = false;
            return;
        }

        var inputPosition = CameraManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
        inputPosition.z = 0;
        
        Vector2 subtractVector =  cheekMaxBounds.ClosestPoint(inputPosition) - inputStartPosition;
        var vector = subtractVector + cheekBoneStartPos;

        cheekBone.SetLocalPosition(vector);
    }

    private IEnumerator CheekBackCoroutine()
    {
        var prevBonePos = new Vector2(cheekBone.X, cheekBone.Y);
        const float timeMultiplier = 1.25f;

        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime * timeMultiplier;

            var vector = Vector2.LerpUnclamped(prevBonePos, cheekBoneStartPos, EaseUtility.GetEaseOutElastic(time));
            cheekBone.SetLocalPosition(vector);
            yield return null;
        }

        yield return cheekReturnFaceWait;

        skeletonAnimation.state.SetAnimation(0, "animation", false);
    }
}