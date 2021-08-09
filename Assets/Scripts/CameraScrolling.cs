using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScrolling : MonoBehaviour
{
    public float speed;
    Vector2 direction;
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;

    void Update()
    {
        if (target)
        {
            Vector3 point = Camera.main.WorldToViewportPoint(new Vector3(target.position.x, target.position.y + 0.75f, target.position.z));
            Vector3 delta = new Vector3(target.position.x, target.position.y + 0.75f, target.position.z) - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;

            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }

        //берём компоненты игрока

        //правая ось Х камеры относительно центра камеры
        //левая ось Х камеры относительно центра камеры
        //записываем текущее положение камеры в переменную

        //визуализация осей х и у
        //Gizmos.DrawLine((focusRight, - Camera.main.pixelHeight/2),(focusRight, Camera.main.pixelHeight / 2));
        //если позиция игрока дальше чем правая х или ближе чем левая х
        //выполняется скролл

        //перемещенеие камеры по оси х - расстояние между предыдущим положением камеры и положением игрока
        //движение камеры по оси х - перемещение / 32 и * скорость камеры
        //если игрок разворачивается
    }
}
