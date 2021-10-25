using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraManager: MonoBehaviour
{
    private GameManager _gameManager;
    private UIManager _uiManager;

    [SerializeField] private float _speedMove;
    [SerializeField] private float _speedScale;

    public bool enable;

    public void Initialize(GameManager gameManager, UIManager uiManager)
    {
        _gameManager = gameManager;
        _uiManager = uiManager;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.K))
            enable = false;
        if (Input.GetKey(KeyCode.L))
            enable = true;
        if (enable && !_uiManager.UiOpen())
        {
            Move();
            ScalingView();
        }
    }

    private void Move()
    {
        var camPos = transform.position;
        if (Input.mousePosition.x < 20)
        {
            camPos.x -= Time.deltaTime * _speedMove;
            camPos.z += Time.deltaTime * _speedMove;
        }
        if (Input.mousePosition.x > Screen.width - 20)
        {
            camPos.x += Time.deltaTime * _speedMove;
            camPos.z -= Time.deltaTime * _speedMove;
        }
        if (Input.mousePosition.y < 20)
        {
            camPos.x -= Time.deltaTime * _speedMove;
            camPos.z -= Time.deltaTime * _speedMove;
        }
        if (Input.mousePosition.y > Screen.height - 20)
        {
            camPos.x += Time.deltaTime * _speedMove;
            camPos.z += Time.deltaTime * _speedMove;
        }

        transform.position = new Vector3(Mathf.Clamp(camPos.x, _gameManager.SizeMap * -20, _gameManager.SizeMap*14 - 42), camPos.y, Mathf.Clamp(camPos.z, _gameManager.SizeMap * -20, _gameManager.SizeMap * 14 - 42));
    }

    private void ScalingView()
    {
        var camScale = transform.position;
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            camScale += new Vector3(-_speedScale/2, _speedScale, -_speedScale / 2);
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            camScale += new Vector3(_speedScale / 2, -_speedScale, _speedScale / 2);
        }

        if(camScale.y > 60 || camScale.y < 10) return;
        transform.position = camScale;
    }
}
