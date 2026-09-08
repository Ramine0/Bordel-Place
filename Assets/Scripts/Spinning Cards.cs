using UnityEngine;

public class SpinningCards : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(new Vector3(0, Time.deltaTime, 0));
    }
    
    public void Reset()
    {
        transform.rotation = Quaternion.identity;
    }
}
