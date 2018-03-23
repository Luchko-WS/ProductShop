using System;

namespace Utils
{
    public static class EventHelper
    {
        public static void Invoke(EventHandler handler, object sender)
        {
            if (handler != null) handler.Invoke(sender, null);
        }

        public static void Invoke(EventHandler handler, object sender, EventArgs args)
        {
            if (handler != null) handler.Invoke(sender, args);
        }
    }
}
