using UnityEngine;
using System.Collections;

public class WeatherController : MonoBehaviour {

	public GameObject windZone;
	public GameObject rainParticles;
	public GameObject snowParticles;

	private bool rainStop;
	private bool snowStop;

	public void Rain(){
		ParticleSystem rainSys = rainParticles.GetComponent<ParticleSystem> ();
		if (rainStop) {
			var vel = rainSys.velocityOverLifetime;

			AnimationCurve curve = new AnimationCurve();
			curve.AddKey( 0.0f, 1.0f );
			curve.AddKey( 1.0f, 1.0f );
			vel.x = new ParticleSystem.MinMaxCurve(Random.Range (-15.0f, 15.0f), curve);
			rainSys.Play ();
			rainStop = false;
		}
	}

	public void Fair(){
		rainParticles.GetComponent<ParticleSystem> ().Stop();
		rainStop = true;

		snowParticles.GetComponent<ParticleSystem> ().Stop();
		snowStop = true;
	}

	public void Sunshine(){
		Fair ();
	}

	public void Frost(){
		ParticleSystem snowSys = snowParticles.GetComponent<ParticleSystem> ();
		if (snowStop) {
			var vel = snowSys.velocityOverLifetime;

			AnimationCurve curve = new AnimationCurve ();
			curve.AddKey (0.0f, 1.0f);
			curve.AddKey (1.0f, 1.0f);
			vel.x = new ParticleSystem.MinMaxCurve (Random.Range (-8.0f, 8.0f), curve);

			snowSys.Play ();
			snowStop = false;
		}
	}
}
