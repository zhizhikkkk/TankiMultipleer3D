using Photon.Pun;
using UnityEngine;

public class TankColor : MonoBehaviourPun
{
    private MeshRenderer[] tankRenderers;

    void Awake() 
    {
        tankRenderers = GetComponentsInChildren<MeshRenderer>();

        if (tankRenderers.Length == 0)
        {
            Debug.LogError(" MeshRenderer не найден ни в одном из дочерних объектов танка!");
        }
    }

    [PunRPC]
    public void SetColorRPC(float r, float g, float b)
    {
        if (tankRenderers == null || tankRenderers.Length == 0)
        {
            Debug.LogError(" tankRenderers пустой! Возможно, объект еще не инициализирован.");
            return;
        }

        Color color = new Color(r, g, b);
        foreach (var renderer in tankRenderers)
        {
            renderer.material.color = color;
        }
        Debug.Log($" Цвет танка изменен на {color}");
    }
}
