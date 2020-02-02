using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidParticleController : MonoBehaviour
{
    public enum FluidShape { Box, Sphere};

    public FluidShape shape;
    public float resolution = 10;
    public float diameter = 100;
    public float randomness = 0;
    public PhysicMaterial physicMaterial;
    public Material objectMaterial;

    private List<GameObject> particle_objs;
    private List<GameObject> particle_collider_objs;
    private float particle_diameter;

    // Start is called before the first frame update
    void Start()
    {
        particle_objs = new List<GameObject>();
        particle_collider_objs = new List<GameObject>();
        particle_diameter = diameter / resolution;
        float particle_radius = particle_diameter / 2;

        Vector3 particle_size = new Vector3(particle_diameter, particle_diameter, particle_diameter);
        Vector3 grid_size = new Vector3(resolution, resolution, resolution);

        if (shape == FluidShape.Box)
        {
            Vector3 start_position = gameObject.transform.position + new Vector3(particle_radius, particle_radius, particle_radius) + new Vector3(-diameter / 2, 0, (-diameter / 2));

            for (int z = 0; z < grid_size.z; z++)
            {
                for (int y = 0; y < grid_size.y; y++)
                {
                    for (int x = 0; x < grid_size.x; x++)
                    {
                        Vector3 particle_position = start_position + new Vector3(x * particle_diameter, y * particle_diameter, z * particle_diameter);
                        float random_range = randomness * particle_radius;
                        particle_position = particle_position + new Vector3(Random.Range(-random_range, random_range), Random.Range(-random_range, random_range), Random.Range(-random_range, random_range));
                        //create visible particle
                        GameObject particle_obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        particle_obj.transform.position = particle_position;
                        particle_obj.transform.localScale = particle_size;
                        particle_obj.GetComponent<MeshRenderer>().material = objectMaterial;
                        /*
                        particle_obj.AddComponent<LineRenderer>();
                        particle_obj.GetComponent<LineRenderer>().positionCount = 2;
                        particle_obj.GetComponent<LineRenderer>().widthMultiplier = 0.2f;
                        particle_obj.GetComponent<LineRenderer>().material = objectMaterial;
                        */
                        particle_obj.GetComponent<Collider>().enabled = false;
                        particle_objs.Add(particle_obj);
                        // create collider particle
                        GameObject particle_collider_obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        particle_collider_obj.transform.position = particle_position;
                        particle_collider_obj.transform.localScale = particle_size;
                        particle_collider_obj.GetComponent<Collider>().material = physicMaterial;
                        particle_collider_obj.GetComponent<MeshRenderer>().enabled = false;
                        particle_collider_obj.AddComponent<Rigidbody>();
                        particle_collider_objs.Add(particle_collider_obj);
                    }
                }
            }
        } else if (shape == FluidShape.Sphere)
        {
            Debug.LogError("Sphere not yet implimented");
        }
    }

    // Update is called once per frame
    void Update()
    {
        int p_index = 0;
        foreach (GameObject p_collider in particle_collider_objs)
        {
            particle_objs[p_index].transform.position = p_collider.transform.position;
            SortTargetsByDistance(particle_objs, particle_objs[p_index]);
            //List<GameObject> SortedTargets = new List<GameObject>(particle_objs);
            //SortedTargets.Sort(SortByDistanceToMe);
            p_index++;
        }
        /*
        //get location of all particals
        List<Transform> p_tfs = GetParticleTransforms(particle_collider_objs);
        int p_index = 0;
        foreach (Transform p_tf in p_tfs)
        {
            GameObject p_collider = particle_collider_objs[p_index];
            GameObject p = particle_objs[p_index];

            p.transform.position = p_collider.transform.position;

            float best_distance = Mathf.Infinity;
            Transform best_position = null;
            GetClosest(p_tfs, p_tf.position, ref best_distance, ref best_position);

            p.GetComponent<LineRenderer>().SetPosition(0, p_tf.position);
            if (best_distance < particle_diameter * 10)
            {
                p.GetComponent<LineRenderer>().SetPosition(1, best_position.position);
            } else
            {
                p.GetComponent<LineRenderer>().SetPosition(1, p_tf.position);
            }
            
            //Adjust size of object based on distance to closest
            float factor = 5f / best_distance;
            float new_particle_size = particle_diameter * factor;
            Vector3 particle_size = new Vector3(new_particle_size, new_particle_size, new_particle_size);
            particle_objs[p_index].transform.localScale = particle_size;

            p.GetComponent<LineRenderer>().widthMultiplier = particle_diameter * factor;

            p_index++;
        }
        */
    }
    /// <summary>
    ///  Sort Targets By Distance
    /// </summary>
    List<GameObject> SortTargetsByDistance(List<GameObject> targets, GameObject particle)
    {
        // TODO get form a dict <id, Unit> - later from Units in CurrentPlayerArea
        List<GameObject> SortedTargets = new List<GameObject>(targets);

        SortedTargets.Sort(
            (unit1, unit2) =>
            (particle.transform.position - unit1.transform.position).sqrMagnitude.CompareTo(
                (particle.transform.position - unit2.transform.position).sqrMagnitude)
            );
        return SortedTargets;
    }

    int SortByDistanceToMe(GameObject a, GameObject b)
    {
        float squaredRangeA = (a.transform.position - transform.position).sqrMagnitude;
        float squaredRangeB = (b.transform.position - transform.position).sqrMagnitude;
        return squaredRangeA.CompareTo(squaredRangeB);
    }

    List<Transform> GetParticleTransforms(List<GameObject> particles)
    {
        List<Transform> tfs = new List<Transform>();
        foreach (GameObject p in particles)
        {
            tfs.Add(p.transform);
        }
        return tfs;
    }

    void GetClosest(List<Transform> targets,Vector3 currentPosition,ref float best_distance,ref Transform best_position)
    {
        //best_position = null;
        //best_distance = Mathf.Infinity;
        foreach (Transform potentialTarget in targets)
        {
            if (potentialTarget.position != currentPosition)
            {
                Vector3 directionToTarget = potentialTarget.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < best_distance)
                {
                    best_distance = dSqrToTarget;
                    best_position = potentialTarget;
                }
            } 
        }
    }
}
