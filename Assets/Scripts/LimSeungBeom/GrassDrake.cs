using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class GrassDrake : Enemy
{

    GameObject Player;
    GameObject Laser;
    [SerializeField] GameObject AttackWarning; //���� �� ���ǥ��
    [SerializeField] GameObject ExplosionAttack;         //���� ��ƼŬ

    GameObject DRAttack;
    GameObject DRAttackWarning;



    Vector3 SavedPlayerPosition;

    float eX;
    bool LaserOn;
    Vector3 Aimpoint;

    private Queue <GameObject> AWQueue = new Queue <GameObject>();
    private Queue<GameObject> AQueue = new Queue<GameObject>();
   
    void Start()
    {
        Player = GameManager.Instance.Player.gameObject;
        Laser= transform.GetChild(0).gameObject;
        Laser.SetActive(false);

        for(int i = 0;i < 5;i++)
        {
            GameObject DRAttack = Instantiate(ExplosionAttack);
            GameObject DRAttackWarning = Instantiate(AttackWarning);

            DRAttack.SetActive(false);
            DRAttackWarning.SetActive(false);

            AWQueue.Enqueue(DRAttackWarning);
            AQueue.Enqueue(DRAttack);
        }

    }

    
    protected override void Update()
    {
        base.Update();
        Laser.transform.LookAt(Aimpoint);
        if(LaserOn)
        {
            Laser.SetActive(true);
            Aimpoint = Vector3.Lerp(new Vector3(SavedPlayerPosition.x - 5, 0, SavedPlayerPosition.z), new Vector3(SavedPlayerPosition.x + 5, 0, SavedPlayerPosition.z), eX);
            eX += Time.deltaTime * 0.35f;
            Debug.Log("�巹��ũ ����");
        }
        if(!LaserOn)
        {
            Laser.SetActive(false);
            eX = 0;
            Debug.Log("�巹��ũ ���� ��");
        }
    }
    protected override void Attack()
    {
        StartCoroutine(AttackCoroutine());
    }
    IEnumerator AttackCoroutine()
    {
        StartCoroutine(Stop(12));
        SavedPlayerPosition = Player.transform.position;
        LaserOn = true;
        yield return new WaitForSeconds(0.6f);
        StartCoroutine(ActiveAW(-3f));
        yield return new WaitForSeconds(0.6f);
        StartCoroutine(ActiveAW(-1.5f));
        yield return new WaitForSeconds(0.6f);
        StartCoroutine(ActiveAW(0));
        yield return new WaitForSeconds(0.6f);
        StartCoroutine(ActiveAW(1.5f));
        yield return new WaitForSeconds(0.6f);
        StartCoroutine(ActiveAW(3f));
        /*
        for (int i = 0; i < 5; i++)
        {
            b += 2.5f * i;
            StartCoroutine(ActiveAW(b));
            Debug.Log(b);
            yield return new WaitForSeconds(0.6f);
            b = -5;
        }              // 3�ʵ��� 5�� ����.
        */
        yield return new WaitForSeconds(1);
        LaserOn = false;
        yield return new WaitForSeconds(1);
        StartCoroutine(ActiveAttack(-3f));
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(ActiveAttack(-1.5f));
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(ActiveAttack(0));
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(ActiveAttack(1.5f));
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(ActiveAttack(3f));
        /*
        for(int i = 0; i < 5; i++)
        {
            b += 2.5f * i;
            StartCoroutine(ActiveAttack(b));
            yield return new WaitForSeconds(0.4f);
            b = -5;
        }
        */
    }
    IEnumerator ActiveAW(float a)
    {
        GameObject aw = AWQueue.Dequeue().gameObject;
        aw.SetActive(true);
        aw.transform.position = new Vector3(SavedPlayerPosition.x + a, 0, SavedPlayerPosition.z + 1);
        AWQueue.Enqueue(aw);
        yield return new WaitForSeconds(5);
        aw.SetActive(false);
    }
    IEnumerator ActiveAttack(float a)
    {
        GameObject Attack = AQueue.Dequeue().gameObject;
        Attack.SetActive(true);
        Attack.transform.position = new Vector3(SavedPlayerPosition.x + a, 0, SavedPlayerPosition.z + 1);
        AQueue.Enqueue(Attack);

        yield return new WaitForSeconds(2);
        Attack.SetActive(false);
    }

}