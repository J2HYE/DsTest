using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class OrbitCinemachine : MonoBehaviour
{
    public CinemachineOrbitalFollow cinemachineOrbitalFollow;
    public float orbitSpeed = 1f;
    Coroutine coroutine;
    private void Start() 
    {

    }
    private void OnEnable() 
    {
        cinemachineOrbitalFollow.TargetOffset.z = -4f;
        coroutine = StartCoroutine(CloseUp());
    }
    IEnumerator CloseUp()
    {
        yield return new WaitForSeconds(1.0f);
        while(cinemachineOrbitalFollow.TargetOffset.z != 2f)
        {
            cinemachineOrbitalFollow.TargetOffset.z += orbitSpeed * Time.deltaTime;
        }
        coroutine = null;
    }
    
}
