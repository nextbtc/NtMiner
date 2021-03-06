﻿using NTMiner.User;
using System;
using System.Text;

namespace NTMiner.Core {
    public class MineWorkData : IMineWork, IDbEntity<Guid>, ITimestampEntity<Guid>, ISignableData {
        public MineWorkData() {
            this.CreatedOn = DateTime.Now;
        }

        public UserMineWorkData ToUserMineWork(string loginName) {
            return new UserMineWorkData {
                LoginName = loginName,
                Id = this.Id,
                CreatedOn = this.CreatedOn,
                Description = this.Description,
                ModifiedOn = this.ModifiedOn,
                Name = this.Name,
                ServerJsonSha1 = this.ServerJsonSha1
            };
        }

        public Guid GetId() {
            return this.Id;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ServerJsonSha1 { get; set; }

        public StringBuilder GetSignData() {
            return this.BuildSign();
        }
    }
}
