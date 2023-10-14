using System.Collections;
using Cinemachine;
using Oculus.Interaction;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
public class followPlayerIslam : MonoBehaviour
{
    //walking vars
    public GameObject bot;
    public Animator botAnim;
    public GameObject prayerBot;

    private XROrigin player;
    private float threshold = 2.0f;

    //Shared audio.
    [Serialize]
    public AudioClip welcomeAudio;
    [Serialize]
    public AudioSource backgroundMusic;
    public AudioSource audioPlayer;
    AudioClip previousClip = null;
    AudioClip clipToPlay = null;

    //Starting room audio.
    [Serialize]
    public AudioClip prayerCallAudio;
    [Serialize]
    public AudioClip hajjNudge;
    [Serialize]
    public AudioClip quranNudge;

    //Safa and Marwa audio.
    [Serialize]
    public AudioClip safaAudio;
    [Serialize]
    public AudioClip marwaAudio;

    //Previous Pillars audio.
    [Serialize]
    public AudioClip oldestPillarAudio;
    [Serialize]
    public AudioClip previousPillarAudio;

    //Current Pillars audio.
    [Serialize]
    public AudioClip firstPillarAudio;
    [Serialize]
    public AudioClip secondPillarAudio;
    [Serialize]
    public AudioClip thirdPillarAudio;
    [Serialize]
    public AudioClip previousPillarsAudio;
    [Serialize]
    public AudioClip[] stoneAudios;

    //Muzdalifah audio.
    [Serialize]
    public AudioClip pebbleNudgeAudio;
    [Serialize]
    public AudioClip peoplePrayingAudio;
    [Serialize]
    public AudioClip muzdalifahStartAudio;
    [Serialize]
    public AudioClip muzdalifahEndAudio;

    //Kaaba audio.
    [Serialize]
    public AudioClip hatimAudio;
    [Serialize]
    public AudioClip blackStoneAudio;
    [Serialize]
    public AudioClip sceneChangeAudio;

    //Arafat audio.
    [Serialize]
    public AudioClip arafatStartAudio;
    [Serialize]
    public AudioClip arafatEndAudio;

    //Tent City (Mina) audio.
    [Serialize]
    public AudioClip minaStartAudio;
    [Serialize]
    public AudioClip minaEndAudio;

    PhotonView view;
    string currentScene;
    GameObject currentPOI = null;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<XROrigin>();
        audioPlayer = bot.GetComponent<AudioSource>();
        view = PhotonView.Get(this);
        StartCoroutine(playWelcomeAudio());
        currentScene = SceneManager.GetActiveScene().name;
    }

    IEnumerator playWelcomeAudio()
    {
        yield return new WaitForSeconds(3);
        botAnim.SetBool("talk", true);
        StartCoroutine(playAudio(welcomeAudio));
        botAnim.SetBool("talk", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentScene != "Landing_Room" && currentScene != "Departure_Room")
        {
            Vector3 offset = player.transform.position - bot.transform.position;
            if (offset.z > threshold || offset.z < -threshold || offset.x > threshold || offset.x < -threshold)
            {
                moveBotToPosition();
            }
            else
            {
                botRechedPosition();
            }
        }
        if(this.audioPlayer.isPlaying)
        {
            botAnim.SetBool("talk", true);
        } else
        {
            botAnim.SetBool("talk", false);
        }
    }

    //walking functions

    void moveBotToPosition()
    {
        botAnim.SetBool("endWalking", false);
        botAnim.SetBool("startWalking", true);
        bot.transform.rotation = Quaternion.Slerp(bot.transform.rotation, Quaternion.LookRotation(player.transform.position - bot.transform.position), 3 * Time.deltaTime);
        bot.transform.position += transform.forward * Time.deltaTime * 2;
    }

    void botRechedPosition()
    {
        botAnim.SetBool("endWalking", true);
        botAnim.SetBool("startWalking", false);
    }

    //talking functions

    IEnumerator playAudio(AudioClip clip)
    {
        if (clip != null)
        {
            audioPlayer.clip = clip;
            audioPlayer.Play();
            double len = clip.length;
            while (len > 10)
            {
                yield return new WaitForSeconds(10);
                len -= 10;
            }
        }
    }

    public void PlayTriggerAudio(string trigger, GameObject poi)
    {
        if (trigger == "prayerExplanation" || trigger == "quranNudge")
        {
            Debug.Log("RPC If");
            view.RPC("PlayTriggerAudioInternal", RpcTarget.All, trigger);
        }
        else
        {
            if (poi)
            {
                currentPOI = poi;
            }
            
            switch (trigger)
            {
                case "hajjNudge":
                    clipToPlay = hajjNudge;
                    break;
                case "marwa":
                    clipToPlay = marwaAudio;
                    break;
                case "safa":
                    clipToPlay = safaAudio;
                    break;
                case "previousPillar":
                    clipToPlay = previousPillarAudio;
                    break;
                case "oldestPillar":
                    clipToPlay = oldestPillarAudio;
                    break;
                case "secondPillar":
                    clipToPlay = secondPillarAudio;
                    break;
                case "firstPillar":
                    clipToPlay = firstPillarAudio;
                    break;
                case "thirdPillar":
                    clipToPlay = thirdPillarAudio;
                    break;
                case "previousPillars":
                    clipToPlay = previousPillarsAudio;
                    break;
                case "peoplePraying":
                    clipToPlay = peoplePrayingAudio;
                    break;
                case "pebble":
                    clipToPlay = pebbleNudgeAudio;
                    break;
                case "muzdalifahStart":
                    clipToPlay = muzdalifahStartAudio;
                    break;
                case "muzdalifahEnd":
                    clipToPlay = muzdalifahEndAudio;
                    break;
                case "blackStone":
                    clipToPlay = blackStoneAudio;
                    break;
                case "hatim":
                    clipToPlay = hatimAudio;
                    break;
                case "sceneChange":
                    clipToPlay = sceneChangeAudio;
                    break;
                case "arafatEnd":
                    clipToPlay = arafatEndAudio;
                    break;
                case "arafatStart":
                    clipToPlay = arafatStartAudio;
                    break;
                case "minaEnd":
                    clipToPlay = minaEndAudio;
                    break;
                case "minaStart":
                    clipToPlay = minaStartAudio;
                    break;
                case "stoneThrowing":
                    int clipNumber = Random.Range(0, 4);
                    clipToPlay = stoneAudios[clipNumber];
                    break;
                default:
                    break;
            }

            if (currentPOI)
            {
                currentPOI.SetActive(false);
                currentPOI = null;
            }
            audioPlayer.clip = clipToPlay;
            previousClip = clipToPlay;
            audioPlayer.Play();
        }
    }

    [PunRPC]
    public void PlayTriggerAudioInternal(string trigger)
    {
        Debug.Log("Play Internal");
        switch (trigger)
        {
            case "prayerExplanation":
                clipToPlay = prayerCallAudio;
                currentPOI = GameObject.Find("POI Prayer");
                currentPOI.SetActive(false);
                this.gameObject.GetComponent<PrayerExplanation>().InitiatePrayer();
                GlobalVarsIslam.interactiveElementActive = true;
                break;
            case "quranNudge":
                clipToPlay = quranNudge;
                currentPOI = GameObject.Find("POI Quran");
                currentPOI.SetActive(false);
                this.gameObject.GetComponent<QuranExplanation>().InitiateOpenQuran();
                GlobalVarsIslam.interactiveElementActive = true;
                audioPlayer.clip = clipToPlay;
                previousClip = clipToPlay;
                audioPlayer.Play();
                break;
        }

        currentPOI = null;
    }

    public void SetPreviousClipToPrayer()
    {
        previousClip = prayerCallAudio;
    }

    public void ResetPreviousClip()
    {
        previousClip = null;
    }

    public void StopAudio()
    {
        this.gameObject.GetComponent<AudioSource>().Stop();

        if (previousClip == prayerCallAudio)
        {
            view.RPC("StopAudioInternal", RpcTarget.All, "prayerCall");
        }
        else if (previousClip == quranNudge)
        {
            view.RPC("StopAudioInternal", RpcTarget.All, "quranNudge");
        }
    }

    [PunRPC]
    public void StopAudioInternal(string trigger)
    {
        ResetPreviousClip();

        if (trigger == "prayerCall")
        {
            this.gameObject.GetComponent<AudioSource>().Stop();
            this.gameObject.GetComponent<PrayerExplanation>().CancelExplanation();
        } else if (trigger == "quranNudge") {
            this.gameObject.GetComponent<AudioSource>().Stop();
            this.gameObject.GetComponent<QuranExplanation>().InitiateCloseQuran();
        }
    }
}
