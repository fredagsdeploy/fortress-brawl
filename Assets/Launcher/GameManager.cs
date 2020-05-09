using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;


namespace dev.fredag.fortressbrawl.launcher
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameManager Instance;
        
        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;

        private void Start()
        {
            Instance = this;
            
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0,0,0), Quaternion.identity, 0);
            } 
        }


        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }
        


        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
        
        void LoadGameScene()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            Debug.Log("PhotonNetwork : Loading Level");
            PhotonNetwork.LoadLevel("GameScene");
        }
        
        
        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
            
        }


        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects
        }
    }
}