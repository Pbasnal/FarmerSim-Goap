using System;
using System.Collections;
using Library.Logging;
using Library.Utilities;
using UnityEngine;

namespace GameSystems.GameTime
{
    public class GameTimeSystem : MonoBehaviour
    {
        [Button("BoostTime", "BoostTime button", true)] public int test1;

        [SerializeField] private int realTimeConversionRateToGameTime;

        [SerializeField] private float gameTimeSeconds;

        [SerializeField] private string currentGameTimeString;
        [SerializeField] private TimeOfDay timeOfDay;

        private DateTime currentGameTime;

        private DateTime startTime;

        // Start is called before the first frame update
        private void Awake()
        {
            startTime = DateTime.Now;
        }

        // Update is called once per frame
        private void Update()
        {
            gameTimeSeconds += Time.deltaTime * realTimeConversionRateToGameTime;
            currentGameTime = (startTime + TimeSpan.FromSeconds(gameTimeSeconds)).ToLocalTime();
            currentGameTimeString = currentGameTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
            timeOfDay = GetCurrentTimeOfDay(currentGameTime.Hour);
        }

        public TimeInfo GetCurrentTimeInfo()
        {
            return new TimeInfo
            {
                CurrentTime = currentGameTime,
                TimeOfDay = GetCurrentTimeOfDay(currentGameTime.Hour)
            };
        }

        public void BoostTime(int testvar)
        {
            // DebugLogger.LogDebug($"Current time of day {GetCurrentTimeOfDay(currentGameTime.Hour).ToString()}");
            BoostTimeMultiplier(5, TimeOfDay.Night);
        }

        public void BoostTimeMultiplier(int boostByAmount, TimeOfDay boostTimeTillTimeOfDay)
        {
            var dateTimeAfterBoost = currentGameTime;
            while (GetCurrentTimeOfDay(dateTimeAfterBoost.Hour) != boostTimeTillTimeOfDay)
            {
                dateTimeAfterBoost = dateTimeAfterBoost.AddHours(1);
            }

            // DebugLogger.LogDebug($"Time at the end of boost {dateTimeAfterBoost}");
            // DebugLogger.LogDebug($"Time of day after boosting {GetCurrentTimeOfDay(dateTimeAfterBoost.Hour).ToString()}");
            var revertToOriginalValueInSecs = (dateTimeAfterBoost - currentGameTime).TotalSeconds;
            if (revertToOriginalValueInSecs == 0)
            {
                return;
            }

            DebugLogger.LogDebug($"Boosting time for {revertToOriginalValueInSecs}secs");

            StartCoroutine(WaitForBoostedTimeToExpire(boostByAmount, dateTimeAfterBoost));
        }

        private TimeOfDay GetCurrentTimeOfDay(int timeInHour)
        {
            if (timeInHour > 22)
            {
                return TimeOfDay.Night;
            }
            else if (timeInHour > 15)
            {
                return TimeOfDay.Evening;
            }
            else
            {
                return TimeOfDay.Morning;
            }
        }

        private IEnumerator WaitForBoostedTimeToExpire(int boostedTimeMultiplier, DateTime endTimeOfBoost)
        {
            var originalTimeMultiplier = realTimeConversionRateToGameTime;
            realTimeConversionRateToGameTime *= boostedTimeMultiplier;

            // DebugLogger.LogDebug($"New time multiplier is {realTimeConversionRateToGameTime}");
            while (endTimeOfBoost > currentGameTime)
            {
                yield return new WaitForEndOfFrame();
            }

            realTimeConversionRateToGameTime = originalTimeMultiplier;
        }
    }

    public class TimeInfo
    {
        public DateTime CurrentTime { get; internal set; }
        public TimeOfDay TimeOfDay { get; internal set; }

        public long InGameTicksSinceStartOfTheGame { get; internal set; }
    }

    public enum TimeOfDay
    {
        Morning,
        Evening,
        Night
    }
}
