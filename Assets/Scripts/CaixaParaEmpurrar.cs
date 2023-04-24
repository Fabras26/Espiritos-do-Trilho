using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
public class CaixaParaEmpurrar : MonoBehaviour
{
    public bool movendo;

	public float velocidade;
	Rigidbody rb;
	PlayerController pl;
	FixedJoint fx;

	public AudioClip somArrastar;

	private void Start()
    {
		rb = GetComponent<Rigidbody>();
		fx = GetComponent<FixedJoint>();

	}
	public void Move()
	{
		pl = FindObjectOfType<PlayerController>();

		fx.connectedBody = pl.GetComponent<Rigidbody>();
		rb.isKinematic = false;


		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;
		

		inputDirection = Quaternion.Euler(0f, pl.transform.eulerAngles.y, 0f) * inputDirection;

		if (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f)
		{
			Vector3 targetVelocity = inputDirection * velocidade;
			targetVelocity.y = rb.velocity.y;
			rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, Time.deltaTime * 10f);
			if(!GameObject.Find("Jogador").GetComponent<AudioSource>().isPlaying)
			GameObject.Find("Jogador").GetComponent<AudioSource>().PlayOneShot(somArrastar);
		}
		else
		{
			rb.velocity = new Vector3(0, rb.velocity.y, 0);
		}
	

	}
	public void Soltar()
    {
		rb.isKinematic = true;
		fx.connectedBody = null;

	}
}
