using System;

namespace OsmSharp.Replication
{
    public static class ReplicationStateExtensions
    {
        /// <summary>
        /// Returns true if the given replication state represents a diff overlapping the given date/time.
        /// </summary>
        /// <param name="state">The replication state.</param>
        /// <param name="dateTime">The date/time.</param>
        /// <returns>True if the given date/time is in the range ]state.timestamp - period, state.timestamp].</returns>
        public static bool Overlaps(this ReplicationState state, DateTime dateTime)
        {
            var start = state.Timestamp.AddSeconds(-state.Config.Period);
            var end = state.Timestamp;

            if (start < dateTime &&
                end >= dateTime)
            {
                return true;
            }

            return false;
        }
    }
}