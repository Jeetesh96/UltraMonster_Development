using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class PlayerMovement : MonoBehaviourPunCallbacks //Photon.MonoBehaviour
{

    public PhotonView PhotonView;
    private Vector3 TargetPosition;
    private Quaternion TargetRotation;


    public void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
    }
    // Update is called once per frame
    public void Update()
    {
        if (PhotonView.IsMine)
            CheckInput();
        else
            SmoothMove();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting == true)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            TargetPosition = (Vector3)stream.ReceiveNext();
            TargetRotation = (Quaternion)stream.ReceiveNext();
        }
    }

    public void SmoothMove()
    {
        transform.position = Vector3.Lerp(transform.position, TargetPosition, 0.25f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetRotation, 500 * Time.deltaTime);
    }

    public void CheckInput()
    {
        float movespeed = 60f;
        float rotatespeed = 150f;

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        transform.position += transform.forward * (vertical * movespeed * Time.deltaTime);
        transform.Rotate(new Vector3(0, horizontal * rotatespeed * Time.deltaTime, 0));
    }
}
