using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour {

	float speed = 20.0F;// velocidade
    float rotationSpeed = 120.0F;// velocidade da  rotação 
    public GameObject bulletPrefab;// prefab da bala
    public Transform bulletSpawn;// local onde a  bala vai spawnar 

    void Update() {
        float translation = Input.GetAxis("Vertical") * speed;//movimentação de andar
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;// movimentação de rotação 
        translation *= Time.deltaTime;// dando força de movimentação pra frente ou pra traz em soma com time da maquina
        rotation *= Time.deltaTime;// dando força de movimentação pra rotacão esquerda ou direita em soma com time da maquina
        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);

        if(Input.GetKeyDown("space"))// se aperta  espaço
        {
            GameObject bullet = GameObject.Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);// instaciando o prefab da bala ao aperta espaço
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward*2000); // dando força e velocida quando a bala for instaciada
        }
    }
}
