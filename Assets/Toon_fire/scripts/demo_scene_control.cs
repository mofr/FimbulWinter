using UnityEngine;
using System.Collections;

public class demo_scene_control : MonoBehaviour {
	public GameObject fire;
	public GameObject fire_w_smoke;
	public GameObject small_fire;
	public GameObject small_fire_w_smoke;
	public GameObject firethrower;
	public GameObject firethrower_w_smoke;
	public GameObject w_fire;
	public GameObject w_fire_smoke;
	public GameObject floor_fire;
	
	private Transform fire_spawn;
	private Transform firethrower_spawn;
	private Transform small_fire_spawn;
	private GameObject active;
	private float x_angle=0f;
	// Use this for initialization
	void Start () {
		x_angle= this.transform.eulerAngles.x;
	fire_spawn = GameObject.Find("fire_spawn").transform;
		firethrower_spawn = GameObject.Find("firethrower_spawn").transform;
	small_fire_spawn = GameObject.Find("small_fire_spawn").transform;
	
	}
	
	// Update is called once per frame
	void Update () {
		if (active!=null){
	this.transform.RotateAround(fire_spawn.position,Vector3.up,.3f);}
	this.transform.LookAt(fire_spawn.position);
		this.transform.eulerAngles = new Vector3(x_angle,this.transform.eulerAngles.y,this.transform.eulerAngles.z);
	}
	
	void OnGUI(){
		if (GUI.Button(new Rect(20,20,200,20),"Basic Fire")){
		GameObject g_fire = Instantiate(fire,fire_spawn.position,fire_spawn.rotation) as GameObject;
			Destroy(active);
			active= g_fire;
		}
		if (GUI.Button(new Rect(20,50,200,20),"Basic Fire With Smoke")){
		GameObject g_fire_w = Instantiate(fire_w_smoke,fire_spawn.position,fire_spawn.rotation) as GameObject;
		Destroy(active);
			active=g_fire_w;
		}
		if (GUI.Button(new Rect(20,80,200,20),"FireThrower")){
		GameObject g_firethrower = Instantiate(firethrower,firethrower_spawn.position,firethrower_spawn.rotation) as GameObject;
		Destroy(active);
			active=g_firethrower;
		}
		if (GUI.Button(new Rect(20,110,200,20),"FireThrower with smoke")){
		GameObject g_firethrower_w = Instantiate(firethrower_w_smoke,firethrower_spawn.position,firethrower_spawn.rotation) as GameObject;
		Destroy(active);
			active=g_firethrower_w;
		}
		
		if (GUI.Button(new Rect(20,140,200,20),"Wall of Fire")){
		GameObject g_w_fire = Instantiate(w_fire,fire_spawn.position,fire_spawn.rotation) as GameObject;
		Destroy(active);
			active=g_w_fire;
		}
		
			if (GUI.Button(new Rect(20,170,200,20),"Wall of Fire with smoke")){
		GameObject g_w_fire_smoke = Instantiate(w_fire_smoke,fire_spawn.position,fire_spawn.rotation) as GameObject;
		Destroy(active);
			active=g_w_fire_smoke;
		}
		
			if (GUI.Button(new Rect(20,200,200,20),"Floor of Fire")){
		GameObject g_floor_fire = Instantiate(floor_fire,fire_spawn.position,fire_spawn.rotation) as GameObject;
		Destroy(active);
			active=g_floor_fire;
		}
		
		if (GUI.Button(new Rect(20,230,200,20),"Small (Torch) Fire")){
		GameObject g_small_fire = Instantiate(small_fire,small_fire_spawn.position,small_fire_spawn.rotation) as GameObject;
		Destroy(active);
			active=g_small_fire;
		}
		if (GUI.Button(new Rect(20,260,200,20),"Small Fire with Smoke")){
		GameObject g_small_fire_w = Instantiate(small_fire_w_smoke,small_fire_spawn.position,small_fire_spawn.rotation) as GameObject;
		Destroy(active);
			active=g_small_fire_w;
		}
		
		
		
	}
}
