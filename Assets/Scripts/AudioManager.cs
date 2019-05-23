using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] gunShots = new AudioClip[] { };
    public AudioClip[] gravelImpacts = new AudioClip[] { };
    public AudioClip[] shellBounces = new AudioClip[] { };
    public AudioClip[] bodyImpacts = new AudioClip[] { };
    public AudioClip[] emptyGuns = new AudioClip[] { };

    // Start is called before the first frame update
    public void PlaySFX (string type)
    {
        if (type == "Shot Hit")
        {
            SpawnSource(RandomFromList(gunShots), 0.7f, 0.9f, 0.8f, 1.2f, 0, 0);
            SpawnSource(RandomFromList(bodyImpacts), 0.4f, 0.6f, 0.8f, 1.2f, 0, Random.Range(0.1f, 0.2f));
            SpawnSource(RandomFromList(shellBounces), 0.1f, 0.3f, 0.8f, 1, 0, Random.Range(0.5f,1));
        }
        if (type == "Shot Miss")
        {
            SpawnSource(RandomFromList(gunShots), 0.7f, 0.9f, 0.8f, 1, 0, 0);
            SpawnSource(RandomFromList(gravelImpacts), 0.4f, 0.6f, 0.8f, 1.2f, 0, Random.Range(0.1f, 0.2f));
            SpawnSource(RandomFromList(shellBounces), 0.1f, 0.3f, 0.8f, 1, 0, Random.Range(0.5f, 1));
        }
        if (type == "Empty")
        {
            
            SpawnSource(RandomFromList(emptyGuns), 0.1f, 0.3f, 1f, 1.03f, 0, 0);
        }


    }

    private AudioClip RandomFromList (AudioClip[] audioClips)
    {
        if(audioClips.Length == 0)
        {
            return null;
        }
        int i = Mathf.RoundToInt(Random.Range(0, audioClips.Length - 1));
        return audioClips[i];

    }
    private void SpawnSource (AudioClip clip, float minV,float maxV,float minP,float maxP,float stereoShift, float delay)
    {
        GameObject tmpAudioPlayer = new GameObject();
        AudioSource audioS = tmpAudioPlayer.AddComponent<AudioSource>();
        audioS.clip = clip;
        audioS.volume = Random.Range(minV, maxV);
        audioS.pitch = Random.Range(minP, maxP);
        audioS.panStereo = stereoShift;
        audioS.PlayDelayed(delay);
        Destroy(tmpAudioPlayer, delay + 10);

    }
}
