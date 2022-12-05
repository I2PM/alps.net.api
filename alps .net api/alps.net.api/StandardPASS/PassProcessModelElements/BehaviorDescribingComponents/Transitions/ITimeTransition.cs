﻿namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the time transition class
    /// </summary>
    public interface ITimeTransition : ITransition
    {

        public enum TimeTransitionType
        {
            TimeBasedReminder,
            BusinessDayTimer,
            CalendarBasedReminder,
            DayTimeTimer,
            YearMonthTimer
        }

        public void setTimeTransitionType(TimeTransitionType type);

        public TimeTransitionType getTimeTransitionType();

        public new ITimeTransitionCondition getTransitionCondition();
    }

}
