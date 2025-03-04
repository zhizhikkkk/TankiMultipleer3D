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
            Debug.LogError(" MeshRenderer �� ������ �� � ����� �� �������� �������� �����!");
        }
    }

    [PunRPC]
    public void SetColorRPC(float r, float g, float b)
    {
        if (tankRenderers == null || tankRenderers.Length == 0)
        {
            Debug.LogError(" tankRenderers ������! ��������, ������ ��� �� ���������������.");
            return;
        }

        Color color = new Color(r, g, b);
        foreach (var renderer in tankRenderers)
        {
            renderer.material.color = color;
        }
        Debug.Log($" ���� ����� ������� �� {color}");
    }
}
