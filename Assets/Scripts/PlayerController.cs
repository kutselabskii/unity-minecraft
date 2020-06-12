using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	public float Speed = 12f;
    public float JumpSpeed = 7f;
    public float Gravity = 48f;

    public float RotationSpeed = 15f;

	public GameObject Block1;
	public GameObject Block2;
	public GameObject Block3;

	public Texture2D Block1Texture;
	public Texture2D Block2Texture;
	public Texture2D Block3Texture;

	public int MaxHealth = 3;
	private int health;

    private CharacterController controller;

	private float lastY = 0f;

	private GameObject currentBlock;

	private RawImage[] textures;

	void Start()
	{
		health = MaxHealth;
		UpdateHealthbar();

        controller = GetComponent<CharacterController>();
		Cursor.lockState = CursorLockMode.Locked;
		currentBlock = Block1;

		textures = transform.GetComponentsInChildren<RawImage>().Skip(1).ToArray();
		textures[0].texture = Block1Texture;
		textures[1].texture = Block2Texture;
		textures[2].texture = Block3Texture;
		SetChosen(0);
	}

    void Update()
    {
		HandleKeyboard();
        HandleCursorLocking();
        HandleMouseControls();
        HandleMovement();
	}

	private void UpdateHealthbar()
	{
		transform.Find("Camera/Canvas/HealthBar").GetComponent<Text>().text = $"Health: {health}/{MaxHealth}";
		if (health <= 0) {
			Dead();
		}
	}

	public void LoseLife()
	{
		health--;
		UpdateHealthbar();
	}

	private void SetChosen(int index)
	{
		transform.Find("Camera/Canvas/ChosenMark").transform.position = textures[index].transform.position;
	}

	private void HandleKeyboard()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			currentBlock = Block1;
			SetChosen(0);
		}

		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			currentBlock = Block2;
			SetChosen(1);
		}

		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			currentBlock = Block3;
			SetChosen(2);
		}

		if (Input.GetKey(KeyCode.Q)) {
			Application.Quit();
		}

		if (Input.GetKeyDown(KeyCode.R)) {
			Dead();
		}
	}

	private void HandleCursorLocking()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) {
			switch (Cursor.lockState) {
                case CursorLockMode.None:
					Cursor.lockState = CursorLockMode.Locked;
					break;
                case CursorLockMode.Locked:
					Cursor.lockState = CursorLockMode.None;
					break;
            }
		}
	}

    private void HandleMouseControls()
	{
		transform.Rotate(new Vector3(0f, Input.GetAxis("Mouse X"), 0f) * RotationSpeed);

		if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) {
			var ray = FindObjectOfType<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
			if (Physics.Raycast(ray, out var hit, 4f)) {
				var script = hit.transform.gameObject.GetComponent<CubeController>();
				if (script != null) {
					if (Input.GetMouseButtonDown(0)) {
						Instantiate(currentBlock, script.GetCreationPosition(hit), Quaternion.identity);
					} else {
						Destroy(hit.transform.gameObject);
					}
				}
			}
		}
	}

    private void HandleMovement()
	{
        var direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        direction *= Speed;

		direction.y = lastY;

		if (controller.isGrounded) {
			if (Input.GetButton("Jump")) {
				direction.y = JumpSpeed;
			}
        }

		direction.y -= Gravity * Time.deltaTime;
		lastY = direction.y;

		direction = transform.TransformDirection(direction);
        controller.Move(direction * Time.deltaTime);
    }

	private void Dead()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
