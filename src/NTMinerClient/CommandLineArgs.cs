﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace NTMiner {
    public static partial class CommandLineArgs {
        private static readonly List<string> s_commandLineArgs = Environment.GetCommandLineArgs().Skip(1).ToList();
        public static List<string> Args {
            get {
                return s_commandLineArgs;
            }
        }

        /// <summary>
        /// 提取格式形如argumentName=argumentValue格式的命令行参数。
        /// 注意：参数名是忽略大小写的，且如果命令行上有重名参数后面的值覆盖前面的值
        /// </summary>
        /// <param name="argumentName"></param>
        /// <returns></returns>
        private static string PickArgument(string argumentName) {
            string result = string.Empty;
            int index = -1;
            for (int i = 0; i < s_commandLineArgs.Count; i++) {
                string item = s_commandLineArgs[i];
                if (item.StartsWith(argumentName, StringComparison.OrdinalIgnoreCase)) {
                    string[] parts = item.Split('=');
                    if (parts.Length == 2) {
                        result = parts[1];
                        index = i;
                    }
                }
            }
            // 移除形如upgrade=格式的没有值的参数
            if (string.IsNullOrEmpty(result) && index != -1) {
                s_commandLineArgs.RemoveAt(index);
            }
            return result;
        }
    }
}
