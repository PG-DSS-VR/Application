using Photon.Voice.Unity;
using UnityEngine;
using UnityEngine.Events;

public class Music : MonoBehaviour
{
    private bool audioPlaying = true;
    private bool transmittingAudio = true;


    public UnityEvent disableMusic;
    public UnityEvent enableMusic;
    public UnityEvent disableVoice;
    public UnityEvent enableVoice;

    public Recorder recorder;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void disableBackground() {
        GameObject music = GameObject.Find("background_music");
        AudioSource background = music.GetComponent<AudioSource>();
        if(background != null) {
            if (audioPlaying) {
                background.Pause();
                disableMusic.Invoke();
                audioPlaying = false;
            } else {
                background.Play();
                enableMusic.Invoke();
                audioPlaying = true;
            }
        }
    }

    public void disableVoiceChat() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("networkPlayer");
        if (transmittingAudio) {
            recorder.TransmitEnabled = false;
            foreach(GameObject xr in players) {
                AudioSource audio = (AudioSource)xr.GetComponent(typeof(AudioSource)) ?? null;
                if (audio) {
                    audio.mute = true;
                }
            }
            disableVoice.Invoke();
            transmittingAudio = false;
        } else {
            recorder.TransmitEnabled = true;
            foreach (GameObject xr in players) {
                AudioSource audio = (AudioSource)xr.GetComponentInParent(typeof(AudioSource)) ?? null;
                if (audio) {
                    audio.mute = false;
                }
            }
            enableVoice.Invoke();
            transmittingAudio = true;
        }

    }
}
