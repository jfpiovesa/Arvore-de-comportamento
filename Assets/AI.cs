using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using Panda;

public class AI : MonoBehaviour
{
    public Transform player;// player
    public Transform bulletSpawn;// onde vai spawn da bala 
    public Slider healthBar;   // barra de vida
    public GameObject bulletPrefab;// prefab bala

    NavMeshAgent agent;
    public Vector3 destination; // The movement destination.
    public Vector3 target;      // The position to aim to.
    float health = 100.0f;// vida 
    float rotSpeed = 5.0f;// velocidade rotação

    float visibleRange = 80.0f;//
    float shotRange = 40.0f;//

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();//pegando componete navemeshe
        agent.stoppingDistance = shotRange - 5; //for a little buffer
        InvokeRepeating("UpdateHealth",5,0.5f);
    }

    void Update()
    {
        Vector3 healthBarPos = Camera.main.WorldToScreenPoint(this.transform.position);
        healthBar.value = (int)health;//falando que barra de vida é igual a vida
        healthBar.transform.position = healthBarPos + new Vector3(0,60,0);
    }

    void UpdateHealth()
    {
       if(health < 100)// se a vida for menor que 100 adicionar mais vida
        health ++;
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "bullet")// se colider com a teg bullet tire  x de vida 
        {
            health -= 10;
        }
    }
    [Task]
    public void PickRandomDestination() 
    {
        Vector3 dest = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100));// destino randomico  entre x valor a x valor 
        agent.SetDestination(dest); // dando o destino do objeto, com o destino randomico
        Task.current.Succeed();
    }
    [Task] 
    public void MoveToDestination()// metodo de movimentação
    {
        if (Task.isInspected) 
            Task.current.debugInfo = string.Format("t={0:0.00}", Time.time); //mostrando tempo de execução 

        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        { 
            Task.current.Succeed();  
        } 
    }
}

