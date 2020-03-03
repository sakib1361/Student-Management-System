using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mobile.Core.Models.Core
{
    public class ActivityCollection
    {
        readonly Dictionary<DateTime, ICollection> Activities;
        public ActivityCollection()
        {
            Activities = new Dictionary<DateTime, ICollection>();
        }

        public void AddData(Dictionary<DateTime, ICollection> newData)
        {
            foreach (var item in newData)
            {
                if (!Activities.ContainsKey(item.Key))
                {
                    Activities.Add(item.Key, item.Value);
                    DataChanged?.Invoke(this, null);
                }
            }
        }

        public void Clear()
        {
            Activities.Clear();
        }

        public ICollection GetCollection(DateTime dateTime)
        {
            if (Activities.TryGetValue(dateTime.Date, out ICollection _info))
            {
                return _info;
            }
            else
            {
                return null;
            }
        }

        public event EventHandler DataChanged;

        internal bool RequireData(DateTime start, DateTime end)
        {
            return !Activities.Keys.Any(x => x > start && x < end);
        }
    }
}
