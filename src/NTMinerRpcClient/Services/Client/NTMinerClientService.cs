﻿using NTMiner.Controllers;
using NTMiner.Core;
using NTMiner.Report;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace NTMiner.Services.Client {
    public class MinerClientService {
        public static readonly MinerClientService Instance = new MinerClientService();

        private readonly string _controllerName = RpcRoot.GetControllerName<IMinerClientController>();
        private MinerClientService() {
        }

        /// <summary>
        /// 本机网络调用
        /// </summary>
        public void ShowMainWindowAsync(Action<bool, Exception> callback) {
            RpcRoot.PostAsync(NTKeyword.Localhost, NTKeyword.MinerClientPort, _controllerName, nameof(IMinerClientController.ShowMainWindow), callback);
        }

        /// <summary>
        /// 本机同步网络调用
        /// </summary>
        public void CloseNTMinerAsync(Action callback) {
            string location = NTMinerRegistry.GetLocation(NTMinerAppType.MinerClient);
            if (string.IsNullOrEmpty(location) || !File.Exists(location)) {
                callback?.Invoke();
                return;
            }
            string processName = Path.GetFileNameWithoutExtension(location);
            if (Process.GetProcessesByName(processName).Length == 0) {
                callback?.Invoke();
                return;
            }
            RpcRoot.PostAsync(NTKeyword.Localhost, NTKeyword.MinerClientPort, _controllerName, nameof(IMinerClientController.CloseNTMiner), new SignRequest { }, (ResponseBase response, Exception e) => {
                if (!response.IsSuccess()) {
                    try {
                        Windows.TaskKill.Kill(processName, waitForExit: true);
                    }
                    catch (Exception ex) {
                        Logger.ErrorDebugLine(ex);
                    }
                }
                callback?.Invoke();
            }, timeountMilliseconds: 2000);
        }

        public void GetSpeedAsync(string clientIp, Action<SpeedData, Exception> callback) {
            RpcRoot.GetAsync(clientIp, NTKeyword.MinerClientPort, _controllerName, nameof(IMinerClientController.GetSpeed), null, callback, timeountMilliseconds: 3000);
        }

        public void WsGetSpeedAsync(Action<SpeedData, Exception> callback) {
            RpcRoot.GetAsync(NTKeyword.Localhost, NTKeyword.MinerClientPort, _controllerName, nameof(IMinerClientController.WsGetSpeed), null, callback, timeountMilliseconds: 3000);
        }

        public void GetConsoleOutLinesAsync(string clientIp, long afterTime, Action<List<ConsoleOutLine>, Exception> callback) {
            RpcRoot.GetAsync(clientIp, NTKeyword.MinerClientPort, _controllerName, nameof(IMinerClientController.GetConsoleOutLines), new Dictionary<string, string> {
                {"afterTime",afterTime.ToString() }
            }, callback, timeountMilliseconds: 3000);
        }

        public void GetLocalMessagesAsync(string clientIp, long afterTime, Action<List<LocalMessageDto>, Exception> callback) {
            RpcRoot.GetAsync(clientIp, NTKeyword.MinerClientPort, _controllerName, nameof(IMinerClientController.GetLocalMessages), new Dictionary<string, string> {
                {"afterTime",afterTime.ToString() }
            }, callback, timeountMilliseconds: 3000);
        }
    }
}
