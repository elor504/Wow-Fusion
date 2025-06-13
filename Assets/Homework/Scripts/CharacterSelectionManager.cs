using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Homework
{
    public class CharacterSelectionManager : NetworkBehaviour
    {
        [SerializeField] private SessionUI sessionUI;

        [SerializeField] private List<CharacterSelection> characterSelection;
        public List<CharacterSelection> GetCharacterSelectionList => characterSelection;

        public static Action OnSelectedCharacter;

        public override void Spawned()
        {
            base.Spawned();
            GameManagerHW.Instance.CharacterSelectionManager = this;
        }
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPCSetCharacterSelection(int index, RpcInfo rpcInfo = default)
        {
            if (characterSelection[index].IsSelected)
            {
                Debug.LogError($"Attempting to select unselectable index {index}");
                return;
            }
            int previousIndex = GetCharacterSelectionIndexByPlayerRef(rpcInfo.Source);
            CharacterSelection existedSelection = GetCharacterByPlayerRef(rpcInfo.Source);
            if (existedSelection != null)
            {
                existedSelection.playerRef = default;
                existedSelection.IsSelected = false;
            }

            Debug.Log($"Attempting to select on index: {index}");
            characterSelection[index].playerRef = rpcInfo.Source;
            characterSelection[index].IsSelected = true;

            RPCUpdateCharacterSelection(index, previousIndex);
        }
        [Rpc(RpcSources.StateAuthority,RpcTargets.All)]
        private void RPCUpdateCharacterSelection(int index,int previousIndex)
        {
            characterSelection[index].IsSelected = true;
            if(previousIndex != -1)
            {
                characterSelection[previousIndex].IsSelected = false;
            }
            OnSelectedCharacter?.Invoke();
        }
        public bool CheckIfCharacterIsAvailable(int index)
        {
            return !characterSelection[index].IsSelected;
        }
        public Color GetCharacterColor(int index)
        {
            return characterSelection[index].color;
        }
        public PlayerRef GetCharacterPlayerRef(int index)
        {
            return characterSelection[index].playerRef;
        }
        public void SetCharacterColor(PlayerRef playerRef, Color color)
        {
            GetCharacterByPlayerRef(playerRef).color = color;
        }
        public Color GetColorByPlayerRef(PlayerRef playerRef) 
        {
            return GetCharacterByPlayerRef(playerRef).color;
        }
        private int GetCharacterSelectionIndexByPlayerRef(PlayerRef playerRef)
        {
            CharacterSelection selection = GetCharacterByPlayerRef(playerRef);
            int index = selection != null? characterSelection.FindIndex(c => c.Equals(selection)) : -1;
            return index;
        }

        public CharacterSelection GetCharacterByPlayerRef(PlayerRef playerRef) 
        {
            return characterSelection.Find(c => c.playerRef == playerRef);
        }

    }
    [Serializable]
    public class CharacterSelection
    {
        public PlayerRef playerRef;
        public bool IsSelected;
        public Color color;
    }
}