using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AnimateObjs : MonoBehaviour
{
    public GameObject[] objects;
    public Material material;
    public float fps;
    public bool reverse;

    [HideInInspector] private GameObject[] m_objects;
    [HideInInspector] private int object_index;
    [HideInInspector] private float nextActionTime = 0.0f;
    [HideInInspector] private float period;
    [HideInInspector] private bool animForward = true;

    // Start is called before the first frame update
    void Start()
    {
        m_objects = new GameObject[objects.Length];
        int i = 0;
        foreach (GameObject go in objects)
        {
            GameObject m_obj = Instantiate(go, new Vector3(0, 0, 0), Quaternion.identity);
            m_obj.GetComponentInChildren<MeshRenderer>().material = material;
            m_obj.SetActive(false);
            m_obj.transform.position = gameObject.transform.position;
            m_obj.transform.rotation = gameObject.transform.rotation;
            m_objects[i] = m_obj;
            i++;
        }
        //MeshRenderer newmeshrenderer = current_object.AddComponent<MeshRenderer>();
        //newmeshrenderer.material = material;
        object_index = 0;
        if (fps < 1)
        {
            Debug.LogError("Cannot set an fps lower than 1");
        }
        period = 1 / fps;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            // execute block of code here
            UpdateEverySecond();
        }
    }

    // Update is called once per second
    void UpdateEverySecond()
    {
        m_objects[object_index].SetActive(false);
        if (animForward)
        {
            object_index++;
        } else
        {
            object_index--;
        }
        
        if (object_index > objects.Length - 1)
        {
            if (reverse)
            {
                object_index = objects.Length - 2;
                animForward = false;
            } else
            {
                object_index = 0;
            }
        }

        if (object_index < 0)
        {
            if (reverse)
            {
                object_index = 1;
                animForward = true;
            }
            else
            {
                object_index = 0;
            }
        }

        m_objects[object_index].SetActive(true);
    }
}
