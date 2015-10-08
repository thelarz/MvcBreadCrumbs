using System.Collections.Generic;
using System.Linq;

namespace MvcBreadCrumbs
{
    public class StateManager
    {
        public static readonly List<State> States = new List<State>();

        public static State GetState(string id)
        {
            if (States.FirstOrDefault(x => x.SessionCookie == id) == null)
            {
                StateManager.CreateState(id);
            }
            return States.First(x => x.SessionCookie == id);
        }

        public static State CreateState(string cookie)
        {
            var newstate = new State(cookie);
            States.Add(newstate);

            return newstate;

        }

        public static void RemoveState(string id)
        {
            var state = GetState(id);
            if (state != null)
            {
                States.Remove(state);
            }
        }

    }
}