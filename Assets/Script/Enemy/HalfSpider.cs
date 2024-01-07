using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HalfSpider : Enemy
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        float[] tmpArray = new float[2] { 0f, 0f };

        if (HPOn == false && other.tag == "PlayerAttack" || other.tag == "StrongPlayerAttack" || other.tag == "PlayerDodgeAttack")
        {

            HPOn = true;
            SetHpBar();
        }

        if (other.tag == "PlayerAttack" && isHit == false)
        {
            isHit = true;
            Debug.Log("Damaged!");

            var playerdata = other.transform.GetComponentInParent<Player>();
            tmpArray = playerdata.PlayerAttack(EnemyHP);

            EnemyHP = tmpArray[0];

            hpBarImage.fillAmount = EnemyHP / FullHP;

            GameManager_JS.Instance.GuageUpdate(playerdata.PlayerSpecialAttackFillingAmount);
            GameManager_JS.Instance.Guage();

            debuffChecker.DebuffCheck((int)tmpArray[1]);
            StartCoroutine(GetDamaged());
        }
        else if (other.tag == "StrongPlayerAttack" && isHit == false)
        {
            Debug.Log("Strongly Damaged!");
            var playerdata = other.transform.GetComponentInParent<Player>();
            EnemyHP = (playerdata.PlayerStrongAttack(EnemyHP));
            tmpArray = playerdata.PlayerAttack(EnemyHP);
            EnemyHP = tmpArray[0];

            hpBarImage.fillAmount = EnemyHP / FullHP;

            StartCoroutine(GetDamaged());
            debuffChecker.DebuffCheck((int)tmpArray[1]);
        }
        else if (other.tag == "PlayerDodgeAttack" && isHit == false)
        {
            isHit = true;
            Debug.Log("Dodge damaged!");

            var playerdata = other.transform.GetComponentInParent<Player>();
            tmpArray = playerdata.PlayerDodgeAttack(EnemyHP);

            EnemyHP = tmpArray[0];

            hpBarImage.fillAmount = EnemyHP / FullHP;

            debuffChecker.DebuffCheck((int)tmpArray[1]);
            StartCoroutine(GetDamaged());
        }

        if (EnemyHP <= 0)
        {
            hpBarImage2.fillAmount = 0;
            enemySpawner.EnemyDead();
            gameObject.SetActive(false);
        }
    }

    void OnParticleCollision(GameObject other)
    {
        float[] tmpArray = new float[2] { 0f, 0f };

        if (other.tag == "PlayerAttack" && isHit == false)
        {
            isHit = true;
            Debug.Log("Damaged!");

            var playerdata = other.transform.GetComponentInParent<Player>();
            tmpArray = playerdata.PlayerAttack(EnemyHP);

            EnemyHP = tmpArray[0];

            debuffChecker.DebuffCheck((int)tmpArray[1]);
            StartCoroutine(GetDamaged());
        }
        else if (other.tag == "StrongPlayerAttack" && isHit == false)
        {
            Debug.Log("Strongly Damaged!");
            var playerdata = other.transform.GetComponentInParent<Player>();
            EnemyHP = (playerdata.PlayerStrongAttack(EnemyHP));
            tmpArray = playerdata.PlayerAttack(EnemyHP);
            EnemyHP = tmpArray[0];
            StartCoroutine(GetDamaged());
            debuffChecker.DebuffCheck((int)tmpArray[1]);
        }

        if (EnemyHP <= 0)
        {
            enemySpawner.EnemyDead();
            gameObject.SetActive(false);
        }
    }


    IEnumerator GetDamaged()
    {
        SR.material.color = Color.red;

        yield return new WaitForSeconds(0.6f);
        SR.material.color = Color.white;
        isHit = false;

    }
}
    