using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{    
    public Sound[] sounds;
    private bool underOneMinute = false;

    void Awake() {
       foreach(Sound sound in sounds){
           sound.source = gameObject.AddComponent<AudioSource>();
           sound.source.clip = sound.clip;
           sound.source.volume = sound.volume;
           sound.source.pitch = sound.pitch;
           sound.source.loop = sound.loop;
       } 
    }

    void Start() {
        Play("Restaurant");
    }

    void Update() {
        if(ReceiptsEngine.Instance.levelTime <= 60 && !underOneMinute) {
            sounds[0].pitch = 1.5f;
            underOneMinute = true;
        }
    }

    public void Play(string name) {
        foreach(Sound sound in sounds){
            if(sound.name == name){
                sound.source.Play();
                break;
            }

        }
    }

    public void Stop(string name){
        foreach(Sound sound in sounds){
            if(sound.name == name){
                sound.source.Stop();
                break;
            }

        }
    }

    public bool IsPlaying(string name){
        foreach(Sound sound in sounds){
            if(sound.name == name)
                return sound.source.isPlaying;
        }
        return false;
    }


}
