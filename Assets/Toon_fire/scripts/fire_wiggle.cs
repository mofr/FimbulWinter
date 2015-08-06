using UnityEngine;
using System.Collections;

public class fire_wiggle : MonoBehaviour {
	private float t=0f;
	private float wiggle_t=0f;
	private float initial_start_speed;
	private float initial_emission_rate;
	private float initial_lifetime;
	private float initial_size;
	// Use this for initialization
	
	void Start () {
	initial_start_speed=this.GetComponent<ParticleSystem>().startSpeed;
    initial_emission_rate=this.GetComponent<ParticleSystem>().emissionRate;
	initial_lifetime = this.GetComponent<ParticleSystem>().startLifetime;
	initial_size = this.GetComponent<ParticleSystem>().startSize;
	}
	
	// Update is called once per frame
	void Update () {
		t+=Time.deltaTime;
		wiggle_t+=Time.deltaTime;
		
		
		//creatin bursts of fire to make it more physically  realistic-->
		if (t>(2f+(2f-Mathf.Sin(wiggle_t)))){
			
		
		
		this.GetComponent<ParticleSystem>().emissionRate+=(initial_emission_rate*.4f-this.GetComponent<ParticleSystem>().emissionRate)/30f;
			this.GetComponent<ParticleSystem>().startLifetime+=(initial_lifetime*.9f-this.GetComponent<ParticleSystem>().startLifetime)/30f;
			
			if (this.GetComponent<ParticleSystem>().emissionRate<initial_emission_rate*.42f){
				this.GetComponent<ParticleSystem>().emissionRate = initial_emission_rate*1.1f;
					this.GetComponent<ParticleSystem>().startLifetime=initial_lifetime*1.1f;
				this.GetComponent<ParticleSystem>().startSpeed=initial_start_speed*.7f;
					this.GetComponent<ParticleSystem>().startSize= initial_size*1.1f;
				t=0f;
			}
		} else{
		this.GetComponent<ParticleSystem>().emissionRate+=(initial_emission_rate-this.GetComponent<ParticleSystem>().emissionRate)/30f;
			this.GetComponent<ParticleSystem>().startLifetime+=(initial_lifetime-this.GetComponent<ParticleSystem>().startLifetime)/30f;
			this.GetComponent<ParticleSystem>().startSpeed+=(initial_start_speed-this.GetComponent<ParticleSystem>().startSpeed)/30f;
			this.GetComponent<ParticleSystem>().startSize+=(initial_size-this.GetComponent<ParticleSystem>().startSize)/30f;
				
			
		}
	}
}
