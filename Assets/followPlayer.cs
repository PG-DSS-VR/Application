using System;
using System.Collections;
using Cinemachine;
using Oculus.Interaction;
using Photon.Pun;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
public class followPlayer : MonoBehaviour
{
    //walking vars
    public GameObject bot;
    public Animator botAnim;

    private XROrigin player;
    private GameObject camera;
    private float threshold = 2.0f;

    public GameObject POIConfession;

    //talking vars
    [Serialize]
    public AudioClip welcomeAudio;
    [Serialize]
    public AudioClip afterEntranceAudio;
    [Serialize]
    public AudioClip afterEucharistAudio;
    [Serialize]
    public AudioClip insideConfessionAudio;
    [Serialize]
    public AudioClip outsideConfessionAudio;
    public AudioSource audioPlayer;

    //predefined animations
    //Entry
    public CinemachineDollyCart priest;
    public CinemachineDollyCart priestfollow1;
    public CinemachineDollyCart priestfollow2;
    public Animator priestAnim;
    public Animator priestfollow1Anim;
    public Animator priestfollow2Anim;
    public GameObject priestObject;
    public GameObject priestfollow1Object;
    public GameObject priestfollow2Object;
    public AudioSource entranceMusic;
    //confession

    //Eucharist
    private bool animationTriggered = false;
    private bool rotateToPlayer = false;
    private Quaternion oldRotation;
    private bool firstConfessionTriggered = false;
    public GameObject hostie;
    public GameObject goblet;
    public Animator priestEucharist;
    public AudioSource eucharistAudio;
    public GameObject priestEuch;
    public GameObject hostiePan;
    public GameObject bible;
    public GameObject oil;
    private state_possible state = state_possible.START;
    private bool isPaused = false;
    PhotonView view;

    private enum state_possible {
        START,
        ENTRANCE,
        ENTRANCEEND,
        EUCHARIST,
        EUCHARISTEND,
        CONFESSION,
        CONFESSIONEND,
        CONFESSIONTWO,
        CONFESSIONTWOEND    
    }


    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<XROrigin>();
        camera = player.transform.GetChild(0).gameObject;
        audioPlayer = bot.GetComponent<AudioSource>();
        oldRotation = bot.transform.rotation;
        StartCoroutine(playWelcomeAudio());
        //view = PhotonView.Get(this);
        view = player.GetComponentInParent<PhotonView>();
        //view.RPC("playWelcomeAudioRPC", RpcTarget.All);
        //StartCoroutine(test());
        //entranceAnimation();
        //StartCoroutine(test());
        //StartCoroutine(firstClip());
        //startEucharist();
    }

    public void EndAnimation() {

    }

    IEnumerator test() {

        entranceAnimation();
        yield return new WaitForSeconds(3);
        AbortAnimation();
        yield return new WaitForSeconds(3);
        entranceAnimation();
        yield return new WaitForSeconds(10);
        AbortAnimation();
        yield return new WaitForSeconds(3);
        entranceAnimation();
    }

    private bool welcomeAudioPlaying = false;

    [PunRPC]
    public void playWelcomeAudioRPC() {
        if (welcomeAudioPlaying) {
            return;
        }
        welcomeAudioPlaying = true;
        StartCoroutine(playWelcomeAudio());
    }

    IEnumerator playWelcomeAudio() {

        yield return new WaitForSeconds(4);
        StartCoroutine(playAudio(welcomeAudio));
        StartCoroutine(checkWhenMusicFinish(audioPlayer, welcomeMusic: true));
    }

    // Update is called once per frame
    void Update()
    {

        //bool endAnimation = OVRInput.GetDown(OVRInput.Button.Three);
        //if (Input.GetKeyDown(KeyCode.X)) {
        //    endAnimation = true;
        //}
        //bool pause = OVRInput.GetDown(OVRInput.Button.Four);
        //if (endAnimation) {
        //    StartCoroutine(AbortAnimationRPC());
        //}
        //if (pause) {
        //    StartCoroutine(PauseAnimationRPC());
        //}
        if (checkPositionCamera) {
            if(player.transform.position.x > cameraDownPos.x + 0.5f ||
                player.transform.position.x < cameraDownPos.x - 0.5f ||
                player.transform.position.y > cameraDownPos.y + 0.5f ||
                player.transform.position.y > cameraDownPos.y - 0.5f ||
                player.transform.position.z > cameraDownPos.z + 0.5f ||
                player.transform.position.z > cameraDownPos.z - 0.5f) {
                cameraBack();
            }
        }
        if (!animationTriggered) {
            if(player == null) {
                player = FindObjectOfType<XROrigin>();
            }
            Vector3 offset = player.transform.position - bot.transform.position;
            if (offset.z > threshold || offset.z < -threshold || offset.x > threshold || offset.x < -threshold) {
                moveBotToPosition();
            } else {
                botRechedPosition();
            }
        }
        if (rotateToPlayer) {
            bot.transform.rotation = Quaternion.Slerp(bot.transform.rotation, Quaternion.LookRotation(player.transform.position - bot.transform.position), 3 * Time.deltaTime);
            if (oldRotation.Equals(bot.transform.rotation)) {
                rotateToPlayer = false;
            }
            oldRotation = bot.transform.rotation;
        }
        if (rotatePriestToHostie) {
            //hostie.transform.rotation = Quaternion.Slerp(hostie.transform.rotation, Quaternion.LookRotation(new Vector3(0, 18.139f, 0)), 3 * Time.deltaTime);
            //hostie.transform.rotation = Quaternion.Slerp(hostie.transform.rotation, new Quaternion(hostieQuat.x - 90, hostieQuat.y, hostieQuat.z, hostieQuat.w), 3 * Time.deltaTime);
            hostie.transform.eulerAngles = Vector3.Lerp(hostie.transform.eulerAngles, new Vector3(0, hostie.transform.eulerAngles.y, hostie.transform.eulerAngles.z), 3 * Time.deltaTime);
            //priestEuch.transform.rotation = Quaternion.Slerp(priestEuch.transform.rotation, new Quaternion(priestQuat.x, priestQuat.y+ 18.139f, priestQuat.z, priestQuat.w), 3 * Time.deltaTime);
            priestEuch.transform.eulerAngles = Vector3.Lerp(priestEuch.transform.eulerAngles, new Vector3(0, 198.139f, 0), 3 * Time.deltaTime);
        }
        if (rotatePriestToGoblet) {
            //hostie.transform.rotation = Quaternion.Slerp(hostie.transform.rotation, new Quaternion(hostieQuat.x + 90, hostieQuat.y, hostieQuat.z, hostieQuat.w), 3 * Time.deltaTime);
            hostie.transform.eulerAngles = Vector3.Lerp(hostie.transform.eulerAngles, new Vector3(90, hostie.transform.eulerAngles.y, hostie.transform.eulerAngles.z), 3 * Time.deltaTime);
            //priestEuch.transform.rotation = Quaternion.Slerp(priestEuch.transform.rotation, Quaternion.LookRotation(new Vector3(0, -42.262f, 0)), 3 * Time.deltaTime);
            //priestEuch.transform.rotation = Quaternion.Slerp(priestEuch.transform.rotation, new Quaternion(priestQuat.x, priestQuat.y - 42.262f, priestQuat.z, priestQuat.w), 3 * Time.deltaTime);
            priestEuch.transform.eulerAngles = Vector3.Lerp(priestEuch.transform.eulerAngles, new Vector3(0, 156.877f, 0), 3 * Time.deltaTime);
        }
        if (rotatePriestBack) {
            //priestEuch.transform.rotation = Quaternion.Slerp(priestEuch.transform.rotation, Quaternion.LookRotation(new Vector3(priestQuat.x, 23.123f, priestQuat.z)), 3 * Time.deltaTime);
            //priestEuch.transform.rotation = Quaternion.Slerp(priestEuch.transform.rotation, new Quaternion(priestQuat.x, priestQuat.y + 23.123f, priestQuat.z, priestQuat.w), 3 * Time.deltaTime);
            priestEuch.transform.eulerAngles = Vector3.Lerp(priestEuch.transform.eulerAngles, new Vector3(0, 180, 0), 3 * Time.deltaTime);
        }
        if (hostieFirstPosition) {
            hostie.transform.position = Vector3.Lerp(hostie.transform.position, new Vector3(18.843f, 1.954f, 46.805f), 1 * Time.deltaTime);
        }
        if(hostieHigh) {
            hostie.transform.position = Vector3.Lerp(hostie.transform.position, new Vector3(18.825f, 2.311f, 46.75f), 2 * Time.deltaTime);
        }
        if (hostieBack) {
            hostie.transform.position = Vector3.Lerp(hostie.transform.position, new Vector3(18.80441f, 1.7171f, 46.68539f), 3 * Time.deltaTime);
        }
        if (gobletFirst) {
            goblet.transform.position = Vector3.Lerp(goblet.transform.position, new Vector3(19.061f, 1.918f, 46.798f), 1 * Time.deltaTime);
        }
        if (gobletHigh) {
            goblet.transform.position = Vector3.Lerp(goblet.transform.position, new Vector3(19.129f, 2.265f, 46.778f), 2 * Time.deltaTime);
        }
        if (gobletBack) {
            goblet.transform.position = Vector3.Lerp(goblet.transform.position, new Vector3(19.086f, 1.682583f, 46.798f ), 3 * Time.deltaTime);
        }
    }

    public IEnumerator AbortAnimationRPC() {
        //view.RPC("AbortAnimation", RpcTarget.All);
        view.RPC("Abort", RpcTarget.All);
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator PauseAnimationRPC() {
        //view.RPC("PauseAnimation", RpcTarget.All);
        //view.RPC("Pause", RpcTarget.All);
        yield return new WaitForEndOfFrame();
    }

    [PunRPC]
    public void AbortAnimation() {
        aborted = true;
        isPaused = false;
        animationTriggered = false;
        audioPlayer.Stop();
        if(SceneManager.GetActiveScene().name == "Catholic_church") {
            switch (state) {
                case state_possible.START:
                    break;
                case state_possible.CONFESSION:
                    try {
                        outsideFinished = true;
                        outsideFinishedAnimation.Invoke();
                        state = state_possible.CONFESSIONEND;
                        confessionStarted = false;
                    } catch (Exception e) {

                    }
                    state = state_possible.START;
                    break;
                case state_possible.CONFESSIONEND:
                    break;
                case state_possible.ENTRANCE:
                    priest.m_Speed = 0;
                    priest.m_Position = 0;
                    priestfollow1.m_Speed = 0;
                    priestfollow1.m_Position = 0;
                    priestfollow2.m_Speed = 0;
                    priestfollow2.m_Position = 0;
                    priestAnim.SetBool("idle", false);
                    priestAnim.SetBool("pray2", false);
                    priestAnim.SetBool("pray1", false);
                    priestAnim.SetBool("walkEnd", false);
                    priestAnim.SetBool("restart", true);
                    priestfollow2Anim.SetBool("walkEnd", false);
                    priestfollow1Anim.SetBool("walkEnd", false);
                    priestfollow2Anim.SetBool("restart", true);
                    priestfollow1Anim.SetBool("restart", true);
                    priestObject.SetActive(false);
                    priestfollow1Object.SetActive(false);
                    priestfollow2Object.SetActive(false);
                    priestEuch.SetActive(true);
                    entranceMusic.Stop();
                    try {
                        StopCoroutine(checkWhenFinishedPriest(priest, priestAnim));
                        StopCoroutine(checkWhenFinished(priestfollow1, priestfollow1Anim));
                        StopCoroutine(checkWhenFinished(priestfollow2, priestfollow2Anim));

                    } catch (Exception e) {

                    }
                    state = state_possible.ENTRANCEEND;
                    break;
                case state_possible.ENTRANCEEND:
                    break;
                case state_possible.EUCHARIST:
                    hostie.SetActive(false);
                    hostiePan.SetActive(false);
                    goblet.SetActive(false);
                    oil.SetActive(false);
                    bible.SetActive(false);
                    try {
                        StopCoroutine(timerPriestMovement());
                    } catch (Exception e) {

                    }
                    priestEucharist.SetBool("small", false);
                    priestEucharist.SetBool("cross", false);
                    priestEucharist.SetBool("wide", false);
                    priestEucharist.SetBool("hostie", false);
                    priestEucharist.SetBool("hostie2", false);
                    priestEucharist.SetBool("wine", false);
                    priestEucharist.SetBool("wine2", false);
                    priestEucharist.SetBool("wine3", false);
                    priestEucharist.SetBool("above", false);
                    priestEucharist.SetBool("small2", false);
                    priestEucharist.SetBool("wide2", false);
                    priestEucharist.SetBool("restart", true);
                    POIConfession.SetActive(true);
                    eucharistAudio.Stop();
                    priestEuch.SetActive(false);
                    state = state_possible.START;
                    break;
                case state_possible.EUCHARISTEND:
                    break;
                case state_possible.CONFESSIONTWO:
                    confessionFinished();
                    secondConfessionStarted = false;
                    break;
                case state_possible.CONFESSIONTWOEND:
                    break;
            }
        }
    }

    [PunRPC]
    public void PauseAnimation() {
        bool paused = isPaused;
        if (!isPaused) {
            isPaused = true;
        } else {
            isPaused = false;
        }
        if (!paused) {
            audioPlayer.Pause();
        } else {
            audioPlayer.Play();
        }
        if (SceneManager.GetActiveScene().name == "Catholic_church") {
            switch (state) {
                case state_possible.START:
                    if (!paused) {

                    } else {

                    }
                    break;
                case state_possible.CONFESSION:
                    if (!paused) {

                    } else {

                    }
                    break;
                case state_possible.CONFESSIONEND:
                    if (!paused) {

                    } else {

                    }
                    break;
                case state_possible.ENTRANCE:
                    if (!paused) {
                        priest.m_Speed = 0;
                        priestfollow1.m_Speed = 0;
                        priestfollow2.m_Speed = 0;
                        priestAnim.speed = 0;
                        priestfollow1Anim.speed = 0;
                        priestfollow2Anim.speed = 0;
                        entranceMusic.Pause();
                    } else {
                        priest.m_Speed = 0.6f;
                        priestfollow1.m_Speed = 0.6f;
                        priestfollow2.m_Speed = 0.6f;
                        priestAnim.speed = 1;
                        priestfollow1Anim.speed = 1;
                        priestfollow2Anim.speed = 1;
                        entranceMusic.Play();
                    }
                    break;
                case state_possible.ENTRANCEEND:
                    if (!paused) {

                    } else {

                    }
                    break;
                case state_possible.EUCHARIST:
                    if (!paused) {
                        priestEucharist.speed = 0;
                        eucharistAudio.Pause();
                    } else {
                        priestEucharist.speed = 1;
                        eucharistAudio.Play();
                    }
                    break;
                case state_possible.EUCHARISTEND:
                    if (!paused) {

                    } else {

                    }
                    break;
                case state_possible.CONFESSIONTWO:
                    if (!paused) {
                        confessionFinished();
                        secondConfessionStarted = false;
                    } else {

                    }
                    break;
                case state_possible.CONFESSIONTWOEND:
                    if (!paused) {

                    } else {

                    }
                    break;
            }
        }
    }

    //walking functions

    void moveBotToPosition() {
        botAnim.SetBool("sit", false);
        botAnim.SetBool("endWalking", false);
        botAnim.SetBool("startWalking", true);
        bot.transform.rotation = Quaternion.Slerp(bot.transform.rotation, Quaternion.LookRotation(player.transform.position - bot.transform.position), 3 * Time.deltaTime);
        bot.transform.position += transform.forward * Time.deltaTime * 2;
    }

    void botRechedPosition() {
        botAnim.SetBool("endWalking", true);
        botAnim.SetBool("startWalking", false);
    }

    //talking functions

    IEnumerator playAudio(AudioClip clip) {
        audioPlayer.PlayOneShot(clip);

        botAnim.SetBool("talk", true);
        botAnim.SetBool("talk", false);
        double len = clip.length;
        while (len > 10) {
            yield return new WaitForSeconds(10);
            botAnim.SetBool("talk", true);
            botAnim.SetBool("talk", false);
            len -= 10;
        }
    }

    //animation functions

    public void entranceAnimation() {
        if (view == null) {
            //view = PhotonView.Get(this);
        }
        //view.RPC("entranceAnimationRPC", RpcTarget.All);
        aborted = false;
        botRechedPosition();
        StartCoroutine(toPlayer());
        botAnim.SetBool("sit", true);
        entranceAnimationRPC();
        view.RPC("Entrance", RpcTarget.All);
    }

    private bool entranceOrEucharistStarted = false;
    public void entranceAnimationRPC() {
        if (animationTriggered) {
            return;
        }
        if (state == state_possible.ENTRANCEEND) {
            entranceOrEucharistStarted = true;
            animationTriggered = true;
            rotateToPlayer = false;
            priestEucharist.SetBool("restart", false);
            startEucharist();
        } else if (state == state_possible.START) {
            POIConfession.SetActive(false);
            entranceOrEucharistStarted = true;
            state = state_possible.ENTRANCE;
            animationTriggered = true;
            rotateToPlayer = false;
            priestObject.SetActive(true);
            priestfollow1Object.SetActive(true);
            priestfollow2Object.SetActive(true);
            priestAnim.SetBool("restart", false);
            priestfollow2Anim.SetBool("restart", false);
            priestfollow1Anim.SetBool("restart", false);
            priest.m_Speed = 0.6f;
            priestfollow1.m_Speed = 0.6f;
            priestfollow2.m_Speed = 0.6f;
            StartCoroutine(checkWhenFinishedPriest(priest, priestAnim));
            StartCoroutine(checkWhenFinished(priestfollow1, priestfollow1Anim));
            StartCoroutine(checkWhenFinished(priestfollow2, priestfollow2Anim));
            botsUp();
            entranceMusic.Play();
            StartCoroutine(checkWhenMusicFinish(entranceMusic, entranceMusic: true));
        }
    }

    IEnumerator toPlayer() {
        yield return new WaitForSeconds(1);
        int offsetx = 0;
        if (player.transform.position.x > 19) {
            offsetx += 1;
        } else {
            offsetx -= 1;
        }
        bot.transform.position = new Vector3(player.transform.position.x + offsetx, bot.transform.position.y, player.transform.position.z);
        bot.transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    private bool aborted = false;

    public UnityEvent churchDoors;


    public IEnumerator checkWhenMusicFinish(AudioSource audio,bool welcomeMusic = false, bool entranceMusic = false, bool EntranceAudio = false, bool EucharistAudio = false, bool insideConfession = false, bool outsideConfession = false) {
        yield return new WaitForSeconds(0.2f);
        while (audio.isPlaying) {
            if (aborted) {
                break;
            }
            yield return new WaitForSeconds(0.2f);
        }
        if (!aborted) {
            if (welcomeMusic) {
                churchDoors.Invoke();
                welcomeAudioPlaying = false;
            }
            if (entranceMusic) {
                entranceAnimationDisabled();
                botsDown();
                priestAnim.SetBool("pray1", true);
                priestAnim.SetBool("pray2", true);
                priestAnim.SetBool("walkEnd", false);
                yield return new WaitForSeconds(5);
                priestAnim.SetBool("pray1", false);
                priestAnim.SetBool("pray2", false);
                priestAnim.SetBool("idle", true);
                state = state_possible.ENTRANCEEND;
                entranceOrEucharistStarted = false;
                afterEntrance();
            }
            if (EntranceAudio) {
                startEucharist();
            }
            if (EucharistAudio) {
                entranceOrEucharistStarted = false;
                eucharistEnd();
            }
            if (insideConfession) {
                confessionFinished();
                secondConfessionStarted = false;
            }
            if (outsideConfession) {
                outsideFinished = true;
                outsideFinishedAnimation.Invoke();
                state = state_possible.CONFESSIONEND;
                confessionStarted = false;
            }
        } else {
            aborted = false;
        }
    }

    public void eucharistEnd() {
        animationTriggered = false;
        botAnim.SetBool("sit", false);
        state = state_possible.START;
        POIConfession.SetActive(true);
    }

    public UnityEvent outsideFinishedAnimation;
    public UnityEvent closeCurtains;

    public void afterEntrance() {
        priestObject.SetActive(false);
        StartCoroutine(playAudio(afterEntranceAudio));
        StartCoroutine(checkWhenMusicFinish(audioPlayer, EntranceAudio: true));
    }


    public IEnumerator checkWhenFinishedPriest(CinemachineDollyCart akt, Animator anim) {
        float lastPos = akt.m_Position;
        yield return new WaitForSeconds(0.2f);
        while (!(lastPos > 51)) {
            lastPos = akt.m_Position;
            yield return new WaitForSeconds(0.2f);
        }
        anim.SetBool("walkEnd", true);

        //sound spielen + andere animationen
    }

    public IEnumerator checkWhenFinished(CinemachineDollyCart akt, Animator anim) {
        float lastPos = akt.m_Position;
        yield return new WaitForSeconds(0.2f);
        while (!lastPos.Equals(akt.m_Position)){
            lastPos = akt.m_Position;
            yield return new WaitForSeconds(0.2f);
        }
        anim.SetBool("walkEnd", true);
    }

    public void entranceAnimationDisabled() {
        //animationTriggered = false;
        //botAnim.SetBool("sit", false);
    }

    public void confessionAnimation() {
        if(view == null) {
            //view = PhotonView.Get(this);
        }
        //view.RPC("confessionAnimationRPC", RpcTarget.All);
        aborted = false;
        confessionAnimationRPC();
        view.RPC("Confession", RpcTarget.All);
    }

    private bool confessionStarted = false;

    public void confessionAnimationRPC() {
        if (animationTriggered || !state.Equals(state_possible.START) || state.Equals(state_possible.CONFESSIONEND) || confessionStarted) {
            return;
        }
        confessionStarted = true;

        state = state_possible.CONFESSION;
        botAnim.SetBool("sit", false);
        animationTriggered = true;
        StartCoroutine(moveToPosition());
        //playAudio(first);
    }

    IEnumerator moveToPosition() {
        botAnim.SetBool("endWalking", false);
        botAnim.SetBool("startWalking", true);
        botAnim.SetBool("sit", false);
        Vector3 offset = new Vector3(7.22f, bot.transform.position.y, 7.74f) - bot.transform.position;
        //while (!bot.transform.position.x.Equals(7.22f) && !bot.transform.position.z.Equals(7.74f)) {
        while ((offset.x > 0.3f || offset.x < -0.3f) || (offset.z > 0.3f || offset.z < -0.3f)) {
            bot.transform.rotation = Quaternion.LookRotation(new Vector3(7.22f, bot.transform.position.y, 7.74f) - bot.transform.position);
            bot.transform.position += transform.forward * Time.deltaTime * 3;
            offset = new Vector3(7.22f, bot.transform.position.y, 7.74f) - bot.transform.position;
            yield return new WaitForEndOfFrameUnit();
        }
        botAnim.SetBool("endWalking", true);
        botAnim.SetBool("startWalking", false);
        botAnim.SetBool("sit", false);
        rotateToPlayer = true;
        //animationTriggered = true;
        StartCoroutine(playAudio(outsideConfessionAudio));
        StartCoroutine(checkWhenMusicFinish(audioPlayer, outsideConfession: true));
    }

    private bool outsideFinished = false;
    private bool rotatePriestToHostie;
    private Quaternion priestQuat;
    private Quaternion hostieQuat;
    //private Quaternion priestQuat;
    private bool rotatePriestToGoblet;
    private bool hostieFollowHands;
    private bool gobletFollowHandy;
    private bool rotatePriestBack;
    private bool hostieFirstPosition;
    private bool hostieHigh;
    private bool hostieBack;
    private bool gobletFirst;
    private bool gobletHigh;
    private bool gobletBack;

    public void botSitInConfession() {
        if (view == null) {
            //view = PhotonView.Get(this);
        }
        //view.RPC("botSitInConfessionRPC", RpcTarget.All);
        aborted = false;
        botSitInConfessionRPC();
        view.RPC("ConfessionTwo", RpcTarget.All);
    }

    private bool secondConfessionStarted = false;

    public void botSitInConfessionRPC() {
        if (state.Equals(state_possible.CONFESSIONEND) && !secondConfessionStarted) {
            secondConfessionStarted = true;
            state = state_possible.CONFESSIONTWO;
            StartCoroutine(secondPartConfession());
        }
    }

    private Vector3 cameraDownPos;
    private bool checkPositionCamera;

    public void cameraDown() {
        GameObject rh = GameObject.Find("Right Hand");
        rh.GetComponent<TeleportController>().AbortTeleportation();
        //camera.transform.position = new Vector3(camera.transform.position.x, 1, camera.transform.position.z);
        //cameraDownPos = player.transform.position;
    }

    public void cameraBack() {
        camera.transform.position = new Vector3(camera.transform.position.x, 1.7f, camera.transform.position.z);
    }

    IEnumerator secondPartConfession() {
        while (!outsideFinished) {
            yield return new WaitForSeconds(0.5f);
        }
        rotateToPlayer = false;
        outsideFinished = false;
        animationTriggered = true;
        botAnim.SetBool("sit", true);
        bot.transform.position = new Vector3(7.11f, 0.275f, 4.703f);
        bot.transform.rotation = new Quaternion(0, 0, 0, 0);
        closeCurtains.Invoke();
        StartCoroutine(playAudio(insideConfessionAudio));
        StartCoroutine(checkWhenMusicFinish(audioPlayer, insideConfession: true));
    }

    public void confessionFinished() {
        botAnim.SetBool("sit", false);
        animationTriggered = false;

        state = state_possible.START;
    }

    //public void confessionAnimationDisabled() {
    //    animationTriggered = false;
    //}

    public void startEucharist() {

        state = state_possible.EUCHARIST;
        botsUp();
        //priestObject.SetActive(false);
        //priestEuch.SetActive(true);
        priestEuch.SetActive(true);
        hostie.SetActive(true);
        hostiePan.SetActive(true);
        goblet.SetActive(true);
        oil.SetActive(true);
        bible.SetActive(true);
        eucharistAudio.Play();
        StartCoroutine(timerPriestMovement());
        //StartCoroutine(activateAllStuff());
        //Hochgebet
        //Sancuts
        //botsKnee();
        //Hochgebet
        //Geheimnis des Glaubens
        //Lobpreis zum Abschluss des Hochgebetes
        //botsKneeUp();
        //Vater unser
        //Friedensgruß
        //Lamm Gottes
        //Zur Kommunion

    }

    IEnumerator activateAllStuff() {
        priestObject.SetActive(false);
        //yield return new WaitForEndOfFrame();
        priestEuch.SetActive(true);
        //yield return new WaitForEndOfFrame();
        hostie.SetActive(true);
        //yield return new WaitForEndOfFrame();
        hostiePan.SetActive(true);
        //yield return new WaitForEndOfFrame();
        goblet.SetActive(true);
        //yield return new WaitForEndOfFrame();
        oil.SetActive(true);
        //yield return new WaitForEndOfFrame();
        bible.SetActive(true);
        //yield return new WaitForEndOfFrame();
        eucharistAudio.Play();
        yield return new WaitForEndOfFrame();
        StartCoroutine(timerPriestMovement());
    }

    IEnumerator timerPriestMovement() {
        priestEucharist.SetBool("small", true);
        yield return new WaitForSeconds(60);
        priestEucharist.SetBool("small", false);
        priestEucharist.SetBool("wide2", true);
            yield return new WaitForSeconds(58); // 1:58
        priestEucharist.SetBool("wide2", false);
        priestEucharist.SetBool("small", true);
            yield return new WaitForSeconds(44); // 2:42
        priestEucharist.SetBool("cross", true);
            yield return new WaitForSeconds(24); // 3:06
        priestEucharist.SetBool("wide", true);
        yield return new WaitForSeconds(36); // 3:42
        //pick hostie
        priestQuat = priestEuch.transform.rotation;
        hostieQuat = hostie.transform.rotation;
        rotatePriestToHostie = true;
        yield return new WaitForSeconds(1);
        priestEucharist.SetBool("hostie", true);
        yield return new WaitForSeconds(1);
        hostieFirstPosition = true;
        yield return new WaitForSeconds(10); // 3:54
        //hostie high
        //bot.transform.rotation = Quaternion.Slerp(bot.transform.rotation, Quaternion.LookRotation(player.transform.position - bot.transform.position), 3 * Time.deltaTime);
        priestEucharist.SetBool("hostie2", true);
        hostieFirstPosition = false;
        hostieHigh = true;
        yield return new WaitForSeconds(2); // 3:56
        //pick wine + hostie back
        hostieQuat = hostie.transform.rotation;
        priestQuat = priestEuch.transform.rotation;
        priestEucharist.SetBool("wine", true);
        rotatePriestToHostie = false;
        rotatePriestToGoblet = true;
        hostieHigh = false;
        hostieBack = true;
        yield return new WaitForSeconds(1);
        gobletFirst = true;
        yield return new WaitForSeconds(9); // 4:06
        //wine high
        //bot.transform.rotation = Quaternion.Slerp(bot.transform.rotation, Quaternion.LookRotation(player.transform.position - bot.transform.position), 3 * Time.deltaTime);
        priestEucharist.SetBool("wine2", true);
        gobletFirst = false;
        gobletHigh = true;
        yield return new WaitForSeconds(2); // 4:07
        //wine back
        //bot.transform.rotation = Quaternion.Slerp(bot.transform.rotation, Quaternion.LookRotation(player.transform.position - bot.transform.position), 3 * Time.deltaTime);
        priestEucharist.SetBool("wine3", true);
        priestQuat = priestEuch.transform.rotation;
        rotatePriestToGoblet = false;
        rotatePriestBack = true;
        gobletHigh = false;
        gobletBack = true;
        yield return new WaitForSeconds(9); // 4:17
        priestEucharist.SetBool("above", true);
        rotatePriestBack = false;
        gobletBack = false;
        yield return new WaitForSeconds(35); // 4:52
        priestEucharist.SetBool("small2", true);
            yield return new WaitForSeconds(61); // 5:53
        afterEucharist();

    }

    public void botsUp() {
        var bots = GameObject.FindGameObjectsWithTag("churchBot");
        foreach (GameObject g in bots) {
            Animator anim = g.GetComponent<Animator>();
            anim.SetBool("sit", false);
            anim.SetBool("stand", true);
            //StartCoroutine(standUpForward(g));
        }
    }

    //IEnumerator standUpForward(GameObject g) {
    //    yield return new WaitForSeconds(3);
    //    g.transform.position += new Vector3(0, 0, 0.6f);
    //    //g.transform.position = new Vector3(g.transform.position.x, g.transform.position.y, g.transform.position.z + 0.6f);
    //}

    public void botsDown() {
        var bots = GameObject.FindGameObjectsWithTag("churchBot");
        foreach (GameObject g in bots) {
            Animator anim = g.GetComponent<Animator>();
            anim.SetBool("stand", false);
            anim.SetBool("sit", true);
            //StartCoroutine(sitDownForward(g));
        }
    }

    //IEnumerator sitDownForward(GameObject g) {
    //    yield return new WaitForSeconds(3);
    //    g.transform.position += new Vector3(0, 0, -0.6f);
    //    //g.transform.position = new Vector3(g.transform.position.x, g.transform.position.y, g.transform.position.z + 0.6f);
    //}

    public void botsKnee() {
        var bots = GameObject.FindGameObjectsWithTag("churchBot");
        foreach (GameObject g in bots) {
            Animator anim = g.GetComponent<Animator>();
            anim.SetBool("stand", false);
            anim.SetBool("knee", true);
            //StartCoroutine(kneeForward(g));
        }
    }

    //IEnumerator kneeForward(GameObject g) {
    //    yield return new WaitForSeconds(3);
    //    g.transform.position += new Vector3(0, -0.09f, 0.26f);
    //    //g.transform.position = new Vector3(g.transform.position.x, g.transform.position.y, g.transform.position.z + 0.6f);
    //}

    public void botsKneeUp() {
        var bots = GameObject.FindGameObjectsWithTag("churchBot");
        foreach (GameObject g in bots) {
            Animator anim = g.GetComponent<Animator>();
            anim.SetBool("knee", false);
            anim.SetBool("stand", true);
            //StartCoroutine(kneeBackward(g));
        }
    }

    //IEnumerator kneeBackward(GameObject g) {
    //    yield return new WaitForSeconds(3);
    //    g.transform.position += new Vector3(0, 0.09f, -0.26f);
    //    //g.transform.position = new Vector3(g.transform.position.x, g.transform.position.y, g.transform.position.z + 0.6f);
    //}

    public void afterEucharist() {
        StartCoroutine(playAudio(afterEucharistAudio));
        StartCoroutine(checkWhenMusicFinish(audioPlayer, EucharistAudio: true));
        botsDown();
    }
}
