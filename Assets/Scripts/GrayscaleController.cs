// ==============================
// Copyright (c) cobalt910
// Neogene Games
// http://youtube.com/cobalt9101/
// ==============================

using UnityEngine;

public class GrayscaleController : MonoBehaviour 
{
    public float Step;
    public float Speed;
    public float Radius;
    public float Softness;

    private Camera cam;
    private Ray ray;
    private RaycastHit hit;
    private Vector3 pos;

    private void Awake() => cam = GetComponent<Camera>();
    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Radius += (Step + Softness) * Time.deltaTime;
            ClampRadius();
        }
        if (Input.GetKey(KeyCode.S))
        {
            Radius -= (Step + Softness) * Time.deltaTime;
            ClampRadius();
        }
        if (Input.GetKey(KeyCode.A))
        {
            Softness -= Step * Time.deltaTime;
            ClampSoftness();
        }
        if (Input.GetKey(KeyCode.D))
        {
            Softness += Step * Time.deltaTime;
            ClampSoftness();
        }

        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            pos = Vector3.MoveTowards(pos, hit.point, Speed * Time.deltaTime);
            Shader.SetGlobalVector("GRAYSCALE_Position", pos);
        }
    }

    private void ClampRadius()
    {
        Radius = Mathf.Clamp(Radius, 0, float.MaxValue);
        Shader.SetGlobalFloat("GRAYSCALE_Radius", Radius);
    }

    private void ClampSoftness()
    {
        Softness = Mathf.Clamp(Softness, 0.3f, float.MaxValue);
        Shader.SetGlobalFloat("GRAYSCALE_Softness", Softness);
    }
}