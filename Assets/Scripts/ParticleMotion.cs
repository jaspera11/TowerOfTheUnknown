using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach to each particle's game object
// GameObject skillParticle; skillParticle.GetComponent<ParticleMotion>.PlaySkillAnim();
public class ParticleMotion : MonoBehaviour
{
    [SerializeField] Vector3 start, end;
    [SerializeField] float speed = 10;
    [SerializeField] private List<ParticleSystem> ps;
    private bool moving;

    // Start is called before the first frame update
    void Start()
    {
        moving = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moving)
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            if (transform.position.x >= end.x)
            {
                moving = false;
                foreach (ParticleSystem p in ps)
                {
                    p.enableEmission = false;
                    p.Stop();
                }
                Debug.Log("STOPPED");
            }
        }
    }

	public void PlaySkillAnim()
	{
        transform.position = start;
        moving = true;
        foreach (ParticleSystem p in ps)
        {
            p.Play();
            p.enableEmission = true;
        }
        Debug.Log("PLAY");
	}
}
