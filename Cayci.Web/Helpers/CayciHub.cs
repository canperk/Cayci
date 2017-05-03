using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Cayci.Helpers
{
    [HubName("Cayci")]
    public class CayciHub : Hub
    {
        public void JoinGroup(string groupName)
        {
            Groups.Add(Context.ConnectionId, groupName);
        }
    }
}