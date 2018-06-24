using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRabbit : MonoBehaviour {
	public float speed = 1;
	public Rigidbody2D myBody = null;
	bool isGrounded = false;
	bool JumpActive = false;
	float JumpTime = 0f;
	public float MaxJumpTime = 2f;
	public float JumpSpeed = 2f;
	Transform heroParent = null;


	// Use this for initialization
	void Start () {
		myBody = this.GetComponent<Rigidbody2D> ();
		LevelController.current.setStartPosition (transform.position);
		heroParent = this.transform.parent;

	}

	// Update is called once per frame
	void FixedUpdate () {
		//[-1, 1]
		float value = Input.GetAxis ("Horizontal");
		if (Mathf.Abs (value) > 0) {
			Vector2 vel = myBody.velocity;
			vel.x = value * speed;
			myBody.velocity = vel;
		}
		SpriteRenderer sr = GetComponent<SpriteRenderer> ();
		if (value < 0) {
			sr.flipX = true;
		} else if (value > 0) {
			sr.flipX = false;
		}

		Animator animator = GetComponent<Animator> ();
		if (Mathf.Abs (value) > 0) {
			animator.SetBool ("run", true);
		} else {
			animator.SetBool ("run", false);
		}
		if(Input.GetButtonDown("Jump") && isGrounded) {
			this.JumpActive = true;
		}

		if(this.JumpActive) {
			//Якщо кнопку ще тримають
			if(Input.GetButton("Jump")) {
				this.JumpTime += Time.deltaTime;
				if (this.JumpTime < this.MaxJumpTime) {
					Vector2 vel = myBody.velocity;
					vel.y = JumpSpeed * (1.0f - JumpTime / MaxJumpTime);
					myBody.velocity = vel;
				}
			} else {
				this.JumpActive = false;
				this.JumpTime = 0;
			}

		}
		if (this.isGrounded) {
			animator.SetBool ("jump", false);
		} else {
			animator.SetBool ("jump", true);
		}

		Vector3 from = transform.position + Vector3.up * 0.3f;
		Vector3 to = transform.position + Vector3.down * 0.1f;
		int layer_id = 1 << LayerMask.NameToLayer ("Ground");
//Перевіряємо чи проходить лінія через Collider з шаром Ground
		RaycastHit2D hit = Physics2D.Linecast (from, to, layer_id);
		if (hit) {
			isGrounded = true;
			if (hit.transform != null
			   && hit.transform.GetComponent<MovingPlatform> () != null) {
				//Приліпаємо до платформи
				SetNewParent (this.transform, hit.transform);
			}
		} else {
			isGrounded = false;
			SetNewParent (this.transform, this.heroParent);

		}
	}
	static void SetNewParent(Transform obj, Transform new_parent) {
		if(obj.transform.parent != new_parent) {
			//Засікаємо позицію у Глобальних координатах
			Vector3 pos = obj.transform.position;
			//Встановлюємо нового батька
			obj.transform.parent = new_parent;
			//Після зміни батька координати кролика зміняться
			//Оскільки вони тепер відносно іншого об’єкта
			//повертаємо кролика в ті самі глобальні координати
			obj.transform.position = pos;
		}
	}
	//Намалювати лінію (для розробника)
	//Debug.DrawLine (from, to, Color.red);
   
	// Update is called once per frame
	void Update () {
		
	}
	public float dieAnimationTime = 1;
	float timeLeftToDie;
	bool isDying = false;

	public void Die() {
		Animator animator = GetComponent<Animator> ();
		if (isDying) {
			return;
		}
			
		if (this.isGrounded) {
			isDying = true;
			animator.SetTrigger ("die");
			animator.Play("Die");
			timeLeftToDie = dieAnimationTime;
		} else {
		//	LevelController.current.onRabbitDeath (this.gameObject);
		//LevelController.current.onRabbitDeath ((HeroRabbit)this.gameObject);
		}
	}
	
	bool isBig;
	public bool IsBig
	{
		get 
		{ 
			return isBig; 
		}
		set 
		{
			if (value) {
				this.transform.localScale = new Vector3 (1.5f, 1.5f, 1.5f);
			} else {
				this.transform.localScale = new Vector3 (1, 1, 1);
			}
			isBig = value;
		}
	}
}
