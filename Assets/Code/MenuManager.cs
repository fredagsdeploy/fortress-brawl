
using dev.fredag.fortressbrawl.launcher;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    
    public void Quit()
    {
        GameManager.Instance.LeaveRoom();
    }
    
}
