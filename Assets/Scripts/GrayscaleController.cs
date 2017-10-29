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

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
            Radius += Step * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            Radius -= Step * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
            Softness -= Step * Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
            Softness += Step * Time.deltaTime;

        Radius = Mathf.Clamp(Radius, 0, float.MaxValue);
        Softness = Mathf.Clamp(Softness, 0.3f, float.MaxValue);
        Shader.SetGlobalFloat("GRAYSCALE_Radius", Radius);
        Shader.SetGlobalFloat("GRAYSCALE_Softness", Softness);

        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            pos = Vector3.MoveTowards(pos, hit.point, Speed);
            Shader.SetGlobalVector("GRAYSCALE_Position", pos);
        }
    }
}
