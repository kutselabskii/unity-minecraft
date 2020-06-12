using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreeperController : MonoBehaviour
{
	private GameObject player;

	public float Speed = 88f;
	public float JumpSpeed = 6f;
	public float Gravity = 24f;
	public float BlastRange = 3f;

	private CharacterController controller;
	private float lastY = 0f;
	private bool needToJump;
	private bool dying;

    void Start()
	{
		player = GameObject.Find("Player(Clone)");

		controller = GetComponent<CharacterController>();
		StartCoroutine(Jump());
	}

    void Update()
	{
		HandleMovement();
		CheckDeath();
	}

	private void CheckDeath()
	{
		if ((player.transform.position - transform.position).magnitude < 4) {
			dying = true;
			StartCoroutine(Dead());
		}
	}

	private IEnumerator Dead()
	{
		transform.localScale = new Vector3(2, 2, 2);
		yield return new WaitForSeconds(2f);
		transform.Find("Bomb").localScale = new Vector3(BlastRange * 4, BlastRange * 4, BlastRange * 4);
		transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
		yield return new WaitForSeconds(1f);
		Destroy(gameObject);
	}

	private void HandleMovement()
	{
		if (dying) {
			return;
		}

		var direction = (player.transform.position - transform.position).normalized;

		direction *= Speed;

		direction.y = lastY;

		if (controller.isGrounded) {
			if (needToJump) {
				direction.y = JumpSpeed;
				needToJump = false;
			}
		}

		direction.y -= Gravity * Time.deltaTime;
		lastY = direction.y;

		direction = transform.TransformDirection(direction);
		controller.Move(direction * Time.deltaTime / 5f);
	}

	private IEnumerator Jump()
	{
		while (true) {
			yield return new WaitForSeconds(Random.Range(4, 7));
			if (dying) {
				yield break;
			}
			needToJump = true;
		}
	}
}
