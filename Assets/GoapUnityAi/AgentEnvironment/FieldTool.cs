using System;
namespace GoapUnityAi
{
    public class FieldTool : SmartObject
    {
        public float HP = 100;

        public void TakeDamage(float damage)
        {
            HP = Math.Max(HP - damage, 0);
        }

        public void RestoreHp(float hp)
        {
            HP = Math.Min(HP + hp, 100);
        }
    }
}
