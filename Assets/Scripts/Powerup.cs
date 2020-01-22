using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField] // 0 = Triple Shot, 1 = Speed, 2 = Shields, 3 = Ammo
    private int powerUpId;

    // Update is called once per frame
    void Update()
    {
        float yLowerBounds = -5.0f;
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < yLowerBounds)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            if(player != null)
            {
                switch(powerUpId)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldBoostActive();
                        break;
                    case 3:
                        player.CollectAmmo();
                        break;
                    case 4:
                        player.CollectHealth();
                        break;
                    default:
                        Debug.Log("Default");
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
