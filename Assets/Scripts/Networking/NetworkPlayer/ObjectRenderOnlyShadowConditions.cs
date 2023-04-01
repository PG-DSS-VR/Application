using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Objects can defined, whether an object should be rendered, if it is (not) the local player
/// </summary>
public class ObjectRenderOnlyShadowConditions : MonoBehaviour {
    [Header("If the following")]
    [SerializeField]
    private PhotonView photonView;

    [Header("-instance is a")]
    [SerializeField]
    private NetworkStatus networkStatus;

    [Header("then")][SerializeField] private RenderStatus renderStatus;

    [Header("the following objects")]
    [SerializeField]
    private List<Renderer> renderers;



    public enum NetworkStatus {
        LocalPlayer,
        NetworkPlayer
    }

    public enum RenderStatus {
        ObjectWithShadows,
        OnlyShadows
    }

    // Start is called before the first frame update
    void Start() {
        //photon view instance has the wanted property
        if (photonView.IsMine == networkStatus is NetworkStatus.LocalPlayer) {
            foreach (var ren in renderers) {
                if (renderStatus is RenderStatus.ObjectWithShadows) {
                    ren.shadowCastingMode = ShadowCastingMode.On;
                } else if (renderStatus is RenderStatus.OnlyShadows) {
                    ren.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
                }

            }
        }

    }
}