using UnityEngine;

public class PlayerAniAction : MonoBehaviour
{
    public void OnFootstep(AnimationEvent animationEvent)
    {
        SoundManager.Instance.PlayClipAtPoint("Ellen_Footsteps_Earth_Run_02", transform.position, 10f, false);
    }

    public void OnLand(AnimationEvent animationEvent)
    {
        SoundManager.Instance.PlayClipAtPoint("Ellen_Footsteps_Earth_Land_Walk_Forward_Landing_01", transform.position, 0.2f, false);
    }
}
