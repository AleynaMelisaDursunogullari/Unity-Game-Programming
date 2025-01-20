using UnityEngine;

public class checkpoint : MonoBehaviour
{
   HealthController gameController;

   private void Awake()
   {
        gameController = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthController>();
   }

   private void OnTriggerEnter2D(Collider2D col)
   {
        if(col.CompareTag("Player"))
        {
            gameController.UpdateCheckpoint(transform); // Respawn noktasını geçerli Transform ile güncelle
        }
   }
 

   
}
