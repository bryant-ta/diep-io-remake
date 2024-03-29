﻿using UnityEngine;

public class Bullet : Projectile
{
    // Bullet Attributes
    [ShowOnly] public float moveSpeed;
    [ShowOnly] public float lifetime;

    Vector2 dir;
    public bool isCaltrop;

    public void Setup(GameObject caller, int dmg, float accuracy, float moveSpeed, float lifetime)
    {
        base.Setup(caller, dmg, accuracy);
        this.moveSpeed = moveSpeed;
        this.lifetime = lifetime;
        dir = (Quaternion.Euler(0, 0, shotAngle) * parentGun.getBarrel().transform.right);

        if (isCaltrop) GetComponent<Rigidbody2D>().AddForce(dir * moveSpeed * 10);

        if (lifetime > -1)
            Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        if (!isCaltrop)
        {
            transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Hit Player
        if (col.GetComponent<PlayerHealth>() != null && col.gameObject != parentGun.owner)
        {
            if(col.GetComponent<PlayerHealth>().DoDamage(dmg) == 1) // A kill
            {
                parentGun.owner.GetComponent<PlayerLevel>().AddExp(col.GetComponent<Damageable>().getExp());
            }
            Destroy(gameObject);
        } 
        // Hit Poly
        else if(col.GetComponent<Damageable>() != null)
        {
            if (col.GetComponent<Damageable>().DoDamage(dmg) == 1)
            {
                parentGun.owner.GetComponent<PlayerLevel>().AddExp(col.GetComponent<Damageable>().getExp());
            }
            Destroy(gameObject);
        }
        // Hit wall
        else if (col.tag == "Environment")
        {
            Destroy(gameObject);
        }
    }

    public Vector2 getMoveDir() { return dir; }
}
