using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInfo : Photon.Pun.MonoBehaviourPun, IPunObservable
{
    public int colorIndex;

    public SpriteRenderer playerBody;

    public List<Color> allPlayerColors = new List<Color>();
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(colorIndex);
        }
        else
        {
            colorIndex = (int)stream.ReceiveNext();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            colorIndex = Random.Range(0, allPlayerColors.Count - 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerBody.color = allPlayerColors[colorIndex];
    }
}
