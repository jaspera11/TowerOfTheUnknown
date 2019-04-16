using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach to each particle's game object
// GameObject skillParticle; skillParticle.GetComponent<ParticleMotion>.PlaySkillAnim();
public class ParticleMotion : MonoBehaviour
{
    Vector3 start, end;
    readonly float speed = 10;

    private ParticleSystem ps;
    private bool moving;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
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
                var emission = ps.emission;
                emission.enabled = false;
                ps.Stop();
            }
        }
    }

	void PlaySkillAnim()
	{
        transform.position = start;
        moving = true;
        ps.Play();
        var emission = ps.emission;
        emission.enabled = false;
	}
}
