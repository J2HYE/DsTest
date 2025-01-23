using UnityEngine;

public class PopupFloatingText : MonoBehaviour
{
    private Camera _camera;

    void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if (_camera != null)
        {
            //Vector3 direction = _camera.transform.position - transform.position;
            //direction.x = direction.z = 0.0f; // X와 Z축의 회전을 제거하여 가로로 고정

            transform.LookAt(_camera.transform.position/* - direction*/);
        }
    }

    void PopUpDestroy()
    {
        Destroy(this.gameObject);
    }
}
