using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController INSTANCE { get; protected set; }

    public GameObject player;
    Vector3 offset;
    public float mouse_sensitivity;

    bool paused = false;

    // Use this for initialization
    void Start()
    {
        offset = new Vector3(0f, 5f, -20f);
        Cursor.visible = false;

        if (INSTANCE != null)
        {
            Debug.LogError("MORE THAN ONE CAMERACONTROLLER!!!!");
        }
        else
        {
            INSTANCE = this;
        }
    }

    void LateUpdate()
    {
        if (!paused)
        {
            float mouse_x = Input.GetAxis("Mouse X") * mouse_sensitivity;

            Camera.main.transform.position = player.transform.position;

            Camera.main.transform.Rotate(Vector3.up, mouse_x);

            Camera.main.transform.Translate(offset);
        }

    }

    public void Pause(bool newPause)
    {
        paused = newPause;
        Cursor.visible = paused;
    }
}
