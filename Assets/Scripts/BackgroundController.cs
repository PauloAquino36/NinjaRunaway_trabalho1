using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private float skySpeed = 0.1f;

    private Transform skyObj;
    private Material skyMaterial;
    private float offset = 0;

    void Start()
    {
        skyObj = transform.GetChild(0);
        MeshRenderer mr = skyObj.GetComponent<MeshRenderer>();
        skyMaterial = mr.material;
    }

    void Update()
    {
        // Faz o background seguir a câmera
        Vector3 skyPos = Camera.main.transform.position;
        skyPos.z = 0;
        skyObj.position = skyPos;

        // Move a textura para criar o efeito de rolagem
        offset += skySpeed * Time.deltaTime;
        Vector2 voff = new Vector2(offset, 0);
        skyMaterial.SetTextureOffset("_MainTex", voff);
    }
}
