﻿using NTMiner.Controllers;
using NTMiner.ServerNode;
using System;
using System.Collections.Generic;

namespace NTMiner.Services.Official {
    public class WsServerNodeService {
        private readonly string _controllerName = RpcRoot.GetControllerName<IWsServerNodeController>();

        public WsServerNodeService() {
        }

        public void GetNodesAsync(Action<DataResponse<List<WsServerNodeState>>, Exception> callback) {
            RpcRoot.SignPostAsync(RpcRoot.OfficialServerHost, RpcRoot.OfficialServerPort, _controllerName, nameof(IWsServerNodeController.Nodes), new SignRequest(), callback);
        }

        public void GetNodeAddressesAsync(Action<DataResponse<string[]>, Exception> callback) {
            RpcRoot.SignPostAsync(RpcRoot.OfficialServerHost, RpcRoot.OfficialServerPort, _controllerName, nameof(IWsServerNodeController.NodeAddresses), new SignRequest(), callback);
        }

        public void GetNodeAddressAsync(Guid clientId, string outerUserId, Action<DataResponse<string>, Exception> callback) {
            var data = new GetNodeAddressRequest {
                ClientId = clientId,
                UserId = outerUserId
            };
            RpcRoot.PostAsync(RpcRoot.OfficialServerHost, RpcRoot.OfficialServerPort, _controllerName, nameof(IWsServerNodeController.GetNodeAddress), data, callback, timeountMilliseconds: 8000);
        }

        public void ReportNodeStateAsync(WsServerNodeState nodeState, Action<ResponseBase, Exception> callback) {
            RpcRoot.SignPostAsync(RpcRoot.OfficialServerHost, RpcRoot.OfficialServerPort, _controllerName, nameof(IWsServerNodeController.ReportNodeState), nodeState, callback, timeountMilliseconds: 3000);
        }

        public void RemoveNodeAsync(string address, Action<ResponseBase, Exception> callback) {
            RpcRoot.SignPostAsync(RpcRoot.OfficialServerHost, RpcRoot.OfficialServerPort, _controllerName, nameof(IWsServerNodeController.RemoveNode), new DataRequest<string> {
                Data = address
            }, callback);
        }
    }
}
