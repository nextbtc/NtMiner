﻿using NTMiner.Core;
using NTMiner.Core.MinerServer;
using System;
using System.Linq;
using System.Web.Http;

namespace NTMiner.Controllers {
    // 注意该控制器不能重命名
    public class KernelOutputKeywordController : ApiControllerBase, IKernelOutputKeywordController {
        [HttpPost]
        public KernelOutputKeywordsResponse KernelOutputKeywords(KernelOutputKeywordsRequest request) {
            try {
                var data = WebApiRoot.KernelOutputKeywordSet;
                return KernelOutputKeywordsResponse.Ok(data.AsEnumerable().Select(a => KernelOutputKeywordData.Create(a)).ToList(), Timestamp.GetTimestamp(WebApiRoot.KernelOutputKeywordTimestamp));
            }
            catch (Exception e) {
                Logger.ErrorDebugLine(e);
                return ResponseBase.ServerError<KernelOutputKeywordsResponse>(e.Message);
            }
        }

        [HttpPost]
        public ResponseBase RemoveKernelOutputKeyword(DataRequest<Guid> request) {
            if (request == null || request.Data == Guid.Empty) {
                return ResponseBase.InvalidInput("参数错误");
            }
            try {
                if (!IsValidAdmin(request, out ResponseBase response)) {
                    return response;
                }
                VirtualRoot.Execute(new RemoveKernelOutputKeywordCommand(request.Data));
                WebApiRoot.UpdateKernelOutputKeywordTimestamp(DateTime.Now);
                return ResponseBase.Ok();
            }
            catch (Exception e) {
                Logger.ErrorDebugLine(e);
                return ResponseBase.ServerError(e.Message);
            }
        }

        [HttpPost]
        public ResponseBase AddOrUpdateKernelOutputKeyword(DataRequest<KernelOutputKeywordData> request) {
            if (request == null || request.Data == null) {
                return ResponseBase.InvalidInput("参数错误");
            }
            try {
                if (!IsValidAdmin(request, out ResponseBase response)) {
                    return response;
                }
                if (request.Data.GetDataLevel() != DataLevel.Global) {
                    return ResponseBase.InvalidInput("添加到服务器的内核输出关键字记录的DataLevel属性必须赋值为Global");
                }
                VirtualRoot.Execute(new AddOrUpdateKernelOutputKeywordCommand(request.Data));
                WebApiRoot.UpdateKernelOutputKeywordTimestamp(DateTime.Now);
                return ResponseBase.Ok();
            }
            catch (Exception e) {
                Logger.ErrorDebugLine(e);
                return ResponseBase.ServerError(e.Message);
            }
        }
    }
}
