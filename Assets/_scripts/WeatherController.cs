using UnityEngine;
using System.Collections;

public class WeatherController : MonoBehaviour {

    public GameController game;
	public GameObject rainParticles;
	public GameObject snowParticles;
	public GameObject windParticles;

	private bool rainStop;
	private bool snowStop;
	private bool windStop;

	public void Rain(){

		game.water += 4;

		ParticleSystem rainSys = rainParticles.GetComponent<ParticleSystem> ();
		if (rainStop) {
			// need to stop any other weather systems
			Fair ();

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
		rainParticles.GetComponent<ParticleSystem> ().Stop ();
		rainStop = true;

		snowParticles.GetComponent<ParticleSystem> ().Stop ();
		snowStop = true;

		windParticles.GetComponent<ParticleSystem> ().Stop ();
		windStop = true;
	}

	public void Sunshine(){
		if (game.water > 0) {
			for (int i = 0; i < 2; i++) {
				if (game.water > 0) {
					game.water--;
					game.energy += 2;
				} 
			}
		}
		else
			game.energy--;

		// no weather system change enabled at the moment
	}

	public void Frost(){
		game.energy -= 1;

		ParticleSystem snowSys = snowParticles.GetComponent<ParticleSystem> ();
		if (snowStop) {
			// stop any other weather systems
			Fair ();

			var vel = snowSys.velocityOverLifetime;

			AnimationCurve curve = new AnimationCurve ();
			curve.AddKey (0.0f, 1.0f);
			curve.AddKey (1.0f, 1.0f);
			vel.x = new ParticleSystem.MinMaxCurve (Random.Range (-8.0f, 8.0f), curve);

			snowSys.Play ();
			snowStop = false;
		}
	}

	public void Wind(){

		ParticleSystem windSys = windParticles.GetComponent<ParticleSystem> ();
		if (windStop) {
			// need to stop any other weather systems
			windSys.Play ();
			windStop = false;
		}
	}
}
