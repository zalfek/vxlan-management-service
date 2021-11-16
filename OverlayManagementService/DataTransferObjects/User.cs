﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.DataTransferObjects
{
    public class User : IUser
    {
        public User(string name, string surename, string email, string oid, string ipAddress)
        {
            Name = name;
            Surename = surename;
            Email = email;
            this.oid = oid;
            IpAddress = ipAddress;
        }

        private string Name { get; set; }
        private string Surename { get; set; }
        private string Email { get; set; }
        private string oid { get; set; }
        private string IpAddress { get; set; }
    }
}
