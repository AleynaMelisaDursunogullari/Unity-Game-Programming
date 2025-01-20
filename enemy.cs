using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    [SerializeField] private GameObject player; // Oyuncu
    [SerializeField] private float speed; // Canavarın hızı
    [SerializeField] private float destroyDelay = 2f; // Yok olma gecikmesi
    [SerializeField] private GameObject head; // Kafaya özel collider
    [SerializeField] private Transform patrolPoint1; // Devriye noktası 1
    [SerializeField] private Transform patrolPoint2; // Devriye noktası 2
    [SerializeField] private float patrolWaitTime = 1f; // Devriye noktasında bekleme süresi
    [SerializeField] private float detectionRadius = 5f; // Algılama alanı
    [SerializeField] private Transform groundCheck; // Zemin kontrol noktası
    [SerializeField] private LayerMask groundLayer; // Zemin katmanı

    private Animator animator; // Animator referansı
    private bool isDead = false; // Canavarın ölüp ölmediğini kontrol etmek için
    private bool isChasing = false; // Oyuncuyu takip ediyor mu?
    private Transform currentPatrolTarget; // Şu anda gidilen devriye noktası
    private float waitTimer = 0f;

    private void Start()
    {
        animator = GetComponent<Animator>(); // Animator bileşenini al
        currentPatrolTarget = patrolPoint1; // İlk devriye hedefi
    }

    private void Update()
    {
        if (isDead) return; // Canavar ölüyorsa hiçbir şey yapma

        if (isChasing)
        {
            ChasePlayer(); // Oyuncuyu takip et
        }
        else
        {
            Patrol(); // Devriye hareketi yap
        }

        CheckPlayerInRange(); // Oyuncunun algılama alanında olup olmadığını kontrol et
    }

    private void Patrol()
    {
        // Hedef devriye noktasına doğru hareket et
        transform.position = Vector2.MoveTowards(transform.position, currentPatrolTarget.position, speed * Time.deltaTime);

        // Devriye noktasına ulaşıldıysa
        if (Vector2.Distance(transform.position, currentPatrolTarget.position) < 0.1f)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= patrolWaitTime)
            {
                // Devriye noktasını değiştir
                currentPatrolTarget = currentPatrolTarget == patrolPoint1 ? patrolPoint2 : patrolPoint1;
                waitTimer = 0f;

                // Yüz yönünü değiştir
                FlipDirection();
            }
        }
    }

    private void FlipDirection()
    {
        // Yüz yönünü tersine çevir
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void ChasePlayer()
    {
        // Oyuncunun bulunduğu pozisyona doğru hareket et
        Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, transform.position.z); // Y eksenini sabit tut
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Oyuncu daha uzakta ise yüz yönünü değiştir
        if ((targetPosition.x < transform.position.x && transform.localScale.x > 0) ||
            (targetPosition.x > transform.position.x && transform.localScale.x < 0))
        {
            FlipDirection();
        }
    }

    private void CheckPlayerInRange()
    {
        // Oyuncunun algılama alanında olup olmadığını kontrol et
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= detectionRadius)
        {
            isChasing = true; // Oyuncuyu takip etmeye başla
        }
        else
        {
            isChasing = false; // Devriye hareketine geri dön
        }
    }

    private bool IsGroundAhead()
    {
        // Önünde zemin var mı kontrol et
        return Physics2D.Raycast(groundCheck.position, Vector2.down, 1f, groundLayer);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Oyuncu çarpışma kontrolü
        if (collision.gameObject.CompareTag("Player"))
        {
            HealthController healthController = collision.gameObject.GetComponent<HealthController>();
            if (healthController != null)
        {
            healthController.TakeDamage(10f); // Belirli bir hasar miktarı uygula
        }
            // Çarpışan Collider'ın head Collider olup olmadığını kontrol et
            if (collision.otherCollider == head.GetComponent<Collider2D>())
            {
                Debug.Log("Canavar kafasına basıldı! Yok olma başlıyor...");
                StartCoroutine(Die()); // Ölme sürecini başlat
                healthController.addhealth();
                
            }
        }

    }

    private IEnumerator Die()
    {
        isDead = true; // Canavar artık ölü
        animator.SetBool("isdead", true); // Ölme animasyonunu başlat

        // Ölme animasyonu süresini hesapla
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Yok olma süresi için ek bekleme
        yield return new WaitForSeconds(destroyDelay);

        Destroy(gameObject); // Canavarı yok et
    }

    private void OnDrawGizmosSelected()
    {
        // Algılama alanını çizmek için
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Zemin kontrolü için ray çizimi
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * 1f);
        }
    }
}
