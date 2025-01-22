using UnityEngine;

public class Kunai : MonoBehaviour
{
    public int Speed = 10;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        move();
        kill();
    }

    public void move(){
        transform.Translate(Vector3.up * Speed * Time.deltaTime);
    }

    public void kill() {
    Destroy(gameObject, 5f);
}

}
