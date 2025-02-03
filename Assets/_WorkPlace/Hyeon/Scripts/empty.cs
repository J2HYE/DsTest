using UnityEngine;

public class empty : MonoBehaviour
{
    PlayerController controller;

    void OnFootstep(AnimationEvent animationEvent)
    {
        SoundManager.Instance.PlayClipAtPoint("Ellen_Footsteps_Earth_Run_02", transform.position, 10f, false);
    }

    void OnLand(AnimationEvent animationEvent)
    {
        SoundManager.Instance.PlayClipAtPoint("Ellen_Footsteps_Earth_Land_Walk_Forward_Landing_01", transform.position, 0.2f, false);
    }

    private void EnableCollider()
    {
        //transform.GetComponentInParent<PlayerCombat>().weaponCollider.enabled = true;
    }

    private void DisableCollider()
    {
        //transform.GetComponentInParent<PlayerCombat>().weaponCollider.enabled = false;
    }

    private void EnableComboInput()
    {
        //transform.GetComponentInParent<PlayerCombat>().CanReceiveInput = true;
    }

    private void StartCombo()
    {
        //transform.GetComponentInParent<PlayerCombat>().CanReceiveInput = false;
    }
}
