using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
	public float restartDelay = 5f;

    Animator anim;
	float restartTimer;

    void Awake()
    {
        anim = GetComponent<Animator>();
		restartTimer = 0f;
    }


    void Update()
    {
        if (playerHealth.currentHealth <= 0)
        {
			restartTimer += Time.deltaTime;
            anim.SetTrigger("GameOver");

			if (restartTimer >= restartDelay) {
				Application.LoadLevel (Application.loadedLevel);
			}
        }
    }
}
