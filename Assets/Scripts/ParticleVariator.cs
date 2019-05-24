using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleVariator : MonoBehaviour
{
    public float randomAngle = 15, lightParticleProbability = 10, flipProbability = 20, randomScaleDifference = 0.1f;
    public GameObject lightPraticle, intenseParticle, slimParticle;
    public bool isBlood = true;
    // Start is called before the first frame update
    void Awake()
    {
        
        
        
        
        if (isBlood)
        {
            if (Random.Range(0f, 100f) < flipProbability)
            {
                transform.Rotate(Vector3.forward * 180);
            }
            if (Random.Range(0f, 100f) < lightParticleProbability)
            {

                lightPraticle.SetActive(true);

            }
            else
            {
                lightPraticle.SetActive(false);
                Destroy(lightPraticle);
            }

            if (Random.Range(0f, 100f) < 50)
            {
                slimParticle.SetActive(true);
                intenseParticle.SetActive(false);
            }
            else
            {
                slimParticle.SetActive(false);
                intenseParticle.SetActive(true);
            }
        }
        



    }
    private void Start()
    {
        float a = Mathf.PerlinNoise(Time.timeSinceLevelLoad * 10, Time.timeSinceLevelLoad * 10);
        a *= 2;
        a -= 1;
        transform.localScale = transform.localScale + Vector3.one * a * randomScaleDifference;
       // transform.Rotate(transform.forward * a * randomAngle);
        
        
           // Debug.Log("weird rotation issue");
            transform.rotation = Quaternion.EulerAngles(0, 0, transform.eulerAngles.z + a * randomAngle);
        
    }


}
