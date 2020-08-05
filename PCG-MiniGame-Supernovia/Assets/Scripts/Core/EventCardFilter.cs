using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    public class EventCardFilter {
        public static EventCard[] Filt() {
            var qualifedEvents = new List<EventCard>();
            foreach (var e in StoryContext.instance.eventDeck.ToArray()) {
                if (e.preconditonSet.SatisfiedByCurrentContext()) {
                    qualifedEvents.Add(e);
                }
            }
            return qualifedEvents.ToArray();
        }
    }
}