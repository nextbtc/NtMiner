﻿using NTMiner.ServerNode;

namespace NTMiner.Controllers {
    public interface IWebApiServerNodeController {
        DataResponse<WebApiServerState> GetServerState(SignRequest request);
    }
}
