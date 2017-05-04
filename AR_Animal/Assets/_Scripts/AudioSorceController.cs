using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSorceController : MonoBehaviour {
    private static AudioSorceController _instance;
    public static AudioSorceController GetIntance() {
        return _instance;
    }
    public AudioClip[] AllAudioClip;
    private Dictionary<string, AudioClip> AudioDictionary = new Dictionary<string, AudioClip>();
    private AudioSource audioSource;
    // Use this for initialization
    void Start () {
        _instance = this;
        audioSource = GetComponent<AudioSource>();
        AudioDictionary.Add("daxiang(Clone)", AllAudioClip[0]);
        AudioDictionary.Add("banma(Clone)", AllAudioClip[1]);
        AudioDictionary.Add("huli(Clone)", AllAudioClip[2]);
        AudioDictionary.Add("shizi(Clone)", AllAudioClip[3]);
        AudioDictionary.Add("lang(Clone)", AllAudioClip[4]);
        AudioDictionary.Add("lingyang(Clone)", AllAudioClip[5]);
        AudioDictionary.Add("laohu(Clone)", AllAudioClip[6]);
        AudioDictionary.Add("baozi(Clone)", AllAudioClip[7]);
        AudioDictionary.Add("ma(Clone)", AllAudioClip[8]);
        AudioDictionary.Add("xionglu(Clone)", AllAudioClip[9]);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void PlayAudio(string audioClip) {

      
        if (AudioDictionary.ContainsKey(audioClip) == true)
        {
            audioSource.PlayOneShot(AudioDictionary[audioClip]);

        }
    }
}
