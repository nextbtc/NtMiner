﻿using NTMiner.Core;
using NTMiner.Core.MinerClient;
using NTMiner.Core.Profile;
using System.Collections.Generic;

namespace NTMiner.JsonDb {
    public interface IGpuProfilesJsonDb : IJsonDb {
        GpuType GpuType { get; }
        GpuData[] Gpus { get; }
        List<GpuProfileData> GpuProfiles { get; }
        List<CoinOverClockData> CoinOverClocks { get; }
    }
}
