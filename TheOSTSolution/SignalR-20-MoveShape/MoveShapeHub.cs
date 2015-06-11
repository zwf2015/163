using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace SignalR_20_MoveShape
{
    //public class Broadcaster 
    //{
    //    private readonly static Lazy<Broadcaster> _instance = new Lazy<Broadcaster>(() => new Broadcaster());
    //    private readonly TimeSpan BroadcastInterval = TimeSpan.FromMilliseconds(40);
    //    private readonly IHubContext _hubContext;
    //    private ShapeModel _model;
    //    private bool _modelUpdated;
    //    public Broadcaster()
    //    {
    //        _hubContext = GlobalHost.ConnectionManager.GetHubContext<MoveShapeHub>();
    //        _model = new ShapeModel();
    //        _modelUpdated = false;
    //        _broadcastLoop = new Timer(Broadcaster, null, BroadcastInterval, BroadcastInterval);
    //    }
    //}

    public class MoveShapeHub : Hub
    {
        public void UpdateModel(ShapeModel clientModel)
        {
            clientModel.LastUpdateById = Context.ConnectionId;
            Clients.AllExcept(clientModel.LastUpdateById).updateShape(clientModel);
        }
    }

    public class ShapeModel
    {
        [JsonProperty("left")]
        public double Left { get; set; }
        [JsonProperty("top")]
        public double Top { get; set; }
        [JsonIgnore]
        public string LastUpdateById { get; set; }
    }
}