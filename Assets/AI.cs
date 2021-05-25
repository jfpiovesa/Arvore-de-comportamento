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
        InvokeRepeating("UpdateHealth", 5, 0.5f);
    }

    void Update()
    {
        Vector3 healthBarPos = Camera.main.WorldToScreenPoint(this.transform.position);
        healthBar.value = (int)health;//falando que barra de vida é igual a vida
        healthBar.transform.position = healthBarPos + new Vector3(0, 60, 0);
    }

    void UpdateHealth()
    {
        if (health < 100)// se a vida for menor que 100 adicionar mais vida
            health++;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "bullet")// se colider com a teg bullet tire  x de vida 
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
    [Task]
    public void PickDestination(int x, int z)
    {
        Vector3 dest = new Vector3(x, 0, z);// posição de destino
        agent.SetDestination(dest);// movendo pra posição de destino
        Task.current.Succeed();
    }
    [Task]
    public void TargetPlayer()
    {
        target = player.transform.position;// pegando posição do player 
        Task.current.Succeed();
    }
    [Task]
    public bool Fire()
    {
        GameObject bullet = GameObject.Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);//instaciando prefab da bala
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 2000);// dando força e velocida quando a bala for instaciada
        return true;
    }
    [Task]
    public void LookAtTarget()
    {
        Vector3 direction = target - this.transform.position;// calculando destino ate o alvo
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotSpeed);// fazendo calculo de rotação para que o objeto fique direcinado ao alvo
        if (Task.isInspected)

            Task.current.debugInfo = string.Format("angle={0}", Vector3.Angle(this.transform.forward, direction));// movendo ate o alvo
        if (Vector3.Angle(this.transform.forward, direction) < 5.0f)
        {
            Task.current.Succeed();
        }
    }
    [Task]
    bool SeePlayer()
    {
        Vector3 distance = player.transform.position - this.transform.position; // calculando a distacia ate o player 
        RaycastHit hit;// hit para colisão
        bool seeWall = false;
        Debug.DrawRay(this.transform.position, distance, Color.red);
        if (Physics.Raycast(this.transform.position, distance, out hit))// para ver se  esta colidindo com a  parede  
        {
            if (hit.collider.gameObject.tag == "wall")
            {
                seeWall = true;
            }
        }
        if (Task.isInspected)
            Task.current.debugInfo = string.Format("wall={0}", seeWall);
        if (distance.magnitude < visibleRange && !seeWall)// se distacia  for menor que o raio e a colisão  retorne verdadeiro 
            return true;

        else return false;
    }
    [Task]
    bool Turn(float angle) //rotação do objeto 
    {
        var p = this.transform.position + Quaternion.AngleAxis(angle, Vector3.up) * this.transform.forward;
        target = p;
        return true;
    }
}
    

