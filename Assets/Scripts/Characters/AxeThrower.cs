using UnityEngine;
using System.Collections;

public class AxeThrower : MonoBehaviour
{
	public GameObject projectilePrefab;
	public float projectileSpeed;
	public float damage;

	public bool attacking {
		set {
			if(value == _attacking) 
				return;

			_attacking = value;
			if(_attacking) {
				StartCoroutine ("Attacking");
			} else {
				StopCoroutine ("Attacking");
			}
		}
	}

	Character character;
	CharacterMovement movement;
	Animator anim;
	GameObject player;
	Character playerCharacter;
	bool _attacking = false;
	
	void Start ()
	{
		character = GetComponent<Character>();
		movement = GetComponent<CharacterMovement>();
		anim = GetComponent<Animator>();
		player = GameObject.FindWithTag("Player");
		playerCharacter = player.GetComponent<Character>();
	}

	IEnumerator Attacking()
	{
		yield return new WaitForSeconds (Random.Range (1f, 2f));
		while (!playerCharacter.dead && !character.dead) {
			movement.LookAt (player.transform.position);
			anim.SetTrigger("Attack");
			yield return new WaitForSeconds (Random.Range (2f, 5f));
		}
	}

	void OnAttack() {
		GameObject projectile = Instantiate (projectilePrefab, transform.position, new Quaternion()) as GameObject;
		Destroy (projectile, 10);
		
		Collider2D targetCollider = player.GetComponent<Collider2D> ();
		
		Vector3 diff = targetCollider.bounds.center - transform.position;
		diff.Normalize();
		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		projectile.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
		
		projectile.GetComponent<Projectile> ().velocity = projectileSpeed * (player.transform.position - projectile.transform.position).normalized;
		projectile.GetComponent<Projectile> ().originator = character;
		projectile.GetComponent<Projectile> ().damage = damage;
	}
}

