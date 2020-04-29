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
            var end = state.EndTimestamp;
            if (end <= dateTime)
            {
                // after, doesn't overlap.
                return false;
            }
            
            var start = state.StartTimestamp;
            if (start > dateTime)
            {
                // before, doesn't overlap.
                return false;
            }
            
            return true;
        }
    }
}