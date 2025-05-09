using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   private static GameManager _instance;
   public static GameManager Instance => _instance;
   
   [SerializeField] private PlayerCharacter clientPlayer;

   [Header("UI")] [SerializeField] private GameObject playerHUD;
   
   
   public PlayerCharacter ClientPlayer => clientPlayer;

   private void Awake()
   {
      if (_instance == null)
      {
         _instance = this;
      }
      else if (_instance != this)
      {
         Destroy(this);
      }
      
      playerHUD.SetActive(true);
   }
}
