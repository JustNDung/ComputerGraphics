using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    public GameObject FirstCamera;
    public GameObject SecondCamera;
    public int Manager;

    public void ManageCamera()
    {
        if (Manager == 0)
        {
            SecondCam();
            Manager = 1;
        }
        else
        {
            FirstCam();
            Manager = 0;
        }
    }
    
    private void FirstCam()
    {
        FirstCamera.SetActive(true);
        SecondCamera.SetActive(false);
    }
    
    private void SecondCam()
    {
        FirstCamera.SetActive(false);
        SecondCamera.SetActive(true);
    }
}
