using UnityEngine;

namespace DuelSystem
{
    public interface IDuelMember
    {
        Transform GetTransform();
        void DoShoot(IDuelMember target);
        void Die();
    }
}