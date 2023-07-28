using UnityEngine;

namespace Infrastructure.StaticData.PlayerData
{
    [CreateAssetMenu(fileName = "PlayerStaticData", menuName = "StaticData/PlayerStaticData")]
    public class PlayerStaticData : ScriptableObject
    {
        public float MovementSpeed;
        public float RotationSpeed;
        
        public float MinMoveInputMagnitude;
        public float MinVelocityMagnitude;

        public float GiveAwayResourceDuration;
        
        public Vector3 AngleBetweenTreeAndPlayer;
    }
}