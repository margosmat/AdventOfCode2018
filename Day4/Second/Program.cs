using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Second
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] inputLines;
            using (var sr = new StreamReader("input.txt"))
            {
                var input = sr.ReadToEnd();
                inputLines = input.Split("\n");
            }

            var parsedEvents = inputLines.Where(item => !string.IsNullOrWhiteSpace(item)).Select(line => ParseEventString(line)).ToList();
            parsedEvents.Sort();
            var guardsWithSleepRegister = GetGuardsWithSleepRegister(parsedEvents);
            var answer = GetTheAnswer(guardsWithSleepRegister);
            Console.WriteLine(answer.ToString());
        }

        public static GuardPostEvent ParseEventString(string eventString)
        {
            var eventType = GetEventType(eventString);
            var guardId = GetGuardId(eventString);
            var time = GetTime(eventString);
            return new GuardPostEvent(time, guardId, eventType);
        }

        public static EventType GetEventType(string eventString)
        {
            if (eventString.Contains("wakes")) return EventType.wakesUp;
            if (eventString.Contains("falls"))return EventType.fallsAsleep;
            return EventType.startsShift;
        }

        public static int? GetGuardId(string eventString)
        {
            if (eventString.Contains("begins"))
            {
                string id = eventString.Trim().Remove(0, eventString.IndexOf('#') + 1);
                id = id.Remove(id.IndexOf('b'));
                return Int32.Parse(id);
            }

            return null;
        }

        public static DateTime GetTime(string eventString)
        {
            var dateString = eventString.Remove(eventString.IndexOf(']')).Remove(0, 1);
            var time = DateTime.Parse(dateString, null, System.Globalization.DateTimeStyles.None);
            return time;
        }

        public static Dictionary<int, List<(DateTime, DateTime)>> GetGuardsWithSleepRegister(List<GuardPostEvent> events)
        {
            var guardsSleepRegister = new Dictionary<int, List<(DateTime, DateTime)>>();
            var eventsEnumerator = events.GetEnumerator();
            int currentGuardId = 0;
            DateTime? napStart = null;

            while (eventsEnumerator.MoveNext())
            {
                if (eventsEnumerator.Current.GuardEvent == EventType.startsShift)
                {
                    currentGuardId = eventsEnumerator.Current.GuardId.Value;
                }
                else if (eventsEnumerator.Current.GuardEvent == EventType.fallsAsleep)
                {
                    napStart = eventsEnumerator.Current.Time;
                }
                else if (napStart.HasValue)
                {
                    double sleepTime;
                    if (eventsEnumerator.Current.Time.TimeOfDay.Hours > 0)
                    {
                        sleepTime = eventsEnumerator.Current.Time.TimeOfDay.TotalMinutes + (60 - napStart.Value.TimeOfDay.TotalMinutes) - 1;
                    }
                    else
                    {
                        sleepTime = (eventsEnumerator.Current.Time.TimeOfDay - napStart.Value.TimeOfDay).TotalMinutes - 1;
                    }
                    
                    if (guardsSleepRegister.ContainsKey(currentGuardId)) 
                    {
                        guardsSleepRegister[currentGuardId].Add((napStart.Value, eventsEnumerator.Current.Time));
                    }
                    else
                    {
                        guardsSleepRegister.Add(currentGuardId, new List<(DateTime, DateTime)> {(napStart.Value, eventsEnumerator.Current.Time)});
                    }
                    napStart = null;
                }
            }

            return guardsSleepRegister;
        }

        public static int GetTheAnswer(Dictionary<int, List<(DateTime, DateTime)>> guardsSleepRegister)
        {
            var guardsWithMostSleptMinute = new Dictionary<int, (int, int)>();

            foreach (var guard in guardsSleepRegister)
            {
                var mostSleptMinuteWithCount = GetMostSleptMinuteWithCount(guard.Value);
                guardsWithMostSleptMinute.Add(guard.Key, mostSleptMinuteWithCount);
            }

            int mostSleptMinuteCount = guardsWithMostSleptMinute.Max(item => item.Value.Item2);
            int guardId = guardsWithMostSleptMinute.FirstOrDefault(item => item.Value.Item2 == mostSleptMinuteCount).Key;
            int mostSleptMinute = guardsWithMostSleptMinute.FirstOrDefault(item => item.Value.Item2 == mostSleptMinuteCount).Value.Item1;

            return guardId * mostSleptMinute;
        }

        public static (int, int) GetMostSleptMinuteWithCount(List<(DateTime, DateTime)> sleepRegister)
        {
            var mostSleptMinutes = new Dictionary<int, int>();
            for (int i = 0; i < 60; i++) mostSleptMinutes.Add(i, 0);

            foreach (var entry in sleepRegister)
            {
                int napStartMinute = (int)entry.Item1.TimeOfDay.TotalMinutes;
                int napEndMinute = (int)entry.Item2.TimeOfDay.TotalMinutes - 1;

                for (int i = napStartMinute; i <= napEndMinute; i++) mostSleptMinutes[i]++;
            }

            var mostSleptMinuteCount = mostSleptMinutes.Max(item => item.Value);

            return (mostSleptMinutes.FirstOrDefault(item => item.Value == mostSleptMinuteCount).Key, mostSleptMinuteCount);
        }

        public enum EventType
        {
            fallsAsleep,
            startsShift,
            wakesUp
        }

        public class GuardPostEvent : IComparable
        {
            public GuardPostEvent(DateTime time, int? guardId, EventType guardEvent)
            {
                this.Time = time;
                this.GuardId = guardId;
                this.GuardEvent = guardEvent;
            }

            public DateTime Time { get; private set; }

            public int? GuardId { get; private set; }

            public EventType GuardEvent { get; private set; }

            public int CompareTo(object obj)
            {
                if (obj == null) return 1;

                GuardPostEvent otherEvent = obj as GuardPostEvent;
                if (otherEvent != null)
                {
                    return this.Time.CompareTo(otherEvent.Time);
                }
                else
                {
                    throw new ArgumentException("Object is not a GuardPostEvent");
                }
            }
        }
    }
}
