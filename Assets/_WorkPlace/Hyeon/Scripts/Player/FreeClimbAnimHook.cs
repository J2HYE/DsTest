using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LightTransport.PostProcessing;

public class FreeClimbAnimHook : MonoBehaviour
{
    Animator anim;

    IKSnapshot ikBase;
    IKSnapshot current = new IKSnapshot();
    IKSnapshot next = new IKSnapshot();
    IKGoals goals = new IKGoals();

    public float w_rh;
    public float w_lh;
    public float w_lf;
    public float w_rf;

    Vector3 rh, lh, rf, lf;
    Transform h;
    bool isMirror;
    bool isLeft;
    Vector3 prevMovDir;

    float delta;
    public float lerpSpeed = 1f;

    public void Init(ClimbTestPlayer c, Transform helper)
    {
        anim = c.anim;
        ikBase = c.baseIKsnapshot;
        h = helper;

    }

    public void CreatePositions(Vector3 origin, Vector3 moveDir, bool isMid)
    {
        delta = Time.deltaTime;
        HandleAnim(moveDir, isMid);

        if (isMid)
        {
            UpdateGoals(moveDir);
            prevMovDir = moveDir;
        }
        else
        {
            UpdateGoals(prevMovDir);
        }

        IKSnapshot ik = CreateSnapshot(origin);
        CopySnapShot(ref current, ik);

        SetIKPosition(isMid, goals.lf, current.lf, AvatarIKGoal.LeftFoot);
        SetIKPosition(isMid, goals.rf, current.rf, AvatarIKGoal.RightFoot);
        SetIKPosition(isMid, goals.lh, current.lh, AvatarIKGoal.LeftHand);
        SetIKPosition(isMid, goals.rh, current.rh, AvatarIKGoal.RightHand);

        UpdateIKWeight(AvatarIKGoal.LeftFoot, 1);
        UpdateIKWeight(AvatarIKGoal.RightFoot, 1);
        UpdateIKWeight(AvatarIKGoal.LeftHand, 1);
        UpdateIKWeight(AvatarIKGoal.RightHand, 1);
    }

    void UpdateGoals(Vector3 moveDir)
    {
        isLeft = (moveDir.x <= 0);

        if(moveDir.x != 0)
        {
            goals.lh = isLeft;
            goals.rh = !isLeft; 
            goals.lf = isLeft;
            goals.rf = !isLeft;
        }
        else
        {
            bool isEnabled = isMirror;
            if(moveDir.y < 0)
            {
                isEnabled = !isEnabled;
            }

            goals.lh = isEnabled;
            goals.rh = !isEnabled;
            goals.lf = isEnabled;
            goals.rf = !isEnabled;
        }
    }

    void HandleAnim(Vector3 moveDir, bool isMid)
    {
        if (isMid)
        {
            if(moveDir.y != 0)
            {
                if(moveDir.y < 0)
                {

                }
                else
                {

                }

                isMirror = !isMirror;
                anim.SetBool("Mirror", isMirror);
                anim.CrossFade("Ladder_Up_Play_InPlace", 0.2f);
            }
        }
        else
        {
            anim.CrossFade("Ladder_Idle", 0.2f);
        }
    }

    public IKSnapshot CreateSnapshot(Vector3 o)
    {
        IKSnapshot r = new IKSnapshot();
        Vector3 _lh = LocalToWorld(ikBase.lh);
        r.lh = GetPosActual(_lh);
        Vector3 _rh = LocalToWorld(ikBase.rh);
        r.rh = GetPosActual(_rh);
        Vector3 _lf = LocalToWorld(ikBase.lf);
        r.lf = GetPosActual(_lf);
        Vector3 _rf = LocalToWorld(ikBase.rf);
        r.rf = GetPosActual(_rf);
        return r;
    }

    public float wallOffset = 0f;

    public void CopySnapShot(ref IKSnapshot to, IKSnapshot from)
    {
        to.rh = from.rh;
        to.lh = from.lh;
        to.lf = from.lf;
        to.rf = from.rf;
    }

    Vector3 GetPosActual(Vector3 o)
    {
        Vector3 r = o;
        Vector3 origin = o;
        Vector3 dir = h.forward;
        origin += -(dir * 0.2f);
        RaycastHit hit;
        if(Physics.Raycast(origin, dir, out hit, 1.5f))
        {
            Vector3 _r = hit.point + (hit.normal * wallOffset);
            r = _r;
        }

        return r;
    }

    Vector3 LocalToWorld(Vector3 p)
    {
        Vector3 r = h.position;
        r += h.right * p.x;
        r += h.forward * p.z;
        r += h.up * p.y;
        return r;
    }

    void SetIKPosition(bool isMid, bool isTrue, Vector3 pos, AvatarIKGoal goal)
    {
        if (isMid)
        {
            if (isTrue)
            {
                Vector3 p = GetPosActual(pos);
                UpdateIKPosition(goal, p);
            }
        }
        else
        {
            if (!isTrue)
            {
                Vector3 p = GetPosActual(pos);
                UpdateIKPosition(goal, p);
            }
        }
    }

    public void UpdateIKPosition(AvatarIKGoal goal, Vector3 pos)
    {
        switch (goal)
        {
            case AvatarIKGoal.LeftFoot:
                lf = pos;
                break;
            case AvatarIKGoal.RightFoot:
                rf = pos;
                break;
            case AvatarIKGoal.LeftHand:
                lh = pos;
                break;
            case AvatarIKGoal.RightHand:
                rh = pos;
                break;
        }
    }

    public void UpdateIKWeight(AvatarIKGoal goal, float w)
    {
        switch (goal)
        {
            case AvatarIKGoal.LeftFoot:
                w_lf = w;
                break;
            case AvatarIKGoal.RightFoot:
                w_rf = w;
                break;
            case AvatarIKGoal.LeftHand:
                w_lh = w;
                break;
            case AvatarIKGoal.RightHand:
                w_rh = w;
                break;
        }
    }



    void OnAnimatorIK()
    {
        delta = Time.deltaTime;

        SetIKPos(AvatarIKGoal.LeftHand, lh, w_lh);
        SetIKPos(AvatarIKGoal.RightHand, rh, w_rh);
        SetIKPos(AvatarIKGoal.LeftFoot, lf, w_lf);
        SetIKPos(AvatarIKGoal.RightFoot, rf, w_rf);
    }

    void SetIKPos(AvatarIKGoal goal, Vector3 tp, float w)
    {
        IKStates ikState = GetIKStates(goal);
        if(ikState == null)
        {
            ikState = new IKStates();
            ikState.goal = goal;
            ikStates.Add(ikState);
        }

        if (w == 0)
        {
            ikState.isSet = false;
        }

        if (!ikState.isSet)
        {
            ikState.position = GoalToBodyBones(goal).position;
            ikState.isSet = true;
        }

        ikState.positionWeight = w;
        ikState.position = Vector3.Lerp(ikState.position, tp, delta * lerpSpeed);


        anim.SetIKPositionWeight(goal, ikState.positionWeight);
        anim.SetIKPosition(goal, ikState.position);
    }

    Transform GoalToBodyBones(AvatarIKGoal goal)
    {
        switch (goal)
        {
            case AvatarIKGoal.LeftFoot:
                return anim.GetBoneTransform(HumanBodyBones.LeftFoot);
            case AvatarIKGoal.RightFoot:
                return anim.GetBoneTransform(HumanBodyBones.RightFoot);
            case AvatarIKGoal.LeftHand:
                return anim.GetBoneTransform(HumanBodyBones.LeftHand);
            default:
            case AvatarIKGoal.RightHand:
                return anim.GetBoneTransform(HumanBodyBones.RightHand);
        }
    }

    IKStates GetIKStates(AvatarIKGoal goal)
    {
        IKStates r = null;
        foreach(IKStates i in ikStates)
        {
            if(i.goal == goal)
            {
                r = i;
                break;
            }
        }

        return r; 
    }

    List<IKStates> ikStates = new List<IKStates>();
    class IKStates
    {
        public AvatarIKGoal goal;
        public Vector3 position;
        public float positionWeight;
        public bool isSet = false;
    }
}

public class IKGoals
{
    public bool rh;
    public bool lh;
    public bool rf;
    public bool lf;
}
