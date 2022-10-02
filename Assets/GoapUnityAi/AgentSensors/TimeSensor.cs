using GameSystems.GameTime;
using UnityEngine;

namespace GoapUnityAi
{
    public class TimeSensor : MonoBehaviour
    {
        public GoapAgent goapAgent;
        private GameTimeSystem gameTimeSystem;

        private void Awake()
        {
            gameTimeSystem = FindObjectOfType<GameTimeSystem>();
        }

        private void Update()
        {
            var currentTimeInfo = gameTimeSystem.GetCurrentTimeInfo();

            switch (currentTimeInfo.TimeOfDay)
            {
                case TimeOfDay.Morning:
                    goapAgent.GetCurrentState().SetFact(GoapFactName.TimeToWork, true);
                    goapAgent.GetCurrentState().SetFact(GoapFactName.TimeToSleep, false);
                    break;
                case TimeOfDay.Night:
                    goapAgent.GetCurrentState().SetFact(GoapFactName.TimeToWork, false);
                    goapAgent.GetCurrentState().SetFact(GoapFactName.TimeToSleep, true);
                    goapAgent.GetCurrentState().SetFact(GoapFactName.HasEnergy, false);
                    break;
            }
        }
    }
}
