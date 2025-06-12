using Fusion;
namespace Homework
{
    public class PlayerSessionData : NetworkBehaviour
    {
        [Networked] private string _nickName { get; set; }

        public override void Spawned()
        {

        }

        public void UpdateNickName(string nickName)
        {
            if (HasStateAuthority)
            {
                _nickName = nickName;
            }
        }
    }
}