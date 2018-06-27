using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";

    public PlayerWeapon weapon;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

	void Start () {
		if (cam == null)
        {
            Debug.Log("PlayerShoot: No camera referenced!");
            this.enabled = false;
        }
	}
	
	void Update () {
		if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
	}

    [Client]
    void Shoot ()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weapon.range, mask))
        {
            Debug.Log(hit.collider.name);
            if (hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(hit.collider.name, weapon.damege);
            }
        }
    }

    [Command]
    void CmdPlayerShot (string playerID, int damage)
    {
        Debug.Log(playerID + "has been shot.");

        Player player = GameManager.GetPlayer(playerID);
        player.RpcTakeDamage(damage);
    }
}
