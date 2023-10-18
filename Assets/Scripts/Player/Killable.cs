using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killable : Photon.Pun.MonoBehaviourPun
{
    [SerializeField] private float _range = 10f;

    private LineRenderer _lineRenderer;

    private Killable _target;

    private void Awake()
    {
        if (!photonView.IsMine) 
        {
            return;
        }
        _lineRenderer = GetComponent<LineRenderer>();

        StartCoroutine(SearchForKillable());
    }

    private void Start()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        UIControl.Instance.curPlayer = this;
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (_target != null)
        {
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, _target.transform.position);
        }
        else
        {
            _lineRenderer.SetPosition(0, Vector3.zero);
            _lineRenderer.SetPosition(1, Vector3.zero);
        }
    }

    private IEnumerator SearchForKillable()
    {
        while (true)
        {
            Killable newTarget = null;
            Killable[] killList = FindObjectsOfType<Killable>();

            foreach (Killable kill in killList)
            {
                if (kill == this) { continue; }

                float distance = Vector3.Distance(transform.position, kill.transform.position);
                
                if (distance > _range) { continue; }

                // kill target found

                newTarget = kill;
                UIControl.Instance.hasTarget = _target != null;

                break;
            }

            _target = newTarget;

            yield return new WaitForSeconds(0.25f);
        }
    }

    public void Kill()
    {
        if (_target == null) { return; }
        PhotonView pv = _target.GetComponent<PhotonView>();
        pv.RPC("KillRPC", RpcTarget.All);
    }

    [PunRPC]
    public void KillRPC()
    {
        if (!photonView.IsMine) { return; }
        
        PlayerDeadBody playerBody = PhotonNetwork.Instantiate(
            "PlayerBody",
            transform.position,
            Quaternion.identity).GetComponent<PlayerDeadBody>();

        PlayerInfo playerInfo = GetComponent<PlayerInfo>();

        playerBody.SetColor(playerInfo.allPlayerColors[playerInfo.colorIndex]);
        transform.position = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0);
    }
}
