using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MonsterHurt : MonoBehaviour
{

	public Collider victimCollider;
	public Transform particlesPool;


	private GameObject[] hitEffects;

	private int hitIndex;

	private CrosshairTarget crosshair;
	public bool canHit;
	public bool hasHit;

	public float hitFrames;
	private float hitTime;

	public float knockbackForce;
	public float knockbackFrames;
	private float knockbackCounter;

	public Rigidbody myRB;

	public int hitCounter;
	public int deathHits;

	public bool knocked;

	public GameObject playerTarget;
	public Transform target;

	public Vector3 directionToTarget;



	void Start()
	{
		Transform target = playerTarget.transform;


		crosshair = FindObjectOfType<CrosshairTarget>();

		canHit = false;
		hasHit = false;

		hitIndex = 0;

		hitEffects = new GameObject[particlesPool.childCount];

		for (int i = 0; i < particlesPool.childCount; i++)
		{
			hitEffects[i] = particlesPool.GetChild(i).gameObject;
		}

		myRB = GetComponent<Rigidbody>();

		hitTime = hitFrames;

	}

	void Update()
	{
		directionToTarget = (target.position - transform.position).normalized;

		canHit = crosshair.isHit;

		
		if (canHit)
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (!EventSystem.current.IsPointerOverGameObject())
				{
					RaycastHit hit = new RaycastHit();
					if (victimCollider.Raycast(Camera.main.ScreenPointToRay(crosshair.transform.position), out hit, 1000f))
					{
						GameObject newHits = SpawnHit();
						newHits.transform.position = hit.point + newHits.transform.position;
						Knockback();
						hasHit = true;
						hitCounter++;
					}
				}
			}
		}		
		
		

		if (hitCounter == deathHits)
        {
			gameObject.SetActive(false);
		}
	}

	public void Knockback()
	{
		
			if (Vector3.Dot(myRB.velocity.normalized, directionToTarget) > 0)
			{
				//velocity is heading towards the target
				myRB.velocity = new Vector3(-knockbackForce, knockbackForce / 3, -knockbackForce);
			}
			else
			{
				//velocity is not heading towards the target
				myRB.velocity = new Vector3(knockbackForce, knockbackForce / 3, knockbackForce);
			}

		

	}


	public void NextHit()
	{
		hitIndex++;
		if (hitIndex >= hitEffects.Length)
		{
			hitIndex = 0;
		}
	}

	public void PreviousHit()
	{
		hitIndex--;
		if (hitIndex < 0)
		{
			hitIndex = hitEffects.Length - 1;
		}
	}


	private GameObject SpawnHit()
	{
		GameObject spawnedHit = Instantiate(hitEffects[hitIndex]);
		spawnedHit.transform.LookAt(Camera.main.transform);
		return spawnedHit;
	}

}
